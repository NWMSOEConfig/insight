import { Component, DebugElement, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { getCategory } from '../data-service';

/**
 * TODO: Temporary location
 */
enum DbObjects {
  Tenant,
  Category,
  Subcategory,
  Setting,
  Parameter,
}

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
  children: any = [];
  level = DbObjects.Tenant;

  constructor(private apiService: ApiService) {}

  /** API request call to parent and its children
   * TODO: Make API requests less redundant
   * TODO: Rename this?
   * @param target Parent we want to request (Tenant, Category, Subcategory, Setting)
   */
  requestDbTarget(target: DbObjects) {
    this.children = []; // Clear out children
    this.level = target; // Update our current level

    if (target == DbObjects.Tenant) {
      this.apiService.getTenant(0).subscribe((data) => {
        this.parent = data;
        this.parent.categoryIds.forEach((id: number) => {
          this.apiService.getCategory(id).subscribe((data) => {
            this.children.push(data);
          });
        });
      });
    } else if (target == DbObjects.Category) {
      this.apiService.getCategory(this.parent.id).subscribe((data) => {
        this.parent = data;
        this.parent.subcategoryIds.forEach((id: number) => {
          this.apiService.getSubcategory(id).subscribe((data) => {
            this.children.push(data);
          });
        });
      });
    } else if (target == DbObjects.Subcategory) {
      this.apiService.getSubcategory(this.parent.id).subscribe((data) => {
        this.parent = data;
        this.parent.settingIds.forEach((id: number) => {
          this.apiService.getSetting(id).subscribe((data) => {
            this.children.push(data);
          });
        });
      });
    } else { // we've gone too far!!! 😲😲😲
      this.settingClicked = true;
      this.settingId = this.parent.id;
    }
  }

  ngOnInit(): void {
    this.refreshNames();
    this.requestDbTarget(DbObjects.Tenant);
  }

  clickCategory(child: any) {
    // TODO: Fix breadcrumb
    // TODO: In backend, hook subcategory to setting
    // TODO: Consider changing subcategoryIds to "children" to be more universal
    // TODO: Consider adding typing to each model item?
    // console.log(this.level);
    this.parent = child;
    this.level++;
    this.requestDbTarget(this.level);
    // this.requestDbTarget(DbObjects.Category);

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
