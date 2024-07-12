export interface EditUserRequest {
	id: string;
	name: string;
	password: string | null;
	storageLimit: number | null;
	hardStorageLimit: number | null;
}
