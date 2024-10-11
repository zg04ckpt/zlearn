import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserManagement } from '../entities/management/user-management.entity';
import { PagingResultDTO } from '../dtos/common/paging-result.dto';
import { Role } from '../entities/management/role.entity';
import { AssignRoleDTO } from '../dtos/management/assign-role.dto';
import { APIResult } from '../dtos/common/api-result.dto';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {
  constructor(
    private http: HttpClient
  ) { }

  //USER --------------------------------------------------
  getAllUsers(pageIndex: number,pageSize: number, searchKeys: any): Observable<PagingResultDTO<UserManagement>> {
    let params = new HttpParams()
    .set('pageIndex', pageIndex)
    .set('pageSize', pageSize)
    .set('lastName', searchKeys.lastName)
    .set('userName', searchKeys.username)
    .set('email', searchKeys.email)
    .set('createdDate', searchKeys.createdDate);
    
    return this.http.get<APIResult<PagingResultDTO<UserManagement>>>(
      `managements/users`, { params }
    ).pipe(map(res => res.data!));
  }

  getUserById(id: string): Observable<UserManagement> {
    return this.http.get<APIResult<UserManagement>>(
      `managements/users/${id}`
    ).pipe(map(res => res.data!));
  }

  getAllUserRoles(id: string): Observable<string[]> {
    return this.http.get<APIResult<string[]>>(
      `managements/users/${id}/roles`
    ).pipe(map(res => res.data!));
  }

  updateUser(data: UserManagement): Observable<void> {
    return this.http.put<APIResult<void>>(
      `managements/users`,
      data
    )
    .pipe(map(res => res.data));;
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(
      `managements/users/${id}`
    );
  }

  assignRole(id: string, data: AssignRoleDTO): Observable<void> {
    return this.http.post<APIResult<void>>(
      `managements/users/${id}/assign-role`,
      data
    ).pipe(map(res => res.data!));
  }


  // ROLE -------------------------------------------------
  getAllRoles = (): Observable<APIResult<Role[]>> => this.http.get<APIResult<Role[]>>(`managements/roles`);

  createRole(name: string, desc: string): Observable<void> {
    return this.http.post<void>(`managements/roles`,{
      name: name,
      description: desc
    });
  }

  updateRole(data: Role): Observable<void> {
    return this.http.put<void>(`managements/roles`,data);
  }

  deleteRole(id: string): Observable<void> {
    return this.http.delete<void>(`managements/roles/${id}`);
  }
}
