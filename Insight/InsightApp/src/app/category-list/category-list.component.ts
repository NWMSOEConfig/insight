import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';

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
  settingClicked: boolean = false;
  settingName = '';
  children: any = [];
  level: DbObjects;
  parent: any;
  breadcrumbs: any;

  constructor(private apiService: ApiService) {
    this.parent = JSON.parse(localStorage.getItem('parent')!);

    this.level = parseInt(
      localStorage.getItem('level') || String(DbObjects.Tenant)
    );

    const noCrumbs = JSON.stringify([{ name: 'Root', level: this.level }]);
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
        data.settingNames.forEach((name: string) => {
          this.settingName = name;
          this.apiService.getSetting(name).subscribe((data) => {
            this.children.push(data);
          });
        });
      });
    } else {
      // we've gone too far!!! 😲😲😲
      this.settingClicked = true;
      this.settingName = this.parent.name;
    }

    this.updateLocalStorage();
  }

  ngOnInit(): void {
    this.requestDbTarget(this.level);
  }

  clickChild(child: any) {
    this.parent = child;
    this.level++;
    this.breadcrumbs.push({ name: this.parent.name, level: this.level });
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
    this.requestDbTarget(breadcrumb.level);
  }

  updateLocalStorage(): void {
    localStorage.setItem('parent', JSON.stringify(this.parent));
    localStorage.setItem('level', String(this.level));
    localStorage.setItem('breadcrumbs', JSON.stringify(this.breadcrumbs));
  }
}
