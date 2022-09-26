import { Component, OnInit } from '@angular/core';

import { getCategory } from '../data-service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categoryId = 0;
  category = getCategory(this.categoryId);
  breadcrumbs = [0];

  constructor() { }

  ngOnInit(): void {
  }

  click(child: {type: string, id: number}) {
    if (child.type === 'category') {
      this.categoryId = child.id;
      this.category = getCategory(this.categoryId);
      this.breadcrumbs.push(child.id);
    }
  }

  getPath(): string {
    return this.breadcrumbs.map(id => getCategory(id).name).join(' > ');
  }

  getName(id: number): string {
    return getCategory(id).name;
  }
}
