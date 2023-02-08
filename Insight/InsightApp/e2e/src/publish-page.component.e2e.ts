import { browser, by, element } from 'protractor';

describe('Publish page', () => {
  beforeEach(async () => {
    browser.get(browser.baseUrl + 'publish');
  });

  afterEach(() => {
    browser.executeScript('window.localStorage.clear();');
    browser.refresh();
  });

  it('can be navigated to', async () => {
    // Navigation is in the beforeEach method above and browser routing tests are done in app-routing.module.e2e.ts
    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'publish'); // Verify we are on the history page
  });

  async function getSettingsCount(): Promise<number> {
    return parseInt(await element(by.id('settingsCount')).getText());
  }

  it('has a publish button', async () => {
    expect(await element(by.buttonText('Publish')).isPresent()).toBe(true);
  });

  it('displays informational message if no settings have been modified', async () => {
    if ((await getSettingsCount()) === 0) {
      expect(await element(by.id('noQueue')).isPresent()).toBe(true); // Error HTML container should be visible

      expect(await element(by.id('yesQueue')).isPresent()).toBe(false); // Queue HTML container should not be visible
    }
  });

  it('can display modified settings', async () => {
    if ((await getSettingsCount()) > 0) {
      expect(await element(by.id('noQueue')).isPresent()).toBe(false); // Error HTML container should not be visible

      expect(await element(by.id('yesQueue')).isPresent()).toBe(true); // Queue HTML container should be visible
    }
  });

  it("displays a modified setting's name, old, and new value", async () => {
    if ((await getSettingsCount()) > 0) {
      expect(await getSettingsCount()).toEqual(
        (await element.all(by.id('settingName'))).length
      );

      expect(await getSettingsCount()).toEqual(
        (await element.all(by.id('oldValue'))).length
      );

      expect(await getSettingsCount()).toEqual(
        (await element.all(by.id('newValue'))).length
      );
    }
  });

  it('displays correct number of modified settings', async () => {
    if ((await getSettingsCount()) > 0) {
      expect(await getSettingsCount()).toEqual(
        (await element.all(by.id('settingEntry'))).length
      );
    }
  });
});
