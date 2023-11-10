import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InternalRoutingModule } from './internal-routing.module';
import { HomeComponent } from './home/home.component';


@NgModule({
	declarations: [
		HomeComponent
	],
	imports: [
		CommonModule,
		InternalRoutingModule
	]
})
export class InternalModule { }
