import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from 'src/app/shared/models/user/user.model';
import { SettingsService } from 'src/app/shared/services/settings.service';
import { TokenService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

	public error: string | null = null;

	constructor(
		private router: Router,
		private settingsService: SettingsService,
		private spinnerService: NgxSpinnerService,
		private tokenService: TokenService
	) {
	}

	ngOnInit() {
		// Force user log out, since this page is intended for anonymous registration, and using it while logged in could lead to unexpected results.
		this.tokenService.signOut();

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

	onUserCreated(user: User) {
		this.router.navigate(['/login'], {
			queryParams: {
				messageCode: user.approved ? 'accountCreated' : 'accountCreatedAwaitingApproval'
			}
		});
	}

}
