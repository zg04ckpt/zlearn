import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private httpClient: HttpClient) { }
  baseUrl: string = environment.baseUrl;

  login(userName: string, password: string, remember: boolean) {

    const formData = new FormData();
    formData.append('userName', userName);
    formData.append('password', password);
    formData.append('remember', remember.toString());

    return this.httpClient.post<any>(
      `${this.baseUrl}/api/users/login`, 
      formData
    ).pipe(tap(res => {
      if(res.code == 200)
      {
        sessionStorage.setItem('token', res.data.token);
        window.location.href= "/";
      }
    }, error => {
      alert(`Error: code = ${error.status}, message = ${error.error.message}`);
    }));
  }

  logout() {
    sessionStorage.removeItem('token');
  }

  get token(): string | null {
    return sessionStorage.getItem('token');
  }
}
