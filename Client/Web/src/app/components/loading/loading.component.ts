import { Component } from '@angular/core';
import { ComponentService } from '../../services/component.service';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [],
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.css'
})
export class LoadingComponent {
  show: boolean = false;
  constructor(
    private componentService: ComponentService
  ) {
    componentService.$showLoadingStatus.subscribe(next => this.show = next);
  }
}
