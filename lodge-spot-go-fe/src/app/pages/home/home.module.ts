import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { HomePageComponent } from './home.page/home.page.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { SearchAccomodationsComponentComponent } from 'src/app/search-accomodations-component/search-accomodations-component.component';
import { MatIconModule } from '@angular/material/icon';
import { MatNativeDateModule } from '@angular/material/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {CoreModule} from "../../core/core.module";
import {
  AccommodationCardComponent
} from "../../search-accomodations-component/accommodation-card/accommodation-card.component";
import {
  FilterSearchAccommodationComponent
} from "../../search-accomodations-component/filter-search-accommodation/filter-search-accommodation.component";
import {MatSelectModule} from "@angular/material/select";




@NgModule({
  declarations: [
    HomePageComponent,
    SearchAccomodationsComponentComponent,
    FilterSearchAccommodationComponent,
    AccommodationCardComponent
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
    MatInputModule,
    ReactiveFormsModule,
    CoreModule,
    NgOptimizedImage,
    MatSelectModule
  ],
  exports:
    [SearchAccomodationsComponentComponent,
      AccommodationCardComponent,
    FilterSearchAccommodationComponent]
})
export class HomeModule { }
