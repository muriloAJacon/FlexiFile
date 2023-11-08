import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { LoginRequest } from 'src/app/shared/models/auth/login-request.model';
import { GetFirstSetupResponse } from 'src/app/shared/models/user/get-first-setup-response.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
	public form: FormGroup;

	public error: string | null = null;

	constructor(
		_formBuilder: FormBuilder,
		private userService: UserService,
		private spinnerService: NgxSpinnerService,
		private router: Router,
		private authService: AuthService
	) {
		this.form = _formBuilder.group({
			email: [null, Validators.compose([
				Validators.required,
				Validators.email
			])],
			password: [null, Validators.compose([
				Validators.required
			])]
		});
	}

	ngOnInit() {
		this.getIsFirstSetup();
	}

	getIsFirstSetup() {
		this.spinnerService.show('login');
		this.userService.getFirstSetup().subscribe({
			next: (result: GetFirstSetupResponse) => {
				if (result.firstSetupPending) {
					this.router.navigate(['/first-setup']);
				}
			},
			error: () => {
				this.error = "An error occured while trying to get the first setup.";
			}
		}).add(() => this.spinnerService.hide('login'));
	}

	submit() {
		this.error = null;

		FormsHelper.markFormAsDirty(this.form);
		if (!this.form.valid) {
			return;
		}

		const controls = this.form.controls;

		const loginData: LoginRequest = {
			email: controls['email'].value,
			password: controls['password'].value,
		};

		this.spinnerService.show('login');
		this.authService.login(loginData).subscribe({
			next: () => {
				// TODO: Save token
			},
			error: (error: HttpErrorResponse) => {
				this.error = error.error?.message ?? "Login failed.";
			}
		}).add(() => this.spinnerService.hide('login'));
	}
}
