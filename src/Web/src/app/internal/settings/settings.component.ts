import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { NgxSpinnerService } from 'ngx-spinner';
import { AccessLevel } from 'src/app/shared/models/user/access-level.enum';
import { ApproveUserRequest } from 'src/app/shared/models/user/approve-user-request.model';
import { User } from 'src/app/shared/models/user/user.model';
import { SettingsService } from 'src/app/shared/services/settings.service';
import { UserService } from 'src/app/shared/services/user.service';
import { forkJoin } from 'rxjs';
import { ChangeGlobalMaxFileSize } from 'src/app/shared/models/settings/change-global-max-file-size.model';
import { ChangeAllowAnonymousRegister } from 'src/app/shared/models/settings/change-allow-anonymous-register.model';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
	selector: 'app-settings',
	templateUrl: './settings.component.html',
	styleUrls: ['./settings.component.css']
})
export class SettingsComponent {

	@ViewChild('newUserModal') newUserModal!: ModalComponent;

	public allowAnonymousRegisterControl: FormControl<boolean>;

	public maxFileSizeForm: FormGroup;

	public users: User[] = [];
	public columnMode: ColumnMode = ColumnMode.force;
	public messages = {
		emptyMessage: 'No users found',
		totalMessage: 'user(s) in total'
	};

	public AccessLevel = AccessLevel;

	public sizeUnits = [
		{ value: 1, description: "B" },
		{ value: 1000, description: "KB" },
		{ value: 1000000, description: "MB" },
		{ value: 1000000000, description: "GB" },
	];

	public editingUser: User | null = null;

	constructor(
		private spinnerService: NgxSpinnerService,
		_formBuilder: FormBuilder,
		private userService: UserService,
		private settingsService: SettingsService
	) {
		this.allowAnonymousRegisterControl = _formBuilder.nonNullable.control(false);

		this.maxFileSizeForm = _formBuilder.group({
			sizeNumber: [0],
			sizeUnit: [1]
		});
	}

	ngOnInit() {
		this.allowAnonymousRegisterControl.valueChanges.subscribe(this.changeAnonymousRegister.bind(this));

		this.loadUsers();
		this.loadSettings();
	}

	loadUsers() {
		this.spinnerService.show('users');
		this.userService.getUsers().subscribe({
			next: (users) => {
				this.users = users;
			},
			error: (error) => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('users'));
	}

	loadSettings() {
		this.spinnerService.show('settings');

		const allowAnonymousRegisterCall = this.settingsService.getAllowAnonymousRegister();
		const maxFileSizeCall = this.settingsService.getGlobalMaxFileSize();

		forkJoin([allowAnonymousRegisterCall, maxFileSizeCall]).subscribe({
			next: ([allowAnonymousRegister, maxFileSize]) => {
				this.allowAnonymousRegisterControl.setValue(allowAnonymousRegister.anonymousRegisterAllowed, { emitEvent: false });

				const sizeUnitsCopy = [...this.sizeUnits];
				sizeUnitsCopy.sort((a, b) => b.value - a.value);
				const maxFileSizeNumber = maxFileSize.maxFileSize;
				for (let i = 0; i < sizeUnitsCopy.length; i++) {
					if (maxFileSizeNumber % sizeUnitsCopy[i].value !== maxFileSizeNumber) {
						this.maxFileSizeForm.setValue({
							sizeNumber: maxFileSizeNumber / sizeUnitsCopy[i].value,
							sizeUnit: sizeUnitsCopy[i].value
						});
						break;
					}
				}
			},
			error: (error) => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('settings'));
	}

	changeAnonymousRegister(newValue: boolean) {
		const data: ChangeAllowAnonymousRegister = {
			allowAnonymousRegister: newValue
		};

		this.spinnerService.show('settings');
		this.settingsService.changeAllowAnonymousRegister(data).subscribe({
			error: () => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('settings'));
	}

	saveMaxFileSize() {
		if (!this.maxFileSizeForm.valid) {
			return;
		}

		const fileSize = (this.maxFileSizeForm.value.sizeNumber ?? 0) * this.maxFileSizeForm.value.sizeUnit;
		const data: ChangeGlobalMaxFileSize = {
			maxFileSize: fileSize
		};

		this.spinnerService.show('settings');
		this.settingsService.changeGlobalMaxFileSize(data).subscribe({
			error: () => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('settings'));
	}

	grantAccess(user: User) {
		this.spinnerService.show('users');
		const data: ApproveUserRequest = {
			id: user.id
		};
		this.userService.approveUser(data).subscribe({
			next: (user) => {
				const userIndex = this.users.findIndex(x => x.id === user.id);
				if (userIndex) {
					this.users[userIndex] = user;
					this.users = [...this.users];
				}
			},
			error: (error) => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('users'));
	}

	onUserCreated(user: User) {
		this.users.unshift(user);
		this.users = [...this.users];
		this.newUserModal.close();
	}
}
