import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { DateTime } from 'luxon';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from './auth.service';
import { Router, RouterStateSnapshot } from '@angular/router';
import { RefreshTokenRequest } from '../models/auth/refresh-token-request.model';
import { LoginResponse } from '../models/auth/login-response.model';

@Injectable({
	providedIn: 'root'
})
export class TokenService {
	private readonly SESSION_TOKEN = "session_token";
	private readonly REFRESH_TOKEN = "refresh_token";
	private readonly USER_INFO = "user_info";

	constructor(private cookieService: CookieService, private authService: AuthService) { }

	signOut(): void {
		this.cookieService.delete(this.SESSION_TOKEN, '/');
		this.cookieService.delete(this.USER_INFO, '/');
	}

	saveLoginResponseData(loginResponse: LoginResponse) {
		this.saveJwtToken(loginResponse.token, DateTime.fromISO(loginResponse.expiresAt));
		this.saveRefreshToken(loginResponse.refreshToken, DateTime.fromISO(loginResponse.refreshTokenExpiresAt));
	}

	private saveJwtToken(token: string, expiration: DateTime): void {
		this.cookieService.delete(this.SESSION_TOKEN, '/');
		this.cookieService.set(this.SESSION_TOKEN, token, { path: '/', expires: expiration.toJSDate(), sameSite: "Strict", secure: true });
		this.saveUserInfo(expiration);
	}

	private saveRefreshToken(token: string, expiration: DateTime) {
		this.cookieService.delete(this.REFRESH_TOKEN);
		this.cookieService.set(this.REFRESH_TOKEN, token, { path: '/', expires: expiration.toJSDate(), sameSite: "Strict", secure: true });
	}

	getJwtToken(): string | null {
		return this.cookieService.get(this.SESSION_TOKEN) == "" ? null : this.cookieService.get(this.SESSION_TOKEN);
	}

	getRefreshToken(): string | null {
		return this.cookieService.get(this.REFRESH_TOKEN) == "" ? null : this.cookieService.get(this.REFRESH_TOKEN);
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

	canActiveLoggedInRoute(router: Router, state: RouterStateSnapshot): Promise<boolean> | boolean {
		if (this.getJwtToken() == null) {
			const refreshToken = this.getRefreshToken();

			if (refreshToken == null) {
				router.navigate(["/login"], { queryParams: { "redirect": state.url } });
				return false;
			}

			return new Promise((resolve, reject) => {
				const refreshTokenData: RefreshTokenRequest = {
					refreshToken: refreshToken
				}
				this.authService.refreshToken(refreshTokenData).subscribe({
					next: (response) => {
						this.saveLoginResponseData(response);

						resolve(true);
					},
					error: () => {
						router.navigate(["/login"], { queryParams: { "redirect": state.url } });
						reject(false);
					}
				});
			});
		}

		return true;
	}
}
