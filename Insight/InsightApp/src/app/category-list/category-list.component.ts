import { Component, OnInit } from '@angular/core';
import { DatabaseSetting } from 'src/models/databaseSetting';
import { ApiService } from '../api-service/api.service';

/**
 * TODO: Temporary location
 */
enum DbObjects {
  Tenant,
  Category,
  Setting,
  Parameter,
  Search,
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
  settings!: DatabaseSetting[];
  searchSetting = '';

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

    this.searchSetting = JSON.parse(localStorage.getItem('searchSetting') || '""');
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
      var categorySettings = ([] = this.settings.filter(
        (setting: any) => setting.Category == this.parent.Name
      ));

      // Insert categories to be visible
      categorySettings.forEach((categorySetting: any) =>
        this.children.push({ Name: categorySetting.Name })
      );
    } else if (target == DbObjects.Search) {
      const filteredSettings = this.settings.filter((setting: any) =>
        setting.Name.toLowerCase().includes(this.searchSetting.toLowerCase())
      );

      filteredSettings.forEach((setting: any) =>
        this.children.push({ Name: setting.Name })
      );
    } else {
      this.settingClicked = true;
    }

    this.updateLocalStorage();
  }

  ngOnInit(): void {
    // Get all settings
    this.apiService.getAllSettings().subscribe((data: any) => {
      // Sort settings alphabetically just in case
      this.settings = data.sort((a: any, b: any) =>
        a.Category.localeCompare(b.Category)
      );

      this.requestDbTarget(this.level);
    });
  }

  searchSettings() {
    // To prevent user confusion, only keep a link back to the root
    this.breadcrumbs = [{ Name: 'Root', level: DbObjects.Tenant }];
    this.parent = this.breadcrumbs[0];

    if (this.searchSetting === '') {
      // Empty search string -> reset to showing everything
      this.requestDbTarget(DbObjects.Tenant);
    } else {
      this.requestDbTarget(DbObjects.Search);
    }
  }

  clickChild(child: any) {
    this.parent = child;
    this.settingName = this.parent.Name;
    this.level++;
    this.breadcrumbs.push({ Name: this.parent.Name, level: this.level });

    this.searchSetting = '';

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

    this.searchSetting = '';

    this.requestDbTarget(breadcrumb.level);
  }

  updateLocalStorage(): void {
    localStorage.setItem('settingName', JSON.stringify(this.settingName));
    localStorage.setItem('parent', JSON.stringify(this.parent));
    localStorage.setItem('level', String(this.level));
    localStorage.setItem('breadcrumbs', JSON.stringify(this.breadcrumbs));
    localStorage.setItem('searchSetting', JSON.stringify(this.searchSetting));
  }
}
