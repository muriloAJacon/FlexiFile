import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { User } from 'src/app/shared/models/user/user.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-edit-user-modal[user]',
  templateUrl: './edit-user-modal.component.html',
  styleUrls: ['./edit-user-modal.component.css']
})
export class EditUserModalComponent implements OnInit, AfterViewInit {
	@Input() user!: User;

	@ViewChild('editUserModal') editUserModal!: ModalComponent;

	public error: string | null = null;

	public form: FormGroup;

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
			email: [null, Validators.compose([
				Validators.required,
				Validators.email
			])],
			password: [null, Validators.compose([
				FormsHelper.strongPasswordValidator
			])],
			confirmPassword: [null, Validators.compose([
				this.equalPasswordValidator.bind(this)
			])]
		});
	}

	ngOnInit() {
		this.form.setValue({
			name: this.user.name,
			email: this.user.email,
			password: null,
			confirmPassword: null
		});

	}

	ngAfterViewInit() {
		this.editUserModal.open();
		// TODO: HANDLE MODAL CLOSE
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
		// TODO
	}
}
