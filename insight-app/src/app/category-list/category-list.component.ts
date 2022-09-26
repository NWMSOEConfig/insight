import { Component, OnInit } from '@angular/core';

const tempData = [
  {
    name: 'Root',
    children: [
      {
        type: 'category',
        id: 1,
      },
      {
        type: 'category',
        id: 2,
      },
      {
        type: 'category',
        id: 3,
      },
    ]
  },
  {
    name: 'Category A',
    children: [
      {
        type: 'category',
        id: 4,
      },
    ],
  },
  {
    name: 'Category B',
    children: [
      {
        type: 'category',
        id: 5,
      },
    ],
  },
  {
    name: 'Category C',
    children: [
      {
        type: 'category',
        id: 6,
      },
    ],
  },
  {
    name: 'Subcategory 1',
    children: [
      {
        type: 'setting',
        id: 0,
      },
    ],
  },
  {
    name: 'Subcategory 2',
    children: [
      {
        type: 'setting',
        id: 1,
      },
    ],
  },
  {
    name: 'Subcategory 3',
    children: [
      {
        type: 'setting',
        id: 2,
      },
    ],
  },
]

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categoryId = 0;
  category = tempData[this.categoryId];

  constructor() { }

  ngOnInit(): void {
  }

  getCategory(id: number) {
    return tempData[id];
  }
}
