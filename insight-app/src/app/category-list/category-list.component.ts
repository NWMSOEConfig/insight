import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { getCategory } from '../data-service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
})
export class CategoryListComponent implements OnInit {
  categoryId = 0;
  category = getCategory(this.categoryId);
  categoryNames: any = { 0: 'Root' };
  breadcrumbs = [{ type: 'category', id: 0 }];
  settingClicked = false;
  settingId = 0;
  settingNames: any = {};

  parent: any;
  categories: any = [];
  subcategories: any = [{}];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.refreshNames();
    this.apiService.getTenant(0).subscribe((data) => {
      this.parent = data;
      this.parent.categoryIds.forEach((id: number) => {
        this.apiService.getCategory(id).subscribe((data) => {
          this.categories.push(data);
        });
      });
    });
  }

  clickCategory(child: any) {
    // TODO: Fix breadcrumb
    // TODO: In backend, hook subcategory to setting
    // TODO: Consider changing subcategoryIds to "children" to be more universal
    // TODO: Consider adding typing to each model item?
    console.log(child);
    this.categories = [];
    this.apiService.getCategory(child.id).subscribe((data) => {
      this.parent = data;
      this.parent.subcategoryIds.forEach((id: number) => {
        this.apiService.getSubcategory(id).subscribe((data) => {
          this.categories.push(data);
        });
      });
    });

    // this.breadcrumbs.push(child);
    // if (this.level == "category") {
    //   console.log("X")
    //   this.refreshNames();
    // } else {
    //   this.settingClicked = true;
    //   this.settingId = child.id;
    // }
  }

  refreshNames(): void {
    // for (const child of this.categories) {
    // }
    // for (const child of this.category.children) {
    //   if (child.type === 'setting') {
    //     this.settingNames[child.id] = 'Loading...';
    //     this.apiService.getSetting(child.id).subscribe((data: any) => {
    //       this.settingNames[child.id] = data.name;
    //     });
    //   } else {
    //     this.categoryNames[child.id] = 'Loading...';
    //     this.categoryNames[child.id] = getCategory(child.id).name;
    //   }
    // }
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

  clickBreadcrumb(child: { type: string; id: number }): void {
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
}
