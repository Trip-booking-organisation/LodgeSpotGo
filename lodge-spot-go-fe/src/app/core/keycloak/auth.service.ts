import { Injectable } from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import {User} from "./user";
import {Observable, Subject} from "rxjs";
import jwtDecode from "jwt-decode";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user$: Subject<User|null> = new Subject();
  private token$: Subject<string> =  new Subject();
  private authenticated$: Subject<boolean> =  new Subject();

  private user: User|null = null;
  private token: string = '';
  private authenticated: boolean = false;

  constructor(private keycloak: KeycloakService,private router:Router) {
    this.keycloak.isLoggedIn().then((authenticated) => {
      this.authenticated = authenticated;
      if (authenticated) {
        this.keycloak.getToken().then((token) => {
          this.token = token;
          this.token$.next(this.token);
          let decoded: any = jwtDecode(token);
          console.log(decoded)
          this.user = {
            id : decoded.sub,
            email : decoded.email,
            name : decoded.name,
            surname : decoded.given_name,
            roles: decoded.roles
          }
          this.user$.next(this.user);
        });

        this.authenticated$.next(this.authenticated);
      }
    });
  }
  getUserObservable(): Observable<User|null> {
    return this.user$;
  }
  getToken(): string {
    return this.token;
  }
  getTokenObservable(): Observable<string> {
    return this.token$;
  }
  isAuthenticated(): boolean {
    return this.authenticated;
  }
  isAuthenticatedObservable(): Observable<boolean>{
    return this.authenticated$;
  }

  login() {
    this.keycloak.login();
  }
  logout() {
    this.keycloak.logout().then(() => this.router.navigate(['']));
  }

  register() {
    this.keycloak.register()
  }
}
