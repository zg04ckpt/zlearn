import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ManagementService } from '../../../services/management.service';
import { Router } from '@angular/router';
import { CategoryNode } from '../../../entities/management/category-node.entity';
import { ComponentService } from '../../../services/component.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { CustomTreeComponent } from '../../../components/custom-tree/custom-tree.component';
import { FormsModule } from '@angular/forms';
import { map } from 'rxjs';

interface Node {
  id: string;
  expandable: boolean;
  name: string;
  level: number;
  description: string;
}

@Component({
  selector: 'app-cate-management',
  imports: [
    CustomTreeComponent,
    FormsModule
  ],
  standalone: true,
  templateUrl: './cate-management.component.html',
  styleUrl: './cate-management.component.css'
})
export class CateManagementComponent implements OnInit {
  data: CategoryNode[] = [];
  treeMap: Map<string, {node: CategoryNode, parentId: string|null}> = new Map;

  //add
  isShowAddDialog = false;
  addModel = {
    parentName: "",
    parentId: "",
    newName: "",
    newDesc: "",
    newSlug: ""
  };

  //update
  isShowUpdateDialog = false;
  updateModel = {
    id: "",
    newParentId: "",
    oldParentId: "",
    oldName: "",
    newName: "",
    newDesc: "",
    slug: ""
  };

  constructor(
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private managementService: ManagementService,
    private router: Router,
    private componentService: ComponentService
  ) { }

  ngOnInit(): void {
    this.titleService.setTitle("Quản lý danh mục");
    this.breadcrumbService.addBreadcrumb("Quản lý danh mục", this.router.url);
    this.managementService.getCategoryTree().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.data.push(next);
      this.buildTreeMap(null, this.data[0]);
    });
  }

  private buildTreeMap(parentId: string|null, node: CategoryNode) {
    this.treeMap.set(node.id, {node, parentId});
    node.children.forEach(child => {
      this.buildTreeMap(node.id, child);
    });
  }

  showAddDialog(id: string) {
    this.addModel.parentName = this.treeMap.get(id)!.node.name;
    this.addModel.parentId = id;
    this.isShowAddDialog = true;
  }

  deleteConfirmDialog(id: string) {
    this.componentService.displayConfirmMessage(`Xác nhận xóa danh mục ${this.treeMap.get(id)!.node.name}?`, () => {
      this.managementService.deleteCate(id).subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa danh mục thành công");

        //remove child in tree
        const parentId = this.treeMap.get(id)!.parentId!;
        let parentNode = this.treeMap.get(parentId)!.node;
        parentNode.children = parentNode.children.filter(e => e.id != id);
        this.treeMap.delete(id);
      });
    });
  }

  showUpdateDialog(id: string) {
    const node = this.treeMap.get(id)!.node;
    this.updateModel = {
      id: id,
      newParentId: this.treeMap.get(id)!.parentId!,
      oldParentId: this.treeMap.get(id)!.parentId!,
      oldName: node.name,
      newName: node.name,
      newDesc: node.description,
      slug: node.slug
    }
    this.isShowUpdateDialog = true;
  }

  updateCategory() {
    this.managementService
      .updateCate(this.updateModel.id, this.updateModel.newParentId, this.updateModel.newName, this.updateModel.slug, this.updateModel.newDesc)
      .subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);        

        // update node
        const currentNode = {...this.treeMap.get(this.updateModel.id)!.node};
        currentNode.isExpand = false;
        currentNode.name = this.updateModel.newName;
        currentNode.description = this.updateModel.newDesc;
        currentNode.slug = this.updateModel.slug;

        //remove node in old parent
        const oldParentNode = this.treeMap.get(this.updateModel.oldParentId)!.node;
        oldParentNode.children = oldParentNode.children.filter(e => e.id != currentNode.id);
        
        // add current node in selected parent
        const newParentNode = this.treeMap.get(this.updateModel.newParentId)!.node;
        newParentNode.children.push(currentNode);

        //update node in treemap
        this.treeMap.set(currentNode.id, {node: currentNode, parentId: this.updateModel.newParentId});

        this.componentService.displayMessage("Cập nhật thành công!");
        this.isShowUpdateDialog = false;
      });
  }

  addNewCategory() {
    this.managementService
      .createNewCate(this.addModel.parentId, this.addModel.newName, this.addModel.newSlug, this.addModel.newDesc)
      .subscribe(newCateId => {
        this.componentService.$showLoadingStatus.next(false);
        this.isShowAddDialog = true; 

        // add new child
        const parentNode = this.treeMap.get(this.addModel.parentId)!.node;
        const newNode: CategoryNode = {
          id: newCateId,
          name: this.addModel.newName,
          description: this.addModel.newDesc,
          slug: this.addModel.newSlug,
          isExpand: false,
          children: []
        };
        parentNode.children.push(newNode);
        this.treeMap.set(newNode.id, {node: newNode, parentId: this.addModel.parentId});
        this.isShowAddDialog = false;

        //clear add dialog
        this.addModel = {
          parentName: "",
          parentId: "",
          newName: "",
          newDesc: "",
          newSlug: ""
        };
      });
  }
}
