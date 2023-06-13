import {APP_INITIALIZER, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {CoreModule} from "./core/core.module";
import {KeycloakAngularModule, KeycloakService} from "keycloak-angular";
import {HttpClientModule} from "@angular/common/http";
import {initializeKeycloak} from "./core/keycloak/init-keycloak";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {CommonModule} from "@angular/common";

import {AccommodationCreateModule} from "./pages/accommodation-create/accommodation-create.module";
import {MatInputModule} from "@angular/material/input";
import {HostsAccomodationsModule} from "./pages/hosts-accomodations/hosts-accomodations.module";
import {ToastrModule} from "ngx-toastr";
import { AccommodationCardComponent } from './search-accomodations-component/accommodation-card/accommodation-card.component';
import {HomeModule} from "./pages/home/home.module";
import {AllAccommodationsModule} from "./pages/all-accommodations/all-accommodations.module";
import {ViewAccommodationModule} from "./pages/view-accommodation/view-accommodation.module";
import {GradeAccommodationModule} from "./pages/grade-accommodation/grade-accommodation.module";

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        CoreModule,
        KeycloakAngularModule,
        HttpClientModule,
        BrowserAnimationsModule,
        CommonModule,
        MatInputModule,
        GradeAccommodationModule,
        HostsAccomodationsModule,
        MatIconModule,
        AccommodationCreateModule,
        ToastrModule.forRoot(),
        HomeModule,
        AllAccommodationsModule,
        ViewAccommodationModule
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
