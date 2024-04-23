import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

	constructor(
		private tokenService: TokenService,
		private router: Router
	) {

	}

	logout() {
		this.tokenService.signOut();
		this.router.navigate(['/login']);
	}
}
