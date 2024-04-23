import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertComponent } from './components/alert/alert.component';
import { ModalComponent } from './components/modal/modal.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';



@NgModule({
	declarations: [
		AlertComponent,
		ModalComponent
	],
	imports: [
		CommonModule,
		FontAwesomeModule
	],
	exports: [
		AlertComponent,
		ModalComponent
	]
})
export class SharedModule { }
