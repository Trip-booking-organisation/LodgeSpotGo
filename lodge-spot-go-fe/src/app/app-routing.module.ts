import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomePageComponent} from "./pages/home/home.page/home.page.component";
import {AccommodationCreateComponent} from "./pages/accommodation-create/accommodation-create/accommodation-create.component"
import {ReservationComponent} from "./pages/reservation/reservation-guest/reservation.component";
import {ReservationModule} from "./pages/reservation/reservation.module";
import {ReservationHostComponent} from "./pages/reservation/reservation-host/reservation-host.component";
import {
  HostsAccomodationsComponent
} from "./pages/hosts-accomodations/hosts-accomodations/hosts-accomodations.component";
import {
  AllAccommodationsPageComponent
} from "./pages/all-accommodations/all-accommodations.page/all-accommodations.page.component";
import {ViewAccommodationComponent} from "./pages/view-accommodation/view-accommodation/view-accommodation.component";
import {GuestGradesViewComponent} from "./pages/guest-grades/guest-grades-view/guest-grades-view.component";
import {ViewHostComponent} from "./pages/view-host/view-host/view-host.component";
import {ViewFlightsComponent} from "./pages/flights/view-flights/view-flights.component";

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: 'create-accommodation', component: AccommodationCreateComponent},
  {path: 'reservations', component: ReservationComponent},
  {path: 'reservations-host', component: ReservationHostComponent},
  {path:'hosts-accommodations', component:HostsAccomodationsComponent},
  {path:'all-accommodations', component:AllAccommodationsPageComponent},
  {path:'accommodation-view', component:ViewAccommodationComponent},
  {path:'all-grades', component:GuestGradesViewComponent},
  {path:'view-host', component:ViewHostComponent},
  {path:'flights', component:ViewFlightsComponent},
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ReservationModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
