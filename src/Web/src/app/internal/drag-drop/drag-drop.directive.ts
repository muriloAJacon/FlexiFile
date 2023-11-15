import { Directive, HostListener, HostBinding, Output, EventEmitter, Input } from '@angular/core';

@Directive({
	selector: '[appDragDrop]'
})
export class DragDropDirective {

	@Input('backdrop') backdrop: HTMLElement | undefined; 
	@Output() filesDropped = new EventEmitter<FileList>();

	constructor() { }

	ngOnInit() {
		this.fileOver = false;
	}

	private _fileOver: boolean = false;
	public get fileOver(): boolean {
		return this._fileOver;
	}
	public set fileOver(value: boolean) {
		if (value) {
			this.backdrop?.classList.remove('hidden');
			this.backdrop?.classList.add('flex');
		} else {
			this.backdrop?.classList.add('hidden');
			this.backdrop?.classList.remove('flex');
		}
		this._fileOver = value;
	}

	@HostListener('dragover', ['$event']) onDragOver(evt: Event) {
		evt.preventDefault();
		evt.stopPropagation();
		this.fileOver = true;
	}

	@HostListener('dragleave', ['$event']) public onDragLeave(evt: Event) {
		evt.preventDefault();
		evt.stopPropagation();
		this.fileOver = false;
		console.log('dragleave');
	}

	@HostListener('drop', ['$event']) public onDrop(evt: DragEvent) {
		evt.preventDefault();
		evt.stopPropagation();

		this.fileOver = false;

		const files = evt.dataTransfer?.files;
		if (files && files.length > 0) {
			this.filesDropped.emit(files);
		}
	}
}
