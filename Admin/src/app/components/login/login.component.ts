import { Component, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private authService: AuthService) { }

  userName: string = "";
  password: string = "";
  remember: boolean = false;

  @ViewChild("inputPass") passInput!: ElementRef;
  showPassword:boolean = false;
  showPass()
  {
    this.showPassword = !this.showPassword;
    this.passInput.nativeElement.type = this.showPassword ? "text" : "password";
  }

  login() {
    console.log(this.userName, this.password, this.remember);
    
    this.authService.login(
      this.userName, 
      this.password, 
      this.remember
    ).subscribe();
  }
}
