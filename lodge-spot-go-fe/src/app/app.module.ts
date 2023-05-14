import {APP_INITIALIZER, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {CoreModule} from "./core/core.module";
import {KeycloakAngularModule, KeycloakService} from "keycloak-angular";
import {HttpClientModule} from "@angular/common/http";
import {initializeKeycloak} from "./core/components/keycloak/init-keycloak";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {CommonModule} from "@angular/common";
import {HomeModule} from "./pages/home/home.module";
//import { SearchAccomodationsComponentComponent } from './search-accomodations-component/search-accomodations-component.component';

@NgModule({
  declarations: [
    AppComponent
    //SearchAccomodationsComponentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    KeycloakAngularModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CommonModule,
    HomeModule,
    MatIconModule
  ],
  providers: [{
    provide: APP_INITIALIZER,
    useFactory: initializeKeycloak,
    multi: true,
    deps: [KeycloakService]
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
