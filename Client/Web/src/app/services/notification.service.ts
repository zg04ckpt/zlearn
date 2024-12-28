import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { APIResult } from '../dtos/common/api-result.dto';
import { Notification } from '../entities/notification/notification.dto';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  baseUrl = environment.baseUrl;
  constructor(
    private http: HttpClient
  ) { }

  getNotifications(start: number): Observable<Notification[]> {
    return this.http
      .get<APIResult<Notification[]>>(`notifications?start=${start}`)
      .pipe(map(res => res.data!))
      .pipe(map(res => {
        res.forEach(e => e.createdAt = new Date(e.createdAt))
        return res;
      }));
  }
  
  listenForUser(connectionId: string): Observable<void> {
    return this.http
      .post<APIResult<void>>(`notifications`, {connectionId: connectionId})
      .pipe(map(res => res.data!));
  }
}
