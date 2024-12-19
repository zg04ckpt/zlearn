import { CommonModule, NgClass, NgStyle } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { User } from "../../../entities/user/user.entity";
import { LayoutService } from "../../../services/layout.service";
import { UserService } from "../../../services/user.service";

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    NgStyle,
    NgClass,
    CommonModule,
    RouterLink
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  show: boolean = true;
  user: User|null = null;

  constructor(
    private layoutService: LayoutService,
    private userService: UserService
  ) {
    layoutService.$showSidebar.subscribe(next => this.show = next);
    userService.$currentUser.subscribe(next => this.user = next);
  }

  ngOnInit(): void {
    if(window.innerWidth < 600)
      this.show = false;
  }

  toggleStatus() {
    this.show = !this.show;
    this.layoutService.$showSidebar.next(this.show);
  }
}
