import { Injectable } from '@angular/core';
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	constructor(private tokenService: TokenService, private router: Router) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<Object>> {
		let req = request;
		const token = this.tokenService.getJwtToken();
		if (token != null) {
			req = this.addTokenHeader(request, token);
		}

		return next.handle(req).pipe(catchError(error => {
			return throwError(() => error);
		}));
	}

	private addTokenHeader(request: HttpRequest<any>, token: string) {
		return request.clone({ headers: request.headers.set("x-access-token", `Bearer ${token}`).set('authorization', `Bearer ${token}`) });
	}
}
