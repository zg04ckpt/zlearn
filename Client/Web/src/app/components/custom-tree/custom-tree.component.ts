import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CategoryNode } from '../../entities/management/category-node.entity';
import { NgClass, NgStyle } from '@angular/common';

@Component({
  selector: 'app-custom-tree',
  standalone: true,
  imports: [
    NgClass,
    NgStyle
  ],
  templateUrl: './custom-tree.component.html',
  styleUrl: './custom-tree.component.css'
})
export class CustomTreeComponent {
  @Input() items!: CategoryNode[];
  @Output() onAdd = new EventEmitter<{id: string}>();
  @Output() onDelete = new EventEmitter<{id: string}>();
  @Output() onEdit = new EventEmitter<{id: string}>();

  add(id: string) {
    this.onAdd.emit({id});
  }

  delete(id: string) {
    this.onDelete.emit({id});
  }

  edit(id: string) {
    this.onEdit.emit({id});
  }
}
