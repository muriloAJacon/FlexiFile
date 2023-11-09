import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { DateTime } from 'luxon';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
	providedIn: 'root'
})
export class TokenService {
	private readonly SESSION_TOKEN = "session_token";
	private readonly REFRESH_TOKEN = "refresh_token";
	private readonly USER_INFO = "user_info";

	constructor(private cookieService: CookieService) { }

	signOut(): void {
		this.cookieService.delete(this.SESSION_TOKEN, '/');
		this.cookieService.delete(this.USER_INFO, '/');
	}

	saveJwtToken(token: string, expiration: DateTime): void {
		this.cookieService.delete(this.SESSION_TOKEN, '/');
		this.cookieService.set(this.SESSION_TOKEN, token, { path: '/', expires: expiration.toJSDate(), sameSite: "Strict", secure: true });
		this.saveUserInfo(expiration);
	}

	saveRefreshToken(token: string, expiration: DateTime) {
		this.cookieService.delete(this.REFRESH_TOKEN);
		this.cookieService.set(this.REFRESH_TOKEN, token, { path: '/', expires: expiration.toJSDate(), sameSite: "Strict", secure: true });
	}

	getJwtToken(): string | null {
		return this.cookieService.get(this.SESSION_TOKEN) == "" ? null : this.cookieService.get(this.SESSION_TOKEN);
	}

	saveUserInfo(expiration: DateTime): void {
		const token: string | null = this.cookieService.get(this.SESSION_TOKEN);
		if (token == null) {
			return;
		}

		const payload: any = jwtDecode(token);

		const userModel = {
			id: payload.nameid
		};

		this.cookieService.delete(this.USER_INFO, '/');
		this.cookieService.set(this.USER_INFO, JSON.stringify(userModel), { path: '/', expires: expiration.toJSDate(), sameSite: "Strict", secure: true });
	}
}
