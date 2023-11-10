import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './shared/helpers/auth.guard';

const routes: Routes = [
	{
		path: "",
		loadChildren: () => import("./internal/internal-routing.module").then(m => m.InternalRoutingModule),
		canActivate: [authGuard],
	},
	{
		path: "",
		loadChildren: () => import("./external/external-routing.module").then(m => m.ExternalRoutingModule),
	},
	{
		path: "**",
		redirectTo: ""
	}
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
