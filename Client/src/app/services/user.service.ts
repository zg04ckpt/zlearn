import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDetailMapper } from '../mappers/user/user-detail.mapper';
import { UserDetail } from '../entities/user/user-detail.entity';
import { lastValueFrom, map, Observable, pipe, Subject } from 'rxjs';
import { UserDetailDTO } from '../dtos/user/user-detail.dto';
import { StorageKey, StorageService } from './storage.service';
import { User } from '../entities/user/user.entity';
import { APIResult } from '../dtos/common/api-result.dto';
import { FileService } from './file.service';
import { FileResponseDTO } from '../dtos/common/file.dto';
import { ComponentService } from './component.service';
import { environment } from '../../environments/environment';
import { UserMapper } from '../mappers/user/user.mapper';
import { UserInfoDTO } from '../dtos/user/user-info.dto';
import { UserInfoMapper } from '../mappers/user/user-info.mapper';
import { UserInfo } from '../entities/user/user-info.entity';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public $currentUser = new Subject<User|null>();
  public $showInfo = new Subject<string>();

  baseUrl: string = environment.baseUrl;
  constructor(
    private http: HttpClient,
    private storageService: StorageService,
    private fileService: FileService,
    private componentService: ComponentService
  ) { }

  getLoggedInUser(): User|null {
    const data = this.storageService.get(StorageKey.user);
    if(data != null)
      return JSON.parse(data) as User;
    else 
      return null;
  }

  getProfile(): Observable<UserDetail> {
    const userDetailMapper = new UserDetailMapper;
    return this.http
      .get<APIResult<UserDetailDTO>>(`users/my-profile`)
      .pipe(map(res => res.data!))
      .pipe(map(userDetailMapper.map));
  }

  async updateProfile(data: UserDetailDTO): Promise<void> {
    debugger
    if(data.image) {
      if(data.imageUrl) {
        data.imageUrl = await this.fileService.updateImage(data.imageUrl, data.image);
      } else {
        data.imageUrl = await this.fileService.saveImage(data.image);
      }
    }

    return lastValueFrom(this.http
      .put<APIResult<void>>(`users/my-profile`, data)
      .pipe(map(res => res.data!)));
  }

  getUserProfile(userId: string): Observable<UserInfo> {
    const mapper = new UserInfoMapper;
    return this.http
      .get<APIResult<UserInfoDTO>>(`users/${userId}`)
      .pipe(map(res => res!.data))
      .pipe(map(res => mapper.map(res!)));
  }

  like(userId: string): Observable<void> {
    return this.http
      .get<APIResult<void>>(`users/like?userId=${userId}`)
      .pipe(map(res => res!.data));
  }
}
