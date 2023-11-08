import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetFirstSetupResponse } from '../models/user/get-first-setup-response.model';
import { CreateFirstSetupRequest } from '../models/user/create-first-setup-request.model';

@Injectable({
	providedIn: 'root'
})
export class UserService {

	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/user`;

	public getFirstSetup() {
		return this.http.get<GetFirstSetupResponse>(`${this.baseURI}/firstSetup`);
	}

	public registerFirstSetup(data: CreateFirstSetupRequest) {
		return this.http.post(`${this.baseURI}/firstSetup`, data);
	}
}
