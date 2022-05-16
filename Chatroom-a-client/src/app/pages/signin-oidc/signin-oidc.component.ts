import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signin-oidc',
  templateUrl: './signin-oidc.component.html',
  styleUrls: ['./signin-oidc.component.scss']
})
export class SigninOidcComponent implements OnInit {

  constructor(private authSvc: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.authSvc.completeLogin().then(user =>{
      this.router.navigate(['/chatroom'], { replaceUrl: true });
    });
   }
}
