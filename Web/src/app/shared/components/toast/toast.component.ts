import { NgClass, NgIf } from '@angular/common';
import { Component, MissingTranslationStrategy, OnInit } from '@angular/core';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css'],
  standalone: true,
  imports: [
    NgIf,
    NgClass
  ]
})
export class ToastComponent implements OnInit {
  msg: string|null = "";

  constructor(
    private readonly toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.toastService.displayStatus.subscribe(msg => {
      this.msg = msg;

      setTimeout(() => this.msg = null, 2000);
    });
  }


}
