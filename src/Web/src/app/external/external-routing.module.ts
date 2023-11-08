import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InitialRegisterComponent } from './initial-register/initial-register.component';

const routes: Routes = [
	{
		path: "first-setup",
		component: InitialRegisterComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class ExternalRoutingModule { }
