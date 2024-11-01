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

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public $currentUser = new Subject<User|null>();
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

  getProfile(userId: string): Observable<UserDetail> {
    const userDetailMapper = new UserDetailMapper;
    return this.http
      .get<APIResult<UserDetailDTO>>(`users/${userId}/profile`)
      .pipe(map(res => res.data!))
      .pipe(map(userDetailMapper.map));
  }

  async updateProfile(userId: string, data: UserDetailDTO): Promise<void> {
    debugger
    if(data.image) {
      const formData = new FormData();
      let result: APIResult<FileResponseDTO[]>|null = null;
      if(data.imageUrl) {
        formData.append(data.imageUrl, data.image);
        result = await this.fileService.updateImage(formData);
      } else {
        formData.append('image', data.image);
        result = await this.fileService.saveFile(formData);
      }
      data.image = null;
      if(result.success) {
        data.imageUrl = result.data![0].url;
      } else {
        this.componentService.displayMessage("Thay đổi ảnh thất bại!");
      }
    }

    return lastValueFrom(this.http
      .put<APIResult<void>>(`users/${userId}/profile`, data)
      .pipe(map(res => res.data!)));
  }
}
