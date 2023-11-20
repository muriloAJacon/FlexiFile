import { FileConversion } from "../file-conversion/file-conversion.model";

export interface FileModel {
	id: string;
	typeDescription: string;
	size: number;
	originalName: string;
	submittedAt: string;
	finishedUpload: boolean;
	finishedUploadAt: string;
	mimeType: string;
	conversions: FileConversion[];
}
