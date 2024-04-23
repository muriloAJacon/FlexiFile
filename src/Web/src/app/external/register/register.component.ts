import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { CreateUserRequest } from 'src/app/shared/models/user/create-user-request.model';
import { User } from 'src/app/shared/models/user/user.model';
import { SettingsService } from 'src/app/shared/services/settings.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
	public form: FormGroup;

	public error: string | null = null;

	public anonymousRegisterAllowed: boolean = false;

	constructor(
		_formBuilder: FormBuilder,
		private userService: UserService,
		private spinnerService: NgxSpinnerService,
		private router: Router,
		private settingsService: SettingsService
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

	ngOnInit() {
		this.loadSettings();
	}

	loadSettings() {
		this.spinnerService.show('register');
		this.settingsService.getAllowAnonymousRegister().subscribe({
			next: (allowAnonymousRegister) => {
				if (!allowAnonymousRegister.anonymousRegisterAllowed) {
					this.router.navigate(['/login']);
				}
			},
			error: () => {
				this.error = "An error occured while attempting to load the settings.";
			}
		}).add(() => this.spinnerService.hide('register'));
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
				this.router.navigate(['/login'], {
					queryParams: {
						messageCode: user.approved ? 'accountCreated' : 'accountCreatedAwaitingApproval'
					}
				});
			},
			error: (error: HttpErrorResponse) => {
				this.error = error.error?.message ?? "Registration failed.";
			}
		}).add(() => this.spinnerService.hide('register'));
	}
}
