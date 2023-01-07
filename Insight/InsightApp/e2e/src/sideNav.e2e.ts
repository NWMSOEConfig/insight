import { browser, by, element } from 'protractor';

describe('Side navigation', () => {
  // Constants
  const sideNavId = 'sideNav';
  const sideNavToggleId = 'sideNavToggle';
  const leftIcon = 'keyboard_arrow_left';
  const rightIcon = 'keyboard_arrow_right';

  beforeEach(() => {
    browser.get(browser.baseUrl); // Navigate to browser
  });

  afterEach(() => {
    browser.executeScript('window.localStorage.clear();');
    browser.refresh();
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

    browser.sleep(1000);

    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle
    
    browser.sleep(1000);

    expect(leftIcon).toEqual(
      await element(by.id(sideNavToggleId)).getText() // Verify that toggle arrow is pointing left (to indicate closing sideNav)
    );

    expect('none').not.toEqual(
      await element(by.id(sideNavId)).getCssValue('display') // Verify that the side navigation is visible
    );
  });

  it('can can close side navigation', async () => {
    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle

    expect(rightIcon).toEqual(
      await element(by.id(sideNavToggleId)).getText() // Verify that toggle arrow is now pointing right
    );

    browser.sleep(1000);

    expect('none').toEqual(
      await element(by.id(sideNavId)).getCssValue('display') // Verify that the side navigation is hidden
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

  it("can persist user's toggle selection", async () => {
    await element(by.id(sideNavToggleId)).click(); // Click the side nav toggle

    const toggleDirection = element(by.id(sideNavToggleId)).getText();

    browser.refresh(); // Refresh the browser

    expect(toggleDirection).toEqual(element(by.id(sideNavToggleId)).getText()); // Verify that toggle direction has not changed
  });
});
