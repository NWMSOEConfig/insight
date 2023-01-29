import { browser, by, element } from 'protractor';

describe('History page', () => {
  const filteredCountElement = element(by.id('filteredCommitsCount')); // Filtered commits CSS element
  const totalCountElement = element(by.id('totalCommitsCount')); // Total commits CSS element
  const searchElement = element(by.xpath('//button[@type="submit"]')); // Search button
  const clearFiltersElement = element(by.xpath('//button[@type="reset"]')); // Clear filters button
  const itemsPerPageElement = element(
    by.xpath('//app-history-page//mat-select')
  );
  const nextPageElement = element(
    by.xpath('//button[@aria-label="Next page"]')
  );
  const previousPageElement = element(
    by.xpath('//button[@aria-label="Previous page"]')
  );

  /**
   * Helper function to click an element and immediately click search after
   * @param filter Filter we would like to utilize (Date Range, Id, User, Setting)
   * @param value1 Primary value we want to set
   * @param value2 Secondary value we want to set. Useful for daterange
   */
  function searchWithFilter(filter: string, value1: string, value2?: string) {
    var filterElement = element(
      by.xpath('//mat-form-field[contains(., "' + filter + '")]')
    );

    filterElement.click(); // Click filter element

    if (value2 !== undefined) {
      // If value2
      var startElement = element(
        by.xpath('//input[contains(@class, "mat-start-date")]')
      );

      startElement.click(); // Click start range input
      startElement.sendKeys(value1); // Send some input

      var endElement = element(
        by.xpath('//input[contains(@class, "mat-end-date")]')
      );

      endElement.click(); // Click end range input
      endElement.sendKeys(value1); // Send some input
    } else {
      var inputElement = element(
        by.xpath('//mat-form-field[contains(., "' + filter + '")]//input')
      );

      inputElement.sendKeys(value1); // Input a commit id
    }

    searchElement.click(); // Click search

    browser.sleep(1000); // Let browser wait for 1 second
  }

  async function isValueOfFilterEmpty(filter: string): Promise<boolean> {
    if (filter === 'Date Range') {
      var start = await element(
        by.xpath('//input[contains(@class, "mat-start-date")]')
      ).getAttribute('value');

      var end = await element(
        by.xpath('//input[contains(@class, "mat-end-date")]')
      ).getAttribute('value');

      return start === '' && start === end;
    } else {
      var input = await element(
        by.xpath('//mat-form-field[contains(., "' + filter + '")]//input')
      ).getAttribute('value');

      return input === '';
    }
  }

  /**
   * Helper function to verify that a filter has been applied
   * by checking if filtered count does not equal total count
   */
  async function expectFilteredCommitsToNotEqualTotalCommits() {
    expect(await totalCountElement.getText()).not.toEqual(
      await filteredCountElement.getText()
    );
  }

  async function isPaginationEnabled(): Promise<boolean> {
    return parseInt(await totalCountElement.getText()) >
      parseInt(await itemsPerPageElement.getText())
      ? true
      : false;
  }

  async function getCards(): Promise<any> {
    return element.all(by.xpath('//app-history-page//mat-expansion-panel'));
  }

  beforeEach(() => {
    browser.get(browser.baseUrl + 'history'); // Navigate to history page
  });

  afterEach(() => {
    browser.executeScript('window.localStorage.clear();'); // Clear cache
    browser.refresh(); // Refresh the page
  });

  it('can be navigated to', async () => {
    // Navigation is in the beforeEach method above and browser routing tests are done in app-routing.module.e2e.ts
    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'history'); // Verify we are on the history page
  });

  it('has filtered commits equal to total commits on page load', async () => {
    // Navigation is in the beforeEach method above and browser routing tests are done in app-routing.module.e2e.ts
    expect(filteredCountElement.getText()).toEqual(totalCountElement.getText()); // Filtered count should match all count at start
  });

  it('has functional date range filter', async () => {
    searchWithFilter('Date Range', '01/01/2000', '01/01/2000');

    expectFilteredCommitsToNotEqualTotalCommits();
  });

  it('has functional search by id filter', async () => {
    searchWithFilter('Id', 'A92CE2L');

    expectFilteredCommitsToNotEqualTotalCommits();
  });

  it('has functional search by user filter', async () => {
    searchWithFilter('User', 'Lad');

    expectFilteredCommitsToNotEqualTotalCommits();
  });

  it('has functional search by setting filter', async () => {
    searchWithFilter('Setting', 'NotARealSetting');

    expectFilteredCommitsToNotEqualTotalCommits();
  });

  it('has a functional "Clear Filters" button', async () => {
    // Set arbitrary values
    searchWithFilter('Date Range', '01/01/2000', '01/01/2000');
    searchWithFilter('Id', 'A92CE2L');
    searchWithFilter('User', 'Lad');
    searchWithFilter('Setting', 'NotARealSetting');

    // Verify filter has worked
    expectFilteredCommitsToNotEqualTotalCommits();

    // Click "Clear Filters" button
    clearFiltersElement.click();

    // Verify values are empty
    expect(await isValueOfFilterEmpty('Date Range')).toBe(true);
    expect(await isValueOfFilterEmpty('Id')).toBe(true);
    expect(await isValueOfFilterEmpty('User')).toBe(true);
    expect(await isValueOfFilterEmpty('Setting')).toBe(true);
  });

  it('enables pagination if commit count is greater than "Items per page"', async () => {
    if (await isPaginationEnabled()) {
      expect(await nextPageElement.getAttribute('disabled')).toBeNull();
    }
  });

  it('disables pagination if commit count is less than or equal to "Items per page"', async () => {
    if (!(await isPaginationEnabled())) {
      expect(await nextPageElement.getAttribute('disabled')).toEqual('true');
      expect(await previousPageElement.getAttribute('disabled')).toEqual(
        'true'
      );
    }
  });

  it('can change items per page', async () => {
    var initialItemsPerPage = parseInt(await itemsPerPageElement.getText());

    getCards().then(async (items) => {
      expect(items.length).toBeLessThanOrEqual(initialItemsPerPage);
    });

    itemsPerPageElement.click(); // Click "Items per page" drop-down

    element(by.xpath('(//span[@class="mat-option-text"])[last()]')).click(); // Click max items per page

    browser.sleep(1000);

    getCards().then(async (items) => {
      expect(items.length).toBeGreaterThanOrEqual(initialItemsPerPage);
    });
  });

  it('can page forward', async () => {
    expect(await isPaginationEnabled()).toEqual(true);

    var currentPage = getCards();

    nextPageElement.click();

    browser.sleep(1000);

    var nextPage = getCards();

    nextPage.then(async (c0: any) => {
      currentPage.then(async (c1: any) => {
        expect(c0).not.toEqual(c1);
      });
    });
  });

  it('can page backward', async () => {
    expect(await isPaginationEnabled()).toEqual(true);

    nextPageElement.click();

    browser.sleep(1000);

    var currentPage = getCards();

    previousPageElement.click();

    browser.sleep(1000);

    var previousPage = getCards();

    previousPage.then(async (c0: any) => {
      currentPage.then(async (c1: any) => {
        expect(c0).not.toEqual(c1);
      });
    });
  });

  it('can open a card', async () => {
    element.all(by.id('commitHeader')).first().click();

    browser.sleep(1000);

    expect(
      await element.all(by.id('commitDescription')).first().isDisplayed()
    ).toBe(true);
  });
});
