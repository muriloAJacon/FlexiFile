import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { environment } from 'src/environments/environment';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
	{
		path: "",
		component: HomeComponent,
		data: { title: `Home · ${environment.baseTitle}` }
	},
	{
		path: "settings",
		component: SettingsComponent,
		data: { title: `Settings · ${environment.baseTitle}` }
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class InternalRoutingModule { }
