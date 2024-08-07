import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../layout/header/header.component';
import { FooterComponent } from '../layout/footer/footer.component';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';
import { LoginComponent } from "../components/login/login.component";
import { RegisterComponent } from "../components/register/register.component";
import { LayoutService } from '../services/layout.service';
import { CommonModule } from '@angular/common';
import { state, style, trigger } from '@angular/animations';
import { LoadingComponent } from '../components/loading/loading.component';
import { MessageComponent } from '../components/message/message.component';
import { ToastComponent } from '../components/toast/toast.component';
import { UserService } from '../services/user.service';
import { StorageKey, StorageService } from '../services/storage.service';
import { ComponentService } from '../services/component.service';
import { AuthService } from '../services/auth.service';
import { ApiNotResponseComponent } from '../pages/errors/api-not-response/api-not-response.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    LoginComponent,
    RegisterComponent,
    LoadingComponent,
    MessageComponent,
    ToastComponent,
    ApiNotResponseComponent,
    CommonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  showSidebar: boolean = true;

  constructor(
    private layoutService: LayoutService,
    private userService: UserService,
    private authService: AuthService
  ) {
    this.layoutService.$showSidebar.subscribe(value => this.showSidebar = value);

    
  }
  
  ngOnInit(): void {
    this.userService.$currentUser.next(this.userService.getLoggedInUser());

    //show end session message when refresh token expired
    this.authService.setLoginSessionTimer();
  }
  

}
