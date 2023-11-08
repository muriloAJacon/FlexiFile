import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InitialRegisterComponent } from './initial-register/initial-register.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
	{
		path: "first-setup",
		component: InitialRegisterComponent
	},
	{
		path: "login",
		component: LoginComponent
	},
	{
		path: "**",
		redirectTo: "login"
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class ExternalRoutingModule { }
