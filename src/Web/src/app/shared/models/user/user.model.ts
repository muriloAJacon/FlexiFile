import { AccessLevel } from "./access-level.enum";

export interface User {
	id: string;
	name: string;
	email: string;
	accessLevel: AccessLevel;
	approved: boolean;
	approvedAt: string | null;
	creationDate: string;
	lastUpdateDate: string;
	storageLimit: number | null;
	hardStorageLimit: number | null;
}
