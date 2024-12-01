import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserManagement } from '../entities/management/user-management.entity';
import { PagingResultDTO } from '../dtos/common/paging-result.dto';
import { Role } from '../entities/management/role.entity';
import { AssignRoleDTO } from '../dtos/management/assign-role.dto';
import { APIResult } from '../dtos/common/api-result.dto';
import { TestDetail } from '../entities/test/test-detail.entity';
import { environment } from '../../environments/environment';
import { Summary } from '../entities/management/summary.entity';
import { CategoryNode } from '../entities/management/category-node.entity';
import { LogDTO } from '../dtos/management/log.dto';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {
  baseUrl = environment.baseUrl;
  constructor(
    private http: HttpClient
  ) { }

  // Overview ----------------------------------------------
  getOverviewToday(): Observable<Summary> {
    return this.http
      .get<APIResult<Summary>>(`managements/overview/today`)
      .pipe(map(res => res.data!));
  }

  getOverviewByRange(start: string, end: string): Observable<Summary> {
    return this.http
      .get<APIResult<Summary>>(`managements/overview/range?start=${start}&end=${end}`)
      .pipe(map(res => res.data!));
  }

  // Logs
  listenSystemLog(connectionId: string): Observable<void> {
    return this.http.post<void>(`managements/overview/connectToLogHub`, {connectionId: connectionId} );
  }

  getLogsOfDate(date: string): Observable<LogDTO[]> {
    return this.http
      .get<APIResult<LogDTO[]>>(`managements/log?date=${date}`)
      .pipe(map(res => res.data!));
  }

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


  // TEST -------------------------------------------------
  getAllTests(pageIndex: number, pageSize: number, key: String): Observable<PagingResultDTO<TestDetail>> {
    return this.http.get<APIResult<PagingResultDTO<TestDetail>>>(
      `managements/tests?pageIndex=${pageIndex}&pageSize=${pageSize}&name=${key}`
    ).pipe(
      map(res => res.data!),
      map(res => {
          res.data.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
          return res;
      })
    );
  }

  // CATEGORY -----------------------------------------------------
  getCategoryTree(): Observable<CategoryNode> {
    return this.http.get<APIResult<CategoryNode>>(
      `managements/categories`
    ).pipe(map(res => res.data!));
  }

  createNewCate(parentId: string, name: string, slug: string, desc: string): Observable<string> {
    return this.http.post<APIResult<string>>(
      `managements/categories`, 
      {name: name, description: desc, parentId: parentId, slug: slug}
    ).pipe(map(res => res.data!));
  }

  updateCate(id: string, parentId: string, name: string, slug: string, desc: string): Observable<void> {
    return this.http.put<APIResult<void>>(
      `managements/categories/${id}`, 
      {name: name, description: desc, parentId: parentId, slug: slug}
    ).pipe(map(res => res.data!));
  }

  deleteCate(id: string): Observable<void> {
    return this.http.delete<APIResult<void>>(
      `managements/categories/${id}`,
    ).pipe(map(res => res.data!));
  }
}
