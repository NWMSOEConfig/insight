import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';

/**
 * TODO: Temporary location
 */
enum DbObjects {
  Tenant,
  Category,
  Setting,
  Parameter,
}

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
})
export class CategoryListComponent implements OnInit {
  settingClicked: boolean = false;
  settingName = '';
  children: any = [];
  level: DbObjects;
  parent: any;
  breadcrumbs: any;
  settings: any;

  constructor(private apiService: ApiService) {
    this.settingName = JSON.parse(localStorage.getItem('settingName')!);

    this.parent = JSON.parse(localStorage.getItem('parent')!);

    this.level = parseInt(
      localStorage.getItem('level') || String(DbObjects.Tenant)
    );

    const noCrumbs = JSON.stringify([{ Name: 'Root', level: this.level }]);
    this.breadcrumbs = JSON.parse(
      localStorage.getItem('breadcrumbs') || noCrumbs
    );
  }

  /** API request call to parent and its children
   * TODO: Make API requests less redundant
   * TODO: Rename this?
   * @param target Parent we want to request (Tenant, Category, Subcategory, Setting)
   */
  requestDbTarget(target: DbObjects) {
    this.children = []; // Clear out children
    this.level = target; // Update our current level

    if (target == DbObjects.Tenant) {
      this.parent = this.settings;

      // Get distinct categories
      var categories = ([] = this.parent.filter(
        (s0: any, i: number, arr: any) =>
          arr.findIndex(
            (s1: { Category: string }) => s0.Category === s1.Category
          ) === i
      ));

      // Sort categories alphabetically
      categories = categories.sort((a: any, b: any) =>
        a.Category.localeCompare(b.Category)
      );

      // Insert categories to be visible
      categories.forEach((setting: any) =>
        this.children.push({ Name: setting.Category })
      );
    } else if (target == DbObjects.Category) {
      var settings = ([] = this.settings.filter(
        (setting: any) => setting.Category == this.parent.Name
      ));

      // Insert categories to be visible
      settings.forEach((setting: any) =>
        this.children.push({ Name: setting.Name })
      );
    } else {
      this.settingClicked = true;
    }

    this.updateLocalStorage();
  }

  ngOnInit(): void {
    this.apiService.getAllSettings().subscribe((data) => {
      this.settings = data;
      this.requestDbTarget(this.level);
    });
  }

  clickChild(child: any) {
    this.parent = child;
    this.settingName = this.parent.Name;
    this.level++;
    this.breadcrumbs.push({ Name: this.parent.Name, level: this.level });
    console.log(this.breadcrumbs);
    this.requestDbTarget(this.level);
  }

  cancel(): void {
    this.breadcrumbs.pop();
    this.settingClicked = false;
    this.requestDbTarget(this.breadcrumbs.length - 1);
  }

  clickBreadcrumb(breadcrumb: any): void {
    const index = this.breadcrumbs.indexOf(breadcrumb);
    this.breadcrumbs.length = index + 1;
    this.settingClicked = false;
    this.parent = this.breadcrumbs[index];
    this.requestDbTarget(breadcrumb.level);
  }

  updateLocalStorage(): void {
    localStorage.setItem('settingName', JSON.stringify(this.settingName));
    localStorage.setItem('parent', JSON.stringify(this.parent));
    localStorage.setItem('level', String(this.level));
    localStorage.setItem('breadcrumbs', JSON.stringify(this.breadcrumbs));
  }
}
