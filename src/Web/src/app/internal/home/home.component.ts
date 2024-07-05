import { HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { catchError, last, tap } from 'rxjs';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { ConversionType } from 'src/app/shared/models/file-conversion/conversion-type.model';
import { FileConversionRequest } from 'src/app/shared/models/file-conversion/file-conversion-request.model';
import { FileConversion } from 'src/app/shared/models/file-conversion/file-conversion.model';
import { ConversionStatus } from 'src/app/shared/models/file/conversion-status.enum';
import { FileModel } from 'src/app/shared/models/file/file-model.model';
import { FileType } from 'src/app/shared/models/file/file-type.enum';
import { InternalFileStatus } from 'src/app/shared/models/file/internal-file-status.enum';
import { FileService } from 'src/app/shared/services/file.service';
import { environment } from 'src/environments/environment';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css']
})
export class HomeComponent {

	recentFiles: any[] = [];

	public InternalFileStatus = InternalFileStatus;

	@ViewChild('convertModal') convertModal!: ModalComponent;

	public conversions: ConversionType[] = [];
	public selectedFile: FileModel | null = null;

	public ConversionStatus = ConversionStatus;

	public convertModalError: string | null = null;

	public baseFilePath = environment.baseFilePath

	public selectedFileConversion: ConversionType | null = null;
	public modalStep: ConvertModalStep = ConvertModalStep.Select;
	public ConvertModalStep = ConvertModalStep;

	public filesSelectForm: FormArray<FormGroup>;
	public pagesSelectForm: FormControl<string | null> = new FormControl<string | null>(null);

	public showOlderFiles = false;
	public olderFiles: FileModel[] = [];

	public FileType = FileType;

	constructor(
		private spinnerService: NgxSpinnerService,
		private fileService: FileService,
		private formBuilder: FormBuilder
	) {
		this.filesSelectForm = formBuilder.array<FormGroup>([]);
	}

	ngOnInit() {
	}

	fileInputChange(event: Event) {
		const input = event.target as HTMLInputElement;
		const files = input.files;
		if (files) {
			this.uploadFiles(files);
			input.value = '';
		}
	}

	uploadFiles(files: FileList) {
		for (let i = 0; i < files.length; i++) {
			const file = files[i];

			const fileProgress = {
				fileName: file.name,
				uploadPercentageComplete: 0,
				status: InternalFileStatus.Start,
				error: null,
				fileModel: null
			};
			this.recentFiles.unshift(fileProgress);

			this.fileService.startFileUpload({
				fileName: file.name,
				fileSize: file.size,
				mimeType: file.type
			}).subscribe({
				next: (fileModel) => {
					fileProgress.status = InternalFileStatus.Uploading;
					// @ts-ignore TODO: REMOVE
					fileProgress.fileModel = fileModel;
					this.uploadFile(file, fileModel, fileProgress);
				},
				error: (error) => {
					fileProgress.status = InternalFileStatus.Failed;
					fileProgress.error = error.error?.message ?? "Failed to prepare file for upload";
				}
			});
		}
	}

	uploadFile(file: File, fileModel: FileModel, fileProgress: any) {
		this.fileService.uploadFile(file, fileModel.id).pipe(
			tap(message => this.showProgress(fileProgress, message)),
			last(),
			catchError(error => {
				fileProgress.error = error.error?.message ?? "Failed to upload file";
				fileProgress.status = InternalFileStatus.Failed;
				return error;
			})
			).subscribe({
				next: (response) => {
					fileProgress.status = InternalFileStatus.Complete;
					const data = response as HttpResponse<FileModel>;
					fileProgress.fileModel = data.body;
				}
			});
	}

	loadOlderFiles() {
		this.showOlderFiles = true;
		const ignoreIds = this.recentFiles.filter(x => x.fileModel !== null).map(x => x.fileModel.id);

		this.spinnerService.show('olderFiles');
		this.fileService.getFiles(ignoreIds).subscribe({
			next: (files) => {
				this.olderFiles = files;
			},
			error: (error) => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('olderFiles'));
	}

	getStatusText(file: any) {
		const templates: {[key in InternalFileStatus]: string} = {
			[InternalFileStatus.Start]: 'Preparing upload "{fileName}"',
			[InternalFileStatus.Uploading]: 'Uploading "{fileName}"',
			[InternalFileStatus.Complete]: '"{fileName}" upload complete',
			[InternalFileStatus.Failed]: 'Failed to upload "{fileName}"'
		}

		return templates[file.status as InternalFileStatus].replace('{fileName}', file.fileName); // TODO: REMOVE 'AS' WHEN PROPER TYPE IS DONE
	}

	showProgress(fileProgress: any, message: HttpEvent<unknown>) {
		if (message.type == HttpEventType.UploadProgress) {
			const loaded = message.loaded;
			const total = message.total!;
			const percent = Math.round(100 * loaded / total);
			fileProgress.uploadPercentageComplete = percent;
		}
	}

	openConvertModal(fileModel: FileModel) {
		this.modalStep = ConvertModalStep.Select;
		this.convertModalError = null;
		this.conversions = [];

		this.convertModal.open();
		this.spinnerService.show('convertType');
		this.fileService.getAvailableConversions(fileModel.mimeType).subscribe({
			next: (conversions) => {
				this.conversions = conversions;
				this.selectedFile = fileModel;
			},
			error: (error) => {
				this.convertModalError = error.error?.message ?? "Failed to load available conversions"
			}
		}).add(() => this.spinnerService.hide('convertType'));
	}

	selectFileConversion(conversion: ConversionType) {
		this.convertModalError = null;
		const selectedFile = this.selectedFile!;
		this.selectedFileConversion = conversion;

		if (conversion.minNumberFiles !== null && conversion.minNumberFiles > 1) {
			this.modalStep = ConvertModalStep.MultiSelect;
			
			const files = this.recentFiles.filter(x => x.fileModel !== null);
			this.filesSelectForm = this.formBuilder.array(files.map(x => this.formBuilder.group({
				fileId: [x.fileModel.id],
				selected: [x.fileModel.id == selectedFile.id ? true : false],
				position: [x.fileModel.id == selectedFile.id ? 1 : null]
			})));
		} else if (conversion.modelClassName !== null) {
			this.modalStep = ConvertModalStep.PageNumber;
			this.pagesSelectForm.reset();
		} else {
			this.requestFileConvert([selectedFile.id], conversion.id);
		}
	}

	getFile(fileId: string) {
		return this.recentFiles.find(x => x.fileModel?.id == fileId);
	}

	getOrderArray(): number[] {
		const selectedFiles = this.filesSelectForm.controls.filter(x => x.controls['selected'].value);
		return selectedFiles.map((x, idx) => idx + 1);
	}

	getControl(control: AbstractControl) {
		return control as FormControl;
	}

	submitMultiSelect() {
		this.convertModalError = null;

		const selectedFiles = this.filesSelectForm.controls.filter(x => x.controls['selected'].value);
		if (selectedFiles.length < 2) {
			this.convertModalError = 'At least two files must be selected';
			return;
		}

		const fileIds = selectedFiles.sort((a, b) => a.controls['position'].value - b.controls['position'].value).map(x => x.controls['fileId'].value);
		this.requestFileConvert(fileIds, this.selectedFileConversion!.id);
	}

	submitPageNumbers() {
		const value = this.pagesSelectForm.value;

		if (value === null || value === '') {
			this.convertModalError = 'Please enter at least one page';
			return;
		}

		const pages = value.split(',').map(x => parseInt(x));

		const parameters = {
			originalPageNumbers: pages
		};

		this.requestFileConvert([this.selectedFile!.id], this.selectedFileConversion!.id, parameters);
	}

	requestFileConvert(fileIds: string[], conversionId: number, extraParameters: any | null = null) {
		this.convertModalError = null;

		const convertRequest: FileConversionRequest = {
			fileIds: fileIds,
			conversionId,
			extraParameters: extraParameters
		};

		this.spinnerService.show('convertType');
		this.fileService.convertFile(convertRequest).subscribe({
			next: (fileConversion: FileConversion) => {
				this.updateFile(this.selectedFile!.id, fileConversion.id);
				this.convertModal.close();
			},
			error: (error) => {
				this.convertModalError = error.error?.message ?? "Failed to request convert for file"
			}
		}).add(() => this.spinnerService.hide('convertType'));
	}

	// TODO: This is a temporary method before SignalR is implemented
	updateFile(fileId: string, conversionId: string) {
		const interval = setInterval(() => {
			this.fileService.getFile(fileId).subscribe({
				next: (fileModel) => {
					const file = this.recentFiles.find(f => f.fileModel.id == fileId);
					if (file) {
						file.fileModel = fileModel;
						const conversion = fileModel.conversions.find(c => c.id == conversionId);
						if (conversion && [ConversionStatus.Completed, ConversionStatus.Failed].includes(conversion.status)) {
							clearInterval(interval);
						}
					}

					this.recentFiles = [...this.recentFiles];
				}
			});
		}, 1000);
	}

	openFile(fileId: string, fileType: FileType, download: boolean) {
		this.spinnerService.show('fileDownload');
		this.fileService.getFileToken({
			fileId,
			fileType
		}).subscribe({
			next: (response) => {
				const url = `${this.baseFilePath}/${response.token}?download=${download}`;
				window.open(url, '_blank');
			},
			error: (error) => {
				// TODO: HANDLE
			}
		}).add(() => this.spinnerService.hide('fileDownload'));
	}
}

enum ConvertModalStep {
	Select = 'Select',
	MultiSelect = 'MultiSelect',
	PageNumber = 'PageNumber'
}