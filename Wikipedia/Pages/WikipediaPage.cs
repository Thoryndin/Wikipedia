using Microsoft.Playwright;

namespace Wikipedia.Pages;

public class WikipediaPage
{
    private readonly IPage _page;

    public WikipediaPage(IPage page)
    {
        _page = page;
    }

    private ILocator FirstLink
        => _page.Locator("(//div[contains(@class, 'mw-content-ltr')]/p//a[starts-with(@href, '/wiki/')])[1]");

    public async Task NavigateToRandomPageAsync()
    {
        await _page.GotoAsync("https://en.wikipedia.org/wiki/Special:Random");
    }

    public async Task<string> GetCurrentUrlAsync()
    {
        return _page.Url;
    }

    public async Task ClickFirstLinkAsync()
    {
        Console.WriteLine($"Redirecting to: {await FirstLink.GetAttributeAsync("href")}");
        await FirstLink.ClickAsync();
    }
}