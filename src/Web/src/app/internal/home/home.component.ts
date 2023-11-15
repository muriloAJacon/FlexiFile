import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css']
})
export class HomeComponent {
	constructor(
		private spinnerService: NgxSpinnerService,
	) {

	}

	ngOnInit() {
	}

	fileInputChange(event: Event) {
		const files = (event.target as HTMLInputElement).files;
		if (files) {
			this.uploadFiles(files);
		}
	}

	uploadFiles(files: FileList) {
		console.log('upload', files);
	}
}
