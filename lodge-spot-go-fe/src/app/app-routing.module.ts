import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomePageComponent} from "./pages/home/home.page/home.page.component";
import {AccommodationCreateComponent} from "./pages/accommodation-create/accommodation-create/accommodation-create.component"
import {ReservationComponent} from "./pages/reservation/reservation-guest/reservation.component";
import {ReservationModule} from "./pages/reservation/reservation.module";

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: 'create-accomodation', component: AccommodationCreateComponent},
  {path: 'reservations', component: ReservationComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ReservationModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
