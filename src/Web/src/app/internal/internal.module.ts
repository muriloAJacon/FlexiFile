import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InternalRoutingModule } from './internal-routing.module';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
	declarations: [
		HomeComponent,
		NavbarComponent
	],
	imports: [
		CommonModule,
		InternalRoutingModule,
		NgxSpinnerModule
	]
})
export class InternalModule { }
