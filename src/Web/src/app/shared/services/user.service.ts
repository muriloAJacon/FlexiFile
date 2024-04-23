import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetFirstSetupResponse } from '../models/user/get-first-setup-response.model';
import { CreateFirstSetupRequest } from '../models/user/create-first-setup-request.model';
import { CreateUserRequest } from '../models/user/create-user-request.model';
import { User } from '../models/user/user.model';
import { ApproveUserRequest } from '../models/user/approve-user-request.model';

@Injectable({
	providedIn: 'root'
})
export class UserService {

	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/user`;

	public getUsers() {
		return this.http.get<User[]>(`${this.baseURI}`);
	}

	public getFirstSetup() {
		return this.http.get<GetFirstSetupResponse>(`${this.baseURI}/firstSetup`);
	}

	public registerFirstSetup(data: CreateFirstSetupRequest) {
		return this.http.post(`${this.baseURI}/firstSetup`, data);
	}

	public createUser(data: CreateUserRequest) {
		return this.http.post<User>(`${this.baseURI}`, data);
	}

	public approveUser(data: ApproveUserRequest) {
		return this.http.put<User>(`${this.baseURI}/approve`, data);
	}
}
