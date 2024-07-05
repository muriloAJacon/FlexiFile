import { FileType } from "./file-type.enum";

export interface FileTokenRequest {
	fileId: string;
	fileType: FileType;
}
