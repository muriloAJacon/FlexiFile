import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { catchError, last, tap } from 'rxjs';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { ConversionType } from 'src/app/shared/models/file-conversion/conversion-type.model';
import { FileConversionRequest } from 'src/app/shared/models/file-conversion/file-conversion-request.model';
import { FileConversion } from 'src/app/shared/models/file-conversion/file-conversion.model';
import { ConversionStatus } from 'src/app/shared/models/file/conversion-status.enum';
import { FileModel } from 'src/app/shared/models/file/file-model.model';
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

	constructor(
		private spinnerService: NgxSpinnerService,
		private fileService: FileService
	) {

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
			console.log(file)

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
				next: () => {
					// TODO: GET FILE MODEL AGAIN FROM RESPONSE
					// fileProgress.fileModel = fileModel;
					fileProgress.status = InternalFileStatus.Complete;
				}
			});
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

	requestFileConvert(conversionId: number) {
		this.convertModalError = null;

		const file = this.selectedFile!;
		const convertRequest: FileConversionRequest = {
			fileIds: [file.id],
			conversionId,
			extraParameters: null
		};

		this.spinnerService.show('convertType');
		this.fileService.convertFile(convertRequest).subscribe({
			next: (fileConversion: FileConversion) => {
				this.updateFile(file.id, fileConversion.id);
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
}
