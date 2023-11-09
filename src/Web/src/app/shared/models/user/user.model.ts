export interface User {
	id: string;
	name: string;
	email: string;
	accessLevel: string;
	approved: boolean;
	approvedAt: string | null;
	creationDate: string;
	lastUpdateDate: string;
	storageLimit: number | null;
}
