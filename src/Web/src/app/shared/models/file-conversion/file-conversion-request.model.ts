export interface FileConversionRequest {
	fileIds: string[];
	conversionId: number;
	extraParameters: { [key: string]: any } | null;
}
