import {inject, Injectable, Signal} from '@angular/core';
import {HttpTransportType, HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {ToastrService} from "ngx-toastr";
import {NotificationResponse} from "../../../shered/model/NotificationResponse";
import {User} from "../../../core/keycloak/user";
import {AuthService} from "../../../core/keycloak/auth.service";
import {hub} from "../../../shered/constants/hub";
export enum HubConnectionStatus {
  ESTABLISHING_CONNECTION,
  CONNECTED,
  STOPPED
}
@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private hubConnection: HubConnection;
  private toast: ToastrService = inject(ToastrService)
  private auth: AuthService = inject(AuthService)
  private notificationSubject: Subject<NotificationResponse[]> = new Subject<NotificationResponse[]>();
  private notificationUrl: string = "http://localhost:5259/notifications"
  private notificationUrlHttp: string = "https://localhost:7283"
  connectionState$: BehaviorSubject<HubConnectionStatus> = new BehaviorSubject(null);
  user: Signal<User>;
  constructor() {
    this.user = this.auth.getUserAsSignal()
  }
  // getByHost(hostId: string) : Observable<any> {
  //   return this.http.get(`${this.notificationUrlHttp}/api/v1/host-notifications/${hostId}`)
  // }
  startConnection() {
    this.connectionState$.next(HubConnectionStatus.ESTABLISHING_CONNECTION)
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.notificationUrl}`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection.start()
      .then(() => {
        this.connectionState$.next(HubConnectionStatus.CONNECTED);
        console.log('Connection started');
      })
      .catch(err => {
        this.toast.error("Connection error")
        console.error('Error starting connection: ', err);
      });
    this.hubConnection.on(this.determinateHub(), (message: NotificationResponse[]) => {
      console.log(message)
      this.notificationSubject.next(message);
    });
  }
  getNotificationsObservable(){
    return this.notificationSubject.asObservable()
  }
  sendMessage(){
    if(this.user().roles.includes('host')){
       this.sendMessageHost(this.user().id)
    }
    if(this.user().roles.includes('guest')){
      this.sendMessageGuest(this.user().id)
    }
  }
  private sendMessageHost(userId: string){
    this.hubConnection.invoke(hub.sendNotificationHostMethod,userId)
      .catch(err => {
        this.toast.error("Connection failed")
        console.log(err)
      })
  }
  private determinateHub(){
    if(this.user().roles.includes('host')){
      return `${hub.sendNotificationHostHub}/${this.user().id}`
    }
    if(this.user().roles.includes('guest')){
      return `${hub.sendNotificationGuestHub}/${this.user().id}`
    }
    return `${hub.sendNotificationDefaultHub}/${this.user().id}`
  }
  private sendMessageGuest(userId: string){
    this.hubConnection.invoke(hub.sendNotificationGuestMethod,userId)
      .catch(err => {
        this.toast.error("Connection failed")
        console.log(err)
      })
  }
  stopConnection() {
    this.hubConnection.stop()
      .then(() => {
        this.connectionState$.next(HubConnectionStatus.STOPPED)
        console.log('Connection stopped');
      })
      .catch(err => {
        console.error('Error stopping connection: ', err);
      });
  }
}
