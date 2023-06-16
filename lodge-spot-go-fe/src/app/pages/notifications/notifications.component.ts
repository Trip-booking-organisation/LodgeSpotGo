import {Component, inject, OnDestroy, OnInit, Signal} from '@angular/core';
import {HubConnectionStatus, NotificationsService} from "./data-access/notifications.service";
import {AuthService} from "../../core/keycloak/auth.service";
import {User} from "../../core/keycloak/user";
import {filter, Observable, take} from "rxjs";
import {NotificationResponse} from "../../shered/model/NotificationResponse";

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit,OnDestroy {
  notifications$: Observable<NotificationResponse[]>;
  private notificationService: NotificationsService = inject(NotificationsService)
  private authService: AuthService = inject(AuthService)
  private userSignal: Signal<User>
  constructor() { }

  ngOnInit() {
    this.userSignal = this.authService.getUserAsSignal()
    this.notifications$ = this.notificationService.getNotificationsObservable()
    this.notificationService.startConnection();
    this.notificationService.connectionState$.pipe(
      filter((state) => state === HubConnectionStatus.CONNECTED),
      take(1)
    ).subscribe(() => {
      if(this.userSignal()){
        this.notificationService.sendMessage(this.userSignal().id)
      }
    })
  }

  ngOnDestroy() {
    this.notificationService.stopConnection();
  }
}
