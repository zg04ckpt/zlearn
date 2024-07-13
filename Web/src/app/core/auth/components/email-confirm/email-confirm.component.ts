import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingComponent } from 'src/app/shared/components/loading/loading.component';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-email-confirm',
  templateUrl: './email-confirm.component.html',
  styleUrls: ['./email-confirm.component.css'],
  standalone: true,
  imports: [
    LoadingComponent
  ]
})
export class EmailConfirmComponent {
  isValidating: boolean = true;
  isSuccess: boolean = false;
  id: string = "";
  token: string = "";
  error: string = "";
  
  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly userService: UserService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.id = params['id'];
      this.token = params['token'];
      
      this.userService.confirmEmail(this.id, this.token)
      .subscribe({
        next: res => {
          this.isValidating = false;
          this.isSuccess = true;
        },
        error: res => {
          this.isValidating = false;
          this.isSuccess = false;
          this.error = res.error?.message || res.status;
        }
      })
    });
  }

  navigate(url: string) {
    this.router.navigate([url]);
  }
}
