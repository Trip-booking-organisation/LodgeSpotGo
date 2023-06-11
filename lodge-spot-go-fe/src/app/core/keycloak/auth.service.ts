import {Injectable, signal} from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import {User} from "./user";
import {Observable, BehaviorSubject} from "rxjs";
import jwtDecode from "jwt-decode";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user$: BehaviorSubject<User|null> = new BehaviorSubject(null);
  private token$: BehaviorSubject<string> =  new BehaviorSubject(null);
  private authenticated$: BehaviorSubject<boolean> =  new BehaviorSubject(null);
  private USER_KEY: string = 'user'

  private user: User|null = null;
  private token: string = '';
  private authenticated: boolean = false;

  constructor(private keycloak: KeycloakService,private router:Router) {
    this.loadFromStorage()
    this.keycloak.isLoggedIn().then((authenticated) => {
      this.authenticated = authenticated;
      if (authenticated) {
        this.keycloak.getToken().then((token) => {
          this.token = token;
          this.token$.next(this.token);
          let decoded: any = jwtDecode(token);
          this.user = {
            id : decoded.sub,
            email : decoded.email,
            name : decoded.name,
            surname : decoded.given_name,
            roles: decoded.roles
          }
          this.saveUserToStorage(this.user)
          this.user$.next(this.user);
        });
        this.authenticated$.next(this.authenticated);
      }
    });
  }
  loadFromStorage(){
    if(!this.user){
     this.user = JSON.parse(sessionStorage.getItem(this.USER_KEY))
    }
  }
  saveUserToStorage(user: User){
    sessionStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }
  removeUserFromStorage(){
    sessionStorage.removeItem(this.USER_KEY)
  }
  getUserObservable(): Observable<User|null> {
    console.log(this.user$)
    return this.user$;
  }
  getUserAsSignal(){
    return signal(this.getUser()).asReadonly()
  }
  getUser(): User | null {
    if(!this.user){
      this.loadFromStorage()
    }
    return this.user
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
    this.removeUserFromStorage()
    this.keycloak.logout().then(() => this.router.navigate(['']));
  }

  register() {
    this.keycloak.register()
  }
}
