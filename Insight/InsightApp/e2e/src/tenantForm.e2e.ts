import { browser, by, element } from 'protractor';

describe('Tenant form', () => {
  const tenantForm = element(by.id('tenantForm')); // Constant to describe the tenant form

  beforeEach(async () => {
    browser.get(browser.baseUrl); // Navigate to browser
    browser.executeScript('window.localStorage.clear();');
    browser.executeScript('window.sessionStorage.clear();');
    browser.driver.manage().deleteAllCookies();
  });

  it('can select a site and environment', async () => {
    const initialText = tenantForm.getText(); // Read initial text

    tenantForm.click(); // Click the tenant drop-down

    const siteText = await element
      .all(by.tagName('mat-option'))
      .first()
      .getText(); // Get site text
    await element.all(by.tagName('mat-option')).first().click(); // Click the first site option

    const environmentText = await element
      .all(by.tagName('mat-option'))
      .first()
      .getText(); // Get environment text

    expect(siteText).not.toEqual(environmentText); // Verify that site and environment aren't the same

    await element.all(by.tagName('mat-option')).first().click(); // Click the first environment option

    expect(initialText).not.toEqual(tenantForm.getText()); // Verify that drop-down has changed
    expect(await tenantForm.getText()).toEqual(
      siteText + ' (' + environmentText + ')'
    ); // Verify the change matches format i.e. "Site (Environment)"
  });

  it('recovers if a site is selected but not environment', async () => {
    const initialText = tenantForm.getText(); // Read initial text

    tenantForm.click(); // Click the tenant drop-down

    await element.all(by.tagName('mat-option')).first().click(); // Click the first site option

    browser
      .actions()
      .mouseMove(element.all(by.tagName('span')).first())
      .click(); // Move the mouse and click away i.e. first span element

    expect(initialText).toEqual(tenantForm.getText()); // Verify that drop-down has not changed
  });

  it("will persist user's selection", async () => {
    tenantForm.click(); // Click the tenant drop-down

    await element.all(by.tagName('mat-option')).first().click(); // Click the first site option

    await element.all(by.tagName('mat-option')).first().click(); // Click the first environment option

    const selection = tenantForm.getText(); // Get user selection from drop-down

    browser.refresh(); // Refresh the browser

    expect(selection).toEqual(tenantForm.getText()); // Verify that tenant's drop-down text has not changed
  });
});
