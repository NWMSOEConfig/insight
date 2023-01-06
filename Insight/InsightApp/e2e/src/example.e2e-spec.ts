import { browser, by, element } from 'protractor';

describe('Example Test', () => {
  it('example read & click', async () => {
    browser.get(browser.baseUrl); // Navigate to browser

    expect('Category A').toEqual(
      await element(by.buttonText('Category A')).getText() // Read that Category A exists
    );

    element(by.buttonText('Category A')).click(); // CLick Category A

    expect('Subcategory 1').toEqual(
      await element(by.buttonText('Subcategory 1')).getText() // Read that Subcategory 1 exists (now that we're under Category A)
    );
  });
});
