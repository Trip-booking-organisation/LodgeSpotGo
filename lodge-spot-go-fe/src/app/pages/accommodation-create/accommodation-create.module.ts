import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { AccommodationCreateComponent } from './accommodation-create/accommodation-create.component';
import { ReactiveFormsModule } from '@angular/forms';
import {MatInputModule} from "@angular/material/input";
import {MatCheckboxModule} from "@angular/material/checkbox";
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatLegacyButtonModule} from "@angular/material/legacy-button";





@NgModule({
  declarations: [
    AccommodationCreateComponent
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatLegacyButtonModule,
    NgOptimizedImage
  ]
})
export class AccommodationCreateModule { }
