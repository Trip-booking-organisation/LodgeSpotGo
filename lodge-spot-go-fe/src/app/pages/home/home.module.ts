import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './home.page/home.page.component';
import { MatCommonModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { SearchAccomodationsComponentComponent } from 'src/app/search-accomodations-component/search-accomodations-component.component';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import {MatInputModule} from "@angular/material/input";
import {FormsModule} from "@angular/forms";
//import { LoadingAnimationComponent } from 'path/to/loading-animation.component';



@NgModule({
  declarations: [
    HomePageComponent,
    SearchAccomodationsComponentComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatOptionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    FormsModule
  ],
  exports:
  [SearchAccomodationsComponentComponent]
})
export class HomeModule { }
