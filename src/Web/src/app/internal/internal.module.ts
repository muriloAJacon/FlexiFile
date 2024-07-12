import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InternalRoutingModule } from './internal-routing.module';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DragDropDirective } from './drag-drop/drag-drop.directive';
import { SettingsComponent } from './settings/settings.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { EditUserModalComponent } from './settings/edit-user-modal/edit-user-modal.component';


@NgModule({
	declarations: [
		HomeComponent,
		NavbarComponent,
		DragDropDirective,
		SettingsComponent,
		EditUserModalComponent
	],
	imports: [
		SharedModule,
		CommonModule,
		InternalRoutingModule,
		ReactiveFormsModule,
		NgxSpinnerModule,
		FontAwesomeModule,
		NgxDatatableModule
	]
})
export class InternalModule { }
