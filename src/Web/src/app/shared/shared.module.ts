import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertComponent } from './components/alert/alert.component';
import { ModalComponent } from './components/modal/modal.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CreateUserFormComponent } from './components/create-user-form/create-user-form.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
	declarations: [
		AlertComponent,
		ModalComponent,
		CreateUserFormComponent
	],
	imports: [
		CommonModule,
		FontAwesomeModule,
		ReactiveFormsModule
	],
	exports: [
		AlertComponent,
		ModalComponent,
		CreateUserFormComponent
	]
})
export class SharedModule { }
