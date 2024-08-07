import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDetailMapper } from '../mappers/user-detail.mapper';
import { UserDetail } from '../entities/user-detail.entity';
import { map, Observable, Subject } from 'rxjs';
import { UserDetailDTO } from '../dtos/user-detail.dto';
import { StorageKey, StorageService } from './storage.service';
import { User } from '../entities/user.entity';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public $currentUser = new Subject<User|null>();
  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) { }

  getLoggedInUser(): User|null {
    const data = this.storageService.get(StorageKey.user);
    if(data != null)
      return JSON.parse(data) as User;
    else 
      return null;
  }

  getProfile(userId: string): Observable<UserDetail> {
    const userDetailMapper = new UserDetailMapper;
    return this.http
      .get<UserDetailDTO>(`users/${userId}/profile`)
      .pipe(map(userDetailMapper.map));
  }

  updateProfile(userId: string, data: UserDetailDTO): Observable<void> {
    return this.http.put<void>(`users/${userId}/profile`, data);
  }
}
