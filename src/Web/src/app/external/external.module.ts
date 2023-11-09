import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExternalRoutingModule } from './external-routing.module';
import { InitialRegisterComponent } from './initial-register/initial-register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
	declarations: [
		InitialRegisterComponent,
		LoginComponent,
		RegisterComponent
	],
	imports: [
		SharedModule,
		CommonModule,
		ExternalRoutingModule,
		ReactiveFormsModule,
		NgxSpinnerModule
	]
})
export class ExternalModule { }
