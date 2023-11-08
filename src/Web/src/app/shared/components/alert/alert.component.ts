import { Component, Input } from '@angular/core';
import { AlertType } from '../../models/alert-type.enum';

@Component({
	selector: 'app-alert',
	templateUrl: './alert.component.html',
	styleUrls: ['./alert.component.css']
})
export class AlertComponent {
	@Input('type') type: AlertType = AlertType.Error;
	@Input('class') class: string = '';

	public AlertType = AlertType;

	public get isSuccess(): boolean {
		return this.type === AlertType.Success;
	}

	public get isError(): boolean {
		return this.type === AlertType.Error;
	}

	public get backgroundClasses() {
		const classes: { [key in AlertType]: string } = {
			[AlertType.Success]: 'from-green-600 to-green-400',
			[AlertType.Error]: 'from-red-600 to-red-400',
			[AlertType.Info]: 'from-blue-600 to-blue-400'
		};

		return classes[this.type];
	}
}
