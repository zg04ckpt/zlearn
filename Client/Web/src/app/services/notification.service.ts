import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  baseUrl = environment.baseUrl;
  constructor(
    private http: HttpClient
  ) { }

  
}
