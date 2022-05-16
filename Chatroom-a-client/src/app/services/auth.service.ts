import { Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _userManager: UserManager;
  private _user: User | null | undefined;
  private _loginChangedSubject = new Subject<boolean>();

  loginChanged$ = this._loginChangedSubject.asObservable();

  constructor() {
    const stsSettings = {
      authority: "https://localhost:44349",
      client_id: "angular",
      redirect_uri: "https://localhost:4200/signin-oidc",
      scope: 'openid profile Chatroom.API',
      response_type: 'code',
      post_logout_redirect_uri: "https://localhost:4200/signout-oidc",
    };

    this._userManager = new UserManager(stsSettings);
  }

  login() {
    return this._userManager.signinRedirect();
  }

  completeLogin(): Promise<User> {
    return this._userManager.signinRedirectCallback()
      .then(user => {
        this._user = user;
        this._loginChangedSubject.next(!!user && !user.expired);
        return user;
      });
  }

  isLoggedIn(): Promise<boolean> {
    return this._userManager.getUser()
      .then(user =>{
        const userCurrent = !!user && !user.expired;
        if(this._user !== user){
          this._loginChangedSubject.next(userCurrent);
        }
        this._user = user as User;
        return userCurrent;
      });
  }

  getAccessToken() {
    for (let i in sessionStorage) {
        if (i.includes('oidc')){
            let val: string = sessionStorage.getItem(i) ?? '';
            return JSON.parse(val).access_token;
        }
    }
  }

  async logout() {
    this._user = null;
    this._userManager.signoutRedirect();
  }

  logUser(){
    this._userManager.getUser().then(user =>{
      console.log(user);
    });
  }
}
