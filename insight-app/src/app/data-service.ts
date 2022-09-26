/**
 * Module for fetching data from the database.
 *
 * Since the database doesn't exist yet, the data fetched is
 * dummy data hardcoded into the application.
 */

const categories = [
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

const paramters = [
  {
    name: 'Enabled',
    type: 'boolean',
    value: true,
  },
  {
    name: 'Foo',
    type: 'number',
    value: '123',
  },
  {
    name: 'Bar',
    type: 'text',
    value: 'Text',
  },
  {
    name: 'Baz',
    type: 'email',
    value: 'a@b.com',
  },
];

export function getCategory(id: number) {
  return categories[id];
}

export function getParameter(id: number) {
  return paramters[id];
}
