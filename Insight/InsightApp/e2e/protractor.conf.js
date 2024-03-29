// @ts-check
// Protractor configuration file, see link for more information
// https://github.com/angular/protractor/blob/master/lib/config.ts

const { SpecReporter } = require("jasmine-spec-reporter");

/**
 * @type { import("protractor").Config }
 */
exports.config = {
  allScriptsTimeout: 11000,
  specs: ["./src/**/*.e2e.ts"],
  directConnect: true,
  // chromeDriver: "./node_modules/webdriver-manager/selenium/chromedriver_107.0.5304.62",
  // geckoDriver: "./node_modules/webdriver-manager/selenium/geckodriver-v0.32.0",
  multiCapabilities: [
    { browserName: "chrome", marionette: true, acceptInsecureCerts: true },
    { browserName: "firefox", marionette: true, acceptInsecureCerts: true },
  ],
  baseUrl: "https://localhost:44413/",
  framework: "jasmine",
  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 30000,
    print: function () {},
  },
  onPrepare() {
    require("ts-node").register({
      project: require("path").join(__dirname, "./tsconfig.json"),
    });
    // @ts-ignore
    jasmine
      .getEnv()
      .addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
  },
};
