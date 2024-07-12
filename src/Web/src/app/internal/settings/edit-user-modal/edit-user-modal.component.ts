import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewChecked, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { EditUserRequest } from 'src/app/shared/models/user/edit-user-request.model';
import { User } from 'src/app/shared/models/user/user.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-edit-user-modal[user]',
  templateUrl: './edit-user-modal.component.html',
  styleUrls: ['./edit-user-modal.component.css']
})
export class EditUserModalComponent implements OnInit, AfterViewChecked {
	@Input() user!: User;

	@Output('userEdited') userEdited = new EventEmitter<User>();
	@Output('modalClosed') modalClosed = new EventEmitter<void>();

	@ViewChild('editUserModal') editUserModal!: ModalComponent;

	public error: string | null = null;

	public form: FormGroup;

	public sizeUnits = FormsHelper.sizeUnits;

	constructor(
		_formBuilder: FormBuilder,
		private userService: UserService,
		private spinnerService: NgxSpinnerService
	) {
		this.form = _formBuilder.group({
			name: [null, Validators.compose([
				Validators.required,
				Validators.maxLength(250)
			])],
			password: [null, Validators.compose([
				FormsHelper.strongPasswordValidator
			])],
			confirmPassword: [null, Validators.compose([
				this.equalPasswordValidator.bind(this)
			])],
			softStorageLimitNumber: [null],
			softStorageLimitUnit: [null],
			hardStorageLimitNumber: [null],
			hardStorageLimitUnit: [null],
		});
	}

	ngOnInit() {
		const softStorageLimitNumber = this.user.storageLimit ?? 0;
		const softStorageSizeUnit = FormsHelper.getSizeUnit(softStorageLimitNumber);

		const hardStorageLimitNumber = this.user.hardStorageLimit ?? 0;
		const hardStorageSizeUnit = FormsHelper.getSizeUnit(hardStorageLimitNumber);

		this.form.setValue({
			name: this.user.name,
			password: null,
			confirmPassword: null,
			softStorageLimitNumber: softStorageLimitNumber / softStorageSizeUnit.value,
			softStorageLimitUnit: softStorageSizeUnit.value,
			hardStorageLimitNumber: hardStorageLimitNumber / hardStorageSizeUnit.value,
			hardStorageLimitUnit: hardStorageSizeUnit.value,
		});
	}

	ngAfterViewChecked() {
		this.editUserModal.open();
		this.editUserModal.onHide.subscribe(() => {
			this.modalClosed.emit();
		});
	}

	equalPasswordValidator(control: AbstractControl): ValidationErrors | null {
		if (this.form) {
			const password = this.form.controls["password"].value;
			const repeatPassword = control.value;

			if (password !== repeatPassword) {
				return { notEqual: true };
			}
		}

		return null;
	}

	submit() {
		this.error = null;

		FormsHelper.markFormAsDirty(this.form);
		if (!this.form.valid) {
			return;
		}

		const controls = this.form.controls;

		let password = controls['password'].value;
		let softStorageLimit = controls['softStorageLimitNumber'].value * controls['softStorageLimitUnit'].value;
		let hardStorageLimit = controls['hardStorageLimitNumber'].value * controls['hardStorageLimitUnit'].value;
		const data: EditUserRequest = {
			id: this.user.id,
			name: controls['name'].value,
			password: password ? password : null,
			storageLimit: softStorageLimit === 0 ? null : softStorageLimit,
			hardStorageLimit: hardStorageLimit === 0 ? null : hardStorageLimit
		};

		this.spinnerService.show('register');
		this.userService.editUser(data).subscribe({
			next: (user: User) => {
				this.form.reset();
				this.userEdited.emit(user);

				this.editUserModal.close();
			},
			error: (error: HttpErrorResponse) => {
				this.error = error.error?.message ?? "Failed to edit user.";
			}
		}).add(() => this.spinnerService.hide('register'));
	}
}
