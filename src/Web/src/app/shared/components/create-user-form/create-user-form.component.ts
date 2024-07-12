import { Component, EventEmitter, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormsHelper } from '../../helpers/forms-helper';
import { CreateUserRequest } from '../../models/user/create-user-request.model';
import { User } from '../../models/user/user.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-user-form',
  templateUrl: './create-user-form.component.html',
  styleUrls: ['./create-user-form.component.css']
})
export class CreateUserFormComponent {

	@Output() userCreated = new EventEmitter<User>();

	public form: FormGroup;

	public error: string | null = null;

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
				Validators.required,
				FormsHelper.strongPasswordValidator
			])],
			confirmPassword: [null, Validators.compose([
				Validators.required,
				this.equalPasswordValidator.bind(this)
			])]
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

		const registerData: CreateUserRequest = {
			name: controls['name'].value,
			email: controls['email'].value,
			password: controls['password'].value
		};

		this.spinnerService.show('register');
		this.userService.createUser(registerData).subscribe({
			next: (user: User) => {
				this.userCreated.emit(user);
			},
			error: (error: HttpErrorResponse) => {
				this.error = error.error?.message ?? "Registration failed.";
			}
		}).add(() => this.spinnerService.hide('register'));
	}
}
