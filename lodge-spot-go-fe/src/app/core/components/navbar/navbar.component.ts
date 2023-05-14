import {Component, HostListener, OnInit} from '@angular/core';
import {noRegisterUserNavData} from "./data/no-register-user-nav-data";
import {hostNavData} from "./data/host-nav-data";
import {guestNavData} from "./data/guest-nav-data";
import {Router} from "@angular/router";
import {AuthService} from "../../keycloak/auth.service";
import {User} from "../../keycloak/user";
import {Observable} from "rxjs";
import {environment} from "../../../../environments/environment.development";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  navDataNoRegister = noRegisterUserNavData;
  navDataGuest = guestNavData;
  navDataHost = hostNavData;
  activeClass: string = 'navbar-menu';
  addBackground: string = 'navbar-two';
  isCollapsed: boolean = false
  user$: Observable<User | null> = this.authService.getUserObservable();

  constructor(private router: Router, private authService:AuthService) {
  }
  ngOnInit(): void {
    this.authService.getUserObservable().subscribe(
      value => {
        console.log(value)
      }
    )
    this.authService.getTokenObservable().subscribe(token => {
      console.log("value")
      console.log(token)
    })
    //this.goToHomePage();
  }
  login(): void {
    this.authService.login();
  }
  register(): void {
    this.authService.register();
  }

  logout(): void {
    this.authService.logout();
  }

  goToHomePage(): void {
    this.router.navigate(['']);
  }
  goToAccountPage(): void {
    window.open(environment.keycloak.accountUrl, '_blank');
  }
  navigateHome() {
    this.router.navigate(['']).then();
  }

  showNavBar() {
    this.activeClass = this.isCollapsed ? 'navbar-menu' : 'navbar-menu show-nav';
    this.isCollapsed = !this.isCollapsed
  }
  @HostListener('window:scroll', ['$event'])
  addBg() {
    this.addBackground = window.scrollY >= 200
      ? 'navbar-two nav-bg'
      : 'navbar-two';
  }
  navigate(routerLink: string) {
    this.activeClass = 'navbar-menu'
    this.router.navigate([routerLink]).then()
  }
}
