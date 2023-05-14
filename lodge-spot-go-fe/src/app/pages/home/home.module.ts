import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './home.page/home.page.component';
import { MatCommonModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { SearchAccomodationsComponentComponent } from 'src/app/search-accomodations-component/search-accomodations-component.component';
import { MatIconModule } from '@angular/material/icon';
import { MatNativeDateModule } from '@angular/material/core';
import {FormsModule} from "@angular/forms";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
//import { LoadingAnimationComponent } from 'path/to/loading-animation.component';



@NgModule({
  declarations: [
    HomePageComponent,
    SearchAccomodationsComponentComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatAutocompleteModule,
    MatOptionModule,
    MatNativeDateModule,
    FormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports:
  [SearchAccomodationsComponentComponent]
})
export class HomeModule { }
