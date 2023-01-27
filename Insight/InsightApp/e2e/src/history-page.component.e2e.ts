import { browser, by, element } from 'protractor';

describe('History page', () => {
  const filteredCountElement = element(by.id('filteredCommitsCount')); // Filtered commits CSS element
  const totalCountElement = element(by.id('totalCommitsCount')); // Total commits CSS element
  const searchElement = element(by.xpath('//button[@type="submit"]')); // Search button
  const clearFiltersElement = element(by.xpath('//button[@type="reset"]')); // Clear filters button

  beforeEach(() => {
    browser.get(browser.baseUrl + 'history'); // Navigate to history page
  });

  afterEach(() => {
    browser.executeScript('window.localStorage.clear();'); // Clear cache
    browser.refresh(); // Refresh the page
  });

  it('can be navigated to', async () => {
    // Navigation is in the beforeEach method above
    // Browser routing tests are done in app-routing.module.e2e.ts
    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'history'); // Verify we are on the history page

    expect(filteredCountElement.getText()).toEqual(totalCountElement.getText()); // Filtered count should match all count at start
  });

  it('has a functional "Clear Filters" button', async () => {
    
  });

  it('has a functional "Search" button', async () => {
    expect(await totalCountElement.getText()).toEqual(
      await filteredCountElement.getText()
    ); // Verify that initially, there is no filter applied

    var filterElement = element(
      by.xpath('//mat-form-field[contains(., "Id")]')
    );
    filterElement.click(); // Click id filter
    filterElement.sendKeys('A92CE2L'); // Input a commit id

    // Check that total count is zero - determine if it's worthwhile to even compare numbers
    if ((await totalCountElement.getText()) !== '0') {
      expect(await totalCountElement.getText()).not.toEqual(
        await filteredCountElement.getText()
      ); // Verify that a filter has been applied
    }
  });

  it('has functional date range filter', async () => {
    // For this test, we are going going to ignore the date range button as that is not necessary to functionality.
    // Instead, we'll use the underlying text input.

    expect(await totalCountElement.getText()).toEqual(
      await filteredCountElement.getText()
    ); // Verify that initially, there is no filter applied

    element(by.tagName('mat-date-range-input')).click(); // Click date range element

    var startElement = element(
      by.xpath('//input[contains(@class, "mat-start-date")]')
    );

    startElement.click(); // Click start range input
    startElement.sendKeys('01/01/2000'); // Send some input

    var endElement = element(
      by.xpath('//input[contains(@class, "mat-end-date")]')
    );

    endElement.click(); // Click end range input
    endElement.sendKeys('01/01/2000'); // Send some input

    searchElement.click(); // Trigger search

    browser.sleep(1000);

    // Check that total count is zero - determine if it's worthwhile to even compare numbers
    if ((await totalCountElement.getText()) !== '0') {
      expect(await totalCountElement.getText()).not.toEqual(
        await filteredCountElement.getText()
      ); // Verify that a filter has been applied
    }
  });

  it('has functional search by id filter', async () => {
    expect(await totalCountElement.getText()).toEqual(
      await filteredCountElement.getText()
    ); // Verify that initially, there is no filter applied

    var filterElement = element(
      by.xpath('//mat-form-field[contains(., "Id")]')
    );

    filterElement.click(); // Click id filter
    filterElement.sendKeys('Lad'); // Input a username

    searchElement.click(); // Trigger search

    browser.sleep(1000);

    // Check that total count is zero - determine if it's worthwhile to even compare numbers
    if ((await totalCountElement.getText()) !== '0') {
      expect(await totalCountElement.getText()).not.toEqual(
        await filteredCountElement.getText()
      ); // Verify that a filter has been applied
    }
  });

  it('has functional search by user filter', async () => {
    expect(await totalCountElement.getText()).toEqual(
      await filteredCountElement.getText()
    ); // Verify that initially, there is no filter applied

    var filterElement = element(
      by.xpath('//mat-form-field[contains(., "User")]')
    );

    filterElement.click(); // Click id filter
    filterElement.sendKeys('Lad'); // Input a username

    searchElement.click(); // Trigger search

    browser.sleep(1000);

    // Check that total count is zero - determine if it's worthwhile to even compare numbers
    if ((await totalCountElement.getText()) !== '0') {
      expect(await totalCountElement.getText()).not.toEqual(
        await filteredCountElement.getText()
      ); // Verify that a filter has been applied
    }
  });

  it('has functional search by setting filter', async () => {
    expect(await totalCountElement.getText()).toEqual(
      await filteredCountElement.getText()
    ); // Verify that initially, there is no filter applied

    var filterElement = element(
      by.xpath('//mat-form-field[contains(., "Search")]')
    );

    filterElement.click(); // Click id filter
    filterElement.sendKeys('Action'); // Input a username

    searchElement.click(); // Trigger search

    browser.sleep(1000);

    // Check that total count is zero - determine if it's worthwhile to even compare numbers
    if ((await totalCountElement.getText()) !== '0') {
      expect(await totalCountElement.getText()).not.toEqual(
        await filteredCountElement.getText()
      ); // Verify that a filter has been applied
    }
  });
});
