import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signout-oidc',
  templateUrl: './signout-oidc.component.html',
  styleUrls: ['./signout-oidc.component.scss']
})
export class SignoutOidcComponent implements OnInit {

  constructor(
    private authSvc: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.authSvc.logout().then(_ =>{
      // TODO: need redirection?
    });
  }
}
