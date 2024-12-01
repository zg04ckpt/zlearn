import { AfterViewInit, Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../layout/header/header.component';
import { FooterComponent } from '../layout/footer/footer.component';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';
import { LayoutService } from '../services/layout.service';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../components/loading/loading.component';
import { MessageComponent } from '../components/message/message.component';
import { ToastComponent } from '../components/toast/toast.component';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';
import { Forbidden403Component } from '../pages/others/forbidden403/forbidden403.component';
import { ServiceUnavailable503Component } from '../pages/others/service-unavailable503/service-unavailable503.component';
import { UserInfoComponent } from '../components/user-info/user-info.component';
import { Register2Component } from "../components/register-2/register-2.component";
import { Login2Component } from "../components/login-2/login-2.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    LoadingComponent,
    MessageComponent,
    ToastComponent,
    UserInfoComponent,
    CommonModule,
    Forbidden403Component,
    ServiceUnavailable503Component,
    Register2Component,
    Login2Component
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
    this.layoutService.$showSidebar.subscribe(value => {
      if(window.innerWidth >= 800)
        this.showSidebar = value;
    });
  }
  
  ngOnInit(): void {
    this.userService.$currentUser.next(this.userService.getLoggedInUser());

    if(window.innerWidth < 800)
    {
      this.layoutService.$showSidebar.next(false);
      this.showSidebar = false;
    }

    //show end session message when refresh token expired
    this.authService.setLoginSessionTimer();
    
  }
  
  @HostListener("window:resize", ['$event'])
  onResize(event: Event) {
    if(window.innerWidth < 1000)
    {
      this.layoutService.$showSidebar.next(false);
      this.showSidebar = false;
    }
  }
}
