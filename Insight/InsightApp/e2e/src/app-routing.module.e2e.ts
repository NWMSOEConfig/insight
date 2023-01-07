import { browser, by, element } from 'protractor';

describe('Browser', () => {
  // TODO: a better location for these
  // Essentially, constants to describe the unique HTML that defines each page
  const configurationPage = 'app-configuration-page';
  const publishPage = 'app-publish-page';
  const historyPage = 'app-history-page';

  beforeEach(async () => {
    browser.get(browser.baseUrl); // Navigate to browser
  });

  it('can route to the configuration page, which is also the default page', async () => {
    browser.get(browser.baseUrl + 'configuration'); // Navigate to /configuration page

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl); // Verify it reroutes browser to the base url (Unique because configuration page is also the default page)

    element.all(by.tagName(configurationPage)).then(function (items) {
      expect(items.length).toBe(1); // Verify that configuration page tag exists
    });
  });

  it('can route to the publish page', async () => {
    browser.get(browser.baseUrl + 'publish'); // Navigate to /publish page

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'publish'); // Verify it reroutes browser to the publish page

    element.all(by.tagName(publishPage)).then(function (items) {
      expect(items.length).toBe(1); // Verify that publish page tag exists
    });
  });

  it('can route to the history page', async () => {
    browser.get(browser.baseUrl + 'history'); // Navigate to /history page

    expect(await browser.getCurrentUrl()).toEqual(browser.baseUrl + 'history'); // Verify it reroutes browser to the publish page

    element.all(by.tagName(historyPage)).then(function (items) {
      expect(items.length).toBe(1); // Verify that history page tag exists
    });
  });
});
