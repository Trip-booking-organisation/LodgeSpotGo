import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllAccommodationsPageComponent } from './all-accommodations.page/all-accommodations.page.component';
import {HomeModule} from "../home/home.module";



@NgModule({
  declarations: [
    AllAccommodationsPageComponent
  ],
    imports: [
        CommonModule,
        HomeModule
    ]
})
export class AllAccommodationsModule { }
