import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetAllowAnonymousRegister } from '../models/settings/get-allow-anonymous-register.model';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/settings`;

	public getAllowAnonymousRegister() {
		return this.http.get<GetAllowAnonymousRegister>(`${this.baseURI}/allowAnonymousRegister`);
	}
}
