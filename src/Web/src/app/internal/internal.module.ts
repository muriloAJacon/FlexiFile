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


@NgModule({
	declarations: [
		HomeComponent,
		NavbarComponent,
		DragDropDirective
	],
	imports: [
		SharedModule,
		CommonModule,
		InternalRoutingModule,
		ReactiveFormsModule,
		NgxSpinnerModule,
		FontAwesomeModule
	]
})
export class InternalModule { }
