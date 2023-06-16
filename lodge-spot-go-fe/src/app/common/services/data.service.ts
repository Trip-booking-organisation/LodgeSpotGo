import { Injectable } from '@angular/core';
import {BehaviorSubject, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private data = new BehaviorSubject<string>(null);
  sendData(data: any) {
    this.data.next(data);
  }
  getData() {
    return this.data.asObservable();
  }
  constructor() { }
}
