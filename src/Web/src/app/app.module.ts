import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { ExternalModule } from './external/external.module';
import { InternalModule } from './internal/internal.module';

@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
		SharedModule,
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
		FontAwesomeModule,
		BrowserAnimationsModule,
		NgxSpinnerModule.forRoot({ type: 'ball-clip-rotate-pulse' }),
		ExternalModule,
		InternalModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
