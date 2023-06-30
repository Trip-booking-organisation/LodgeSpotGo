import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { NotificationsComponent } from './notifications.component';
import {NotificationCardComponent} from "./notification-card/notification-card.component";



@NgModule({
  declarations: [
    NotificationsComponent,
    NotificationCardComponent
  ],
  imports: [
    CommonModule,
    NgOptimizedImage
  ]
})
export class NotificationsModule { }
