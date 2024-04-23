import { ConversionStatus } from "../file/conversion-status.enum";
import { FileConversionResult } from "./file-conversion-result.model";

export interface FileConversion {
	id: string;
	status: ConversionStatus;
	percentageComplete: number;
	creationDate: string;
	fileResults: FileConversionResult[];
}
