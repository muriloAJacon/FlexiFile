export interface ConversionType {
	id: number;
	toTypeDescription: string | null;
	description: string;
	maxNumberFiles: number | null;
	minNumberFiles: number | null;
	modelClassName: string | null;
}
