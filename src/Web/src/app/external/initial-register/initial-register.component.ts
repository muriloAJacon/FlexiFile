import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { CreateFirstSetupRequest } from 'src/app/shared/models/user/create-first-setup-request.model';
import { GetFirstSetupResponse } from 'src/app/shared/models/user/get-first-setup-response.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-initial-register',
  templateUrl: './initial-register.component.html',
  styleUrls: ['./initial-register.component.css']
})
export class InitialRegisterComponent implements OnInit {
	public form: FormGroup;

	public error: string | null = null;

	constructor(
		_formBuilder: FormBuilder,
		private userService: UserService,
		private spinnerService: NgxSpinnerService,
		private router: Router
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
				this.strongPasswordValidator
			])],
			confirmPassword: [null, Validators.compose([
				Validators.required,
				this.equalPasswordValidator.bind(this)
			])]
		});

		this.form.controls['password'].valueChanges.subscribe(() => this.form.controls['confirmPassword'].updateValueAndValidity());
	}

	ngOnInit() {
		this.getIsFirstSetup();
	}

	getIsFirstSetup() {
		this.spinnerService.show('firstSetup');
		this.userService.getFirstSetup().subscribe({
			next: (result: GetFirstSetupResponse) => {
				if (!result.firstSetupPending) {
					this.router.navigate(['/login']);
				}
			},
			error: () => {
				this.error = "An error occured while trying to get the first setup.";
			}
		}).add(() => this.spinnerService.hide('firstSetup'));
	}

	strongPasswordValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value as string | null;

		if (value === null) {
			return null;
		}

		let errors: ValidationErrors = {};

		if (!/[A-Z]/.test(value)) {
			errors["noUpperCase"] = true;
		}

		if (!/[0-9]/.test(value)) {
			errors["noNumber"] = true;
		}

		return errors;
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

		const registerData: CreateFirstSetupRequest = {
			name: controls['name'].value,
			email: controls['email'].value,
			password: controls['password'].value,
		};

		this.spinnerService.show('firstSetup');
		this.userService.registerFirstSetup(registerData).subscribe({
			next: () => {
				this.router.navigate(['/login']);
			},
			error: () => {
				this.error = "An error occured while trying to register the first setup.";
			}
		}).add(() => this.spinnerService.hide('firstSetup'));
	}
}
