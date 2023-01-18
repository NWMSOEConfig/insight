import { browser, by, element } from 'protractor';

describe('An individual setting', () => {
  const optionClassName = 'mat-list-item mat-focus-indicator'; // The classname for a category, subcategory, setting tile

  beforeEach(async () => {
    browser.get(browser.baseUrl + 'configuration');

    await element.all(by.className(optionClassName)).first().click(); // Click Category

    await element.all(by.className(optionClassName)).first().click(); // Click Subcategory

    await element.all(by.className(optionClassName)).first().click(); // Click Setting
  });

  afterEach(() => {
    browser.executeScript('window.localStorage.clear();');
    browser.refresh();
  });

  it('can be navigated to', async () => {
    // All navigation has been done in the beforeEach() up above

    expect(await element(by.buttonText('Queue')).isPresent()).toBe(true); // Verify there is a Queue button on the page
  });

  it('has "Is browser visible" toggle', async () => {
    expect(await element(by.tagName('mat-slide-toggle')).isPresent()).toBe(
      true
    ); // Verify toggle exists on page

    expect(await element(by.tagName('mat-slide-toggle')).getText()).toEqual(
      'Is browser visible'
    ); // Verify 'Is browser visible' text exists
  });

  it('has a value input field', async () => {
    expect(await element(by.xpath('//mat-form-field//input')).isPresent()).toBe(
      true
    ); // Verify there is an input field
  });
});
