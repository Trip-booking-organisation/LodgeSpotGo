import { Injectable } from '@angular/core';
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private data = new Subject<string>();
  sendData(data: any) {
    this.data.next(data);
  }
  getData() {
    return this.data.asObservable();
  }
  constructor() { }
}
