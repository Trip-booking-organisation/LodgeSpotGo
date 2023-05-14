import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import {MatIconModule} from "@angular/material/icon";
import {MatTooltipModule} from "@angular/material/tooltip";
import { ReserveAccomodationCardComponent } from './components/reserve-accomodation-card/reserve-accomodation-card.component';
import {MatButtonModule} from "@angular/material/button";

@NgModule({
  declarations: [
    NavbarComponent,
    ReserveAccomodationCardComponent
  ],
    exports: [
        NavbarComponent,
        ReserveAccomodationCardComponent
    ],
  imports: [
    CommonModule,
    MatIconModule,
    MatTooltipModule,
    NgOptimizedImage,
    MatButtonModule
  ]
})
export class CoreModule { }
