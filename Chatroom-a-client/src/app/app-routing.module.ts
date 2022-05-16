import { AuthGuard } from './auth.guard';
import { ChatroomComponent } from './pages/chatroom/chatroom.component';
import { SignoutOidcComponent } from './pages/signout-oidc/signout-oidc.component';
import { SigninOidcComponent } from './pages/signin-oidc/signin-oidc.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'signin-oidc', component: SigninOidcComponent },
  { path: 'signout-oidc', component: SignoutOidcComponent },
  { path: 'chatroom', component: ChatroomComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
