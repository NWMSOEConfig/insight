import { Component, OnInit } from '@angular/core';

import { getCategory, getSetting } from '../data-service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categoryId = 0;
  category = getCategory(this.categoryId);
  breadcrumbs = [{type: 'category', id: 0}];
  settingClicked = false;
  settingId = 0;

  constructor() { }

  ngOnInit(): void {
  }

  click(child: {type: string, id: number}) {
    this.breadcrumbs.push(child);

    if (child.type === 'category') {
      this.categoryId = child.id;
      this.category = getCategory(this.categoryId);
    } else {
      this.settingClicked = true;
      this.settingId = child.id;
    }
  }

  cancel(): void {
    this.breadcrumbs.pop();

    // settings are only ever the final element of the breadcrumbs,
    // and there is always at least one category in the breadcrumbs (the root),
    // so this is guaranteed to be a child-category
    const previous = this.breadcrumbs[this.breadcrumbs.length - 1];
    this.categoryId = previous.id;
    this.category = getCategory(previous.id);
    this.settingClicked = false;
  }

  clickBreadcrumb(child: {type: string, id: number}): void {
    const index = this.breadcrumbs.indexOf(child);

    this.breadcrumbs.length = index + 1;

    const last = this.breadcrumbs[this.breadcrumbs.length - 1];

    if (last.type === 'category') {
      this.categoryId = last.id;
      this.category = getCategory(last.id);
      this.settingClicked = false;
    } else {
      this.settingClicked = true;
      this.settingId = last.id;
    }
  }

  getPath(): string {
    return this.breadcrumbs.map(child =>
      child.type === 'category'
        ? getCategory(child.id).name
        : getSetting(child.id).name)
      .join(' > ');
  }

  getName(child: {type: string, id: number}): string {
    if (child.type === 'category') {
      return getCategory(child.id).name;
    } else {
      return getSetting(child.id).name;
    }
  }
}
