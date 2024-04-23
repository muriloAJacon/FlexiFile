import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InitialRegisterComponent } from './initial-register/initial-register.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { environment } from 'src/environments/environment';

const routes: Routes = [
	{
		path: "first-setup",
		component: InitialRegisterComponent,
		data: { title: `First Setup · ${environment.baseTitle}` }
	},
	{
		path: "login",
		component: LoginComponent,
		data: { title: `Login · ${environment.baseTitle}` }
	},
	{
		path: "register",
		component: RegisterComponent,
		data: { title: `Register · ${environment.baseTitle}` }
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class ExternalRoutingModule { }
