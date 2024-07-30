import { AfterViewInit, Component, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ToastComponent } from './shared/components/toast/toast.component';
import { ToastService } from './shared/services/toast.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { LoginComponent } from './core/auth/components/login/login.component';
import { RegisterComponent } from './core/auth/components/register/register.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [
    trigger('toggleLayout', [
      state('no-expand', style({
        'margin-left':'220px'
      })),
      state('expanded', style({
        'margin-left':'0px'
      })),
      transition("no-expand => expanded", [
        animate("0.3s 0s ease-out")
      ]),
      transition("expanded => no-expand", [
        animate("0.3s 0s ease-in")
      ])
    ])
  ]
})
export class AppComponent implements OnInit,AfterViewInit {
  private title = 'Web';
  showSidebar: boolean;
  @ViewChild(RouterOutlet) routerOutlet!: RouterOutlet;
  // private routerEvent: Subscription;

  constructor(
    // private router: Router
  ) {
    this.showSidebar = true;
    // this.routerEvent = router.events.subscribe(event => {
    //   if(event instanceof NavigationEnd)
    //   {
    //     // this.showSidebar = true;
    //     console.log(this.showSidebar);
    //   }
    // })
  }

  ngAfterViewInit(): void {
    this.routerOutlet.activateEvents.subscribe(component => {

      if(component instanceof LoginComponent) {
        //đóng slideBar khi hiển thị đăng nhập
        component.initEvent.subscribe(next => this.showSidebar = false);
        //mở slideBar khi kết thúc đăng nhập
        component.destroyRef.onDestroy(() => this.showSidebar = true);
      }

      if(component instanceof RegisterComponent) {
        //đóng slideBar khi hiển thị đăng kí
        component.initEvent.subscribe(next => this.showSidebar = false);
        //mở slideBar khi kết thúc đăng kí
        component.destroyRef.onDestroy(() => this.showSidebar = true);
      }
    });
  }

  ngOnInit(): void {
  }

  toggleStatus(isShow: boolean) {
    this.showSidebar = isShow;
  }
}
