import { browser, by, element } from 'protractor';

describe('Side navigation', () => {
  // Constants
  const sideNavId = 'sideNav';
  const sideNavToggleId = 'sideNavToggle';
  const leftIcon = 'keyboard_arrow_left';
  const rightIcon = 'keyboard_arrow_right';

  beforeEach(async () => {
    browser.get(browser.baseUrl); // Navigate to browser
  });

  it('is open by default', async () => {
    expect(leftIcon).toEqual(
      await element(by.id(sideNavToggleId)).getText() // Verify that toggle arrow is pointing left (to indicate closing sideNav)
    );

    expect('visible').toEqual(
      await element(by.id(sideNavId)).getCssValue('visibility') // Verify that the side navigation is visible
    );
  });

  it('can open side navigation', async () => {
    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle

    browser.sleep(1000); // Delay to allow sideNav to close

    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle

    expect(leftIcon).toEqual(
      await element(by.id(sideNavToggleId)).getText() // Verify that toggle arrow is pointing left (to indicate closing sideNav)
    );

    expect('visible').toEqual(
      await element(by.id(sideNavId)).getCssValue('visibility') // Verify that the side navigation is visible
    );
  });

  it('can can close side navigation', async () => {
    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle

    browser.sleep(1000); // Delay to allow sideNav to close

    expect(rightIcon).toEqual(
      await element(by.id(sideNavToggleId)).getText() // Verify that toggle arrow is now pointing right
    );

    expect('hidden').toEqual(
      await element(by.id(sideNavId)).getCssValue('visibility') // Verify that the side navigation is hidden
    );
  });

  it('can route to the configuration page', async () => {
    await element(by.id('configuration')).click(); // Click configuration button

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl); // Verify URL change
  });

  it('can route to the publish page', async () => {
    await element(by.id('publish')).click(); // Click publish button

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'publish'); // Verify correct URL
  });

  it('can route to the history page', async () => {
    await element(by.id('history')).click(); // Click history button

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'history'); // Verify correct URL
  });
});