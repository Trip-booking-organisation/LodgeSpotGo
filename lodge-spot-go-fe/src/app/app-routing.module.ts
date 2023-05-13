import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {AppComponent} from "./app.component";
import {HomePageComponent} from "./pages/home/home.page/home.page.component";
import {AccommodationCreateComponent} from "./pages/accommodation-create/accommodation-create/accommodation-create.component"

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: 'create-accomodation', component: AccommodationCreateComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
