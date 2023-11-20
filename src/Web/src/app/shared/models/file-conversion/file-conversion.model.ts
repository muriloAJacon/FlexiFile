import { ConversionStatus } from "../file/conversion-status.enum";

export interface FileConversion {
	id: string;
	status: ConversionStatus;
	percentageComplete: number;
	creationDate: string;
	fileResults: string[];
}
