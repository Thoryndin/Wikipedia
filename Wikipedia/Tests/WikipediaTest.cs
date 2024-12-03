using Microsoft.Playwright;
using NUnit.Framework;
using Wikipedia.Pages;

namespace Wikipedia.Tests;

[TestFixture]
public class WikipediaTest
{
    private IBrowser? _browser;
    private IPage? _page;

    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _page = await _browser.NewPageAsync();
    }

    [Test]
    public async Task NavigateToPhilosophyTest()
    {
        if (_page == null) throw new InvalidOperationException("Page is not initialized.");

        var wikipediaPage = new WikipediaPage(_page);
        await wikipediaPage.NavigateToRandomPageAsync();

        Console.WriteLine($"Starting page: {await wikipediaPage.GetCurrentUrlAsync()}");
        List<string> urls = new List<string> {await wikipediaPage.GetCurrentUrlAsync()};

        var redirects = 0;

        while (!(await wikipediaPage.GetCurrentUrlAsync()).Split('/').Last().Equals("Philosophy", StringComparison.OrdinalIgnoreCase))
        {
            await wikipediaPage.ClickFirstLinkAsync();

            if (urls.Contains(await wikipediaPage.GetCurrentUrlAsync()))
            {
                Assert.Fail("Navigation stopped because this page was already visited");
                break;
            }

            urls.Add(await wikipediaPage.GetCurrentUrlAsync());

            redirects++;
        }

        Assert.That((await wikipediaPage.GetCurrentUrlAsync()).Contains("Philosophy"), Is.True, $"Failed to reach 'Philosophy' after {redirects} redirects.");
        Console.WriteLine($"Reached 'Philosophy' after {redirects} redirects.");
    }

    [OneTimeTearDown]
    public async Task TeardownAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
    }
}