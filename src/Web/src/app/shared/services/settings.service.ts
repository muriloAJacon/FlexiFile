import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetAllowAnonymousRegister } from '../models/settings/get-allow-anonymous-register.model';
import { GetGlobalMaxFileSize } from '../models/settings/get-global-max-file-size.model';
import { ChangeGlobalMaxFileSize } from '../models/settings/change-global-max-file-size.model';
import { ChangeAllowAnonymousRegister } from '../models/settings/change-allow-anonymous-register.model';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

	constructor(private http: HttpClient) { }

	private baseURI = `${environment.baseURI}/v1/settings`;

	public getAllowAnonymousRegister() {
		return this.http.get<GetAllowAnonymousRegister>(`${this.baseURI}/allowAnonymousRegister`);
	}

	public changeAllowAnonymousRegister(data: ChangeAllowAnonymousRegister) {
		return this.http.put(`${this.baseURI}/allowAnonymousRegister`, data);
	}

	public getGlobalMaxFileSize() {
		return this.http.get<GetGlobalMaxFileSize>(`${this.baseURI}/globalMaximumFileSize`);
	}

	public changeGlobalMaxFileSize(data: ChangeGlobalMaxFileSize) {
		return this.http.put(`${this.baseURI}/globalMaximumFileSize`, data);
	}
}
