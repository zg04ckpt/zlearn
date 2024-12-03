import { Component } from '@angular/core';
import { ComponentService } from '../../services/component.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent {
  show: boolean = false;
  message: string|null = "";
  constructor(private componentService: ComponentService) {
    componentService.$showToast.subscribe(next => {
      this.show = true;
      this.message = next;
      setTimeout(() => {
        this.show = false;
        this.message = null;
      }, 2000);
    });
  }
}
