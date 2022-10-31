import { browser } from 'protractor';

describe('Angular App', () => {
  it('check title', async () => {
    browser.get(browser.baseUrl);
  });
});
