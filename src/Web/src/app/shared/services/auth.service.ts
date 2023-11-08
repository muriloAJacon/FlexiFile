import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../models/auth/login-request.model';
import { LoginResponse } from '../models/auth/login-response.model';

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/auth`;

	public login(data: LoginRequest) {
		return this.http.post<LoginResponse>(`${this.baseURI}/login`, data);
	}
}
