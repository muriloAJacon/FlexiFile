import { Component, EventEmitter, Output } from '@angular/core';

@Component({
	selector: 'app-modal',
	templateUrl: './modal.component.html',
	styleUrls: ['./modal.component.css']
})
export class ModalComponent {
	@Output() onHide: EventEmitter<void> = new EventEmitter<void>();
	@Output() onShow: EventEmitter<void> = new EventEmitter<void>();

	protected hidden: boolean = true;

	open(): void {
		this.onShow.emit();
		this.hidden = false;
	}

	close(): void {
		this.onHide.emit();
		this.hidden = true;
	}
}
