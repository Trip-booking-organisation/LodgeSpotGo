import {inject, Injectable} from '@angular/core';
import {HttpTransportType, HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {ToastrService} from "ngx-toastr";
import {NotificationResponse} from "../../../shered/model/NotificationResponse";
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
  private http: HttpClient = inject(HttpClient)
  private toast: ToastrService = inject(ToastrService)
  private notificationSubject: Subject<NotificationResponse[]> = new Subject<NotificationResponse[]>();
  private notificationUrl: string = "http://localhost:5259/notifications"
  private notificationUrlHttp: string = "https://localhost:7283"
  connectionState$: BehaviorSubject<HubConnectionStatus> = new BehaviorSubject(null);
  constructor() { }
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
    this.hubConnection.on('ReceiveNotification', (message: NotificationResponse[]) => {
      console.log(message)
      this.notificationSubject.next(message);
    });
  }
  getNotificationsObservable(){
    return this.notificationSubject.asObservable()
  }
  sendMessage(userId: string){
    this.hubConnection.invoke("SendNotificationsRequest",userId)
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
