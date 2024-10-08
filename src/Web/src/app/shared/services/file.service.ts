import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { StartFileUploadRequest } from '../models/file/start-file-upload-request.model';
import { FileModel } from '../models/file/file-model.model';
import { ConversionType } from '../models/file-conversion/conversion-type.model';
import { FileConversionRequest } from '../models/file-conversion/file-conversion-request.model';
import { FileConversion } from '../models/file-conversion/file-conversion.model';
import { FileTokenRequest } from '../models/file/file-token-request.model';
import { FileTokenResponse } from '../models/file/file-token-result.model';

@Injectable({
	providedIn: 'root'
})
export class FileService {
	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/file`;

	public getFile(id: string) {
		return this.http.get<FileModel>(`${this.baseURI}/${id}`);
	}

	public getFiles(ignoreFileIds: string[]) {
		return this.http.get<FileModel[]>(this.baseURI, {
			params: {
				ignoreIdsString: ignoreFileIds.join(',')
			}
		});
	}

	public startFileUpload(data: StartFileUploadRequest) {
		return this.http.post<FileModel>(`${this.baseURI}/start`, data);
	}

	public uploadFile(file: File, uploadId: string) {
		const formData = new FormData();
		formData.append('file', file);

		const request = new HttpRequest('POST', `${this.baseURI}/${uploadId}`, formData, {
			reportProgress: true
		});
		return this.http.request(request);
	}

	public getAvailableConversions(mimeType: string) {
		return this.http.get<ConversionType[]>(`${this.baseURI}/conversions`, {
			params: {
				mimeType: encodeURIComponent(mimeType)
			}
		});
	}

	public convertFile(data: FileConversionRequest) {
		return this.http.post<FileConversion>(`${this.baseURI}/convert`, data);
	}

	public getFileToken(fileInfo: FileTokenRequest) {
		return this.http.post<FileTokenResponse>(`${this.baseURI}/url`, fileInfo);
	}
}
