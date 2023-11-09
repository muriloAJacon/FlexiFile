import { Location } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DateTime } from 'luxon';
import { NgxSpinnerService } from 'ngx-spinner';
import { forkJoin } from 'rxjs';
import { FormsHelper } from 'src/app/shared/helpers/forms-helper';
import { AlertType } from 'src/app/shared/models/alert-type.enum';
import { LoginRequest } from 'src/app/shared/models/auth/login-request.model';
import { LoginResponse } from 'src/app/shared/models/auth/login-response.model';
import { GetFirstSetupResponse } from 'src/app/shared/models/user/get-first-setup-response.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { SettingsService } from 'src/app/shared/services/settings.service';
import { TokenService } from 'src/app/shared/services/token.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
	public form: FormGroup;

	public success: string | null = null;
	public error: string | null = null;
	public AlertType = AlertType;

	public anonymousRegisterAllowed: boolean = false;

	constructor(
		_formBuilder: FormBuilder,
		private userService: UserService,
		private spinnerService: NgxSpinnerService,
		private router: Router,
		private authService: AuthService,
		private tokenService: TokenService,
		private settingsService: SettingsService,
		private route: ActivatedRoute,
		private location: Location
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
		this.loadSettings();

		this.route.queryParamMap.subscribe(params => {
			const messageCode = params.get('messageCode');
			if (messageCode !== null) {
				this.loadSuccessMessage(messageCode);
				this.location.replaceState('/login');
			}
		});
	}

	loadSuccessMessage(messageCode: string) {
		const messages: { [key: string]: string } = {
			"firstSetupCompleted": "First setup was completed successfully. You can now login.",
			"accountCreated": "Your account was succesfully created. You can now login.",
			"accountCreatedAwaitingApproval": "Your account was succesfully created. You may wait for the approval of an administrator before logging in.",
		};
		this.success = messages[messageCode];
	}

	loadSettings() {
		this.spinnerService.show('login');
		const getFirstSetup = this.userService.getFirstSetup();
		const getAllowAnonymousRegister = this.settingsService.getAllowAnonymousRegister();

		forkJoin([getFirstSetup, getAllowAnonymousRegister]).subscribe({
			next: ([firstSetup, allowAnonymousRegister]) => {
				if (firstSetup.firstSetupPending) {
					this.router.navigate(['/first-setup']);
				}

				this.anonymousRegisterAllowed = allowAnonymousRegister.anonymousRegisterAllowed;
			},
			error: () => {
				this.error = "An error occured while attempting to load the settings.";
			}
		}).add(() => this.spinnerService.hide('login'));
	}

	submit() {
		this.success = null;
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
			next: (loginInfo: LoginResponse) => {
				this.tokenService.saveJwtToken(loginInfo.token, DateTime.fromISO(loginInfo.expiresAt));
				this.tokenService.saveRefreshToken(loginInfo.refreshToken, DateTime.fromISO(loginInfo.refreshTokenExpiresAt));

				this.router.navigate(['/upload']);
			},
			error: (error: HttpErrorResponse) => {
				this.error = error.error?.message ?? "Login failed.";
			}
		}).add(() => this.spinnerService.hide('login'));
	}
}
