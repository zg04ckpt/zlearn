import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserManagement } from '../entities/management/user-management.entity';
import { PagingResultDTO } from '../dtos/common/paging-result.dto';
import { Role } from '../entities/management/role.entity';
import { AssignRoleDTO } from '../dtos/management/assign-role.dto';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {
  constructor(
    private http: HttpClient
  ) { }

  //USER --------------------------------------------------
  getAllUsers(pageIndex: number,pageSize: number): Observable<PagingResultDTO<UserManagement>> {
    return this.http.get<PagingResultDTO<UserManagement>>(
      `managements/users?pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
  }

  getUsersByKey(url: string, key: string, pageIndex: number,pageSize: number): Observable<PagingResultDTO<UserManagement>> {
    return this.http.get<PagingResultDTO<UserManagement>>(
      `managements/users/${url}/${key}?pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
  }

  getUserById(id: string): Observable<UserManagement> {
    return this.http.get<UserManagement>(
      `managements/users/${id}`
    );
  }

  getAllUserRoles(id: string): Observable<string[]> {
    return this.http.get<string[]>(
      `managements/users/${id}/roles`
    );
  }

  updateUser(data: UserManagement): Observable<void> {
    return this.http.put<void>(
      `managements/users`,
      data
    );
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(
      `managements/users/${id}`
    );
  }

  assignRole(id: string, data: AssignRoleDTO): Observable<void> {
    return this.http.post<void>(
      `managements/users/${id}/assign-role`,
      data
    );
  }


  // ROLE -------------------------------------------------
  getAllRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`roles`);
  }

  createRole(name: string, desc: string): Observable<void> {
    return this.http.post<void>(`roles`,{
      name: name,
      description: desc
    });
  }

  updateRole(data: Role): Observable<void> {
    return this.http.put<void>(`roles`,data);
  }

  deleteRole(id: string): Observable<void> {
    return this.http.delete<void>(`roles/${id}`);
  }
}
