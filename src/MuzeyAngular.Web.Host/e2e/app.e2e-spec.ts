import { MuzeyAngularTemplatePage } from './app.po';

describe('MuzeyAngular App', function() {
  let page: MuzeyAngularTemplatePage;

  beforeEach(() => {
    page = new MuzeyAngularTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
