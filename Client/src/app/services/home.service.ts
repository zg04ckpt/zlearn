import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { TestItem } from '../entities/test/test-item.entity';
import { APIResult } from '../dtos/common/api-result.dto';
import { environment } from '../../environments/environment';
import { UserInfo } from '../entities/user/user-info.entity';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(
    private http: HttpClient
  ) { }

  public getRandomTest(amount: number): Observable<TestItem[]> {
    return this.http
      .get<APIResult<TestItem[]>>(`home/random-tests?amount=${amount}`)
      .pipe(map(res => {
        let data = res.data!;
        data.forEach(e => {
          if(e.imageUrl) {
            e.imageUrl = environment.baseUrl + e.imageUrl
          }
        });
        return data;
      }));
  }

  public getTopTest(amount: number): Observable<TestItem[]> {
    return this.http
      .get<APIResult<TestItem[]>>(`home/top-tests?amount=${amount}`)
      .pipe(map(res => {
        let data = res.data!;
        data.forEach(e => {
          if(e.imageUrl) {
            e.imageUrl = environment.baseUrl + e.imageUrl
          }
        });
        return data;
      }));
  }

  public getTopUsers(amount: number): Observable<UserInfo[]> {
    return this.http
      .get<APIResult<UserInfo[]>>(`home/top-users?amount=${amount}`)
      .pipe(map(res => {
        let data = res.data!;
        data.forEach(e => {
          if(e.imageUrl) {
            e.imageUrl = environment.baseUrl + e.imageUrl
          }
        });
        return data;
      }));
  }
}
