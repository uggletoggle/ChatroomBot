import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isLoggedIn = false;

  constructor(private authSvc: AuthService,
    private router: Router) {
    this.authSvc.loginChanged$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  ngOnInit() {
    this.authSvc.isLoggedIn().then(loggedIn =>{
      console.log(this.isLoggedIn);
      this.isLoggedIn = loggedIn;

      if(this.isLoggedIn){
        this.router.navigate(['chatroom']);
      }
    });
  }

  login(){
    this.authSvc.login();
  }
}
