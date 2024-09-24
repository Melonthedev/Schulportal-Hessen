using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Schulportal_Hessen.Helpers;
public class SpWrapper
{
    private readonly HttpClient _httpClient;

    public SpWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetHtmlAsync(string url)
    {
           var response = await _httpClient.GetAsync(url);
           response.EnsureSuccessStatusCode();
           return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> GetFullName()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var nachname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[2]/td[2]");
        var vorname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[3]/td[2]");
        if (nachname == null || vorname == null) return null;
        var name = vorname.InnerText + " " + nachname.InnerText;
        return name;
    }


    public async Task<string> GetDateOfBirth()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var dateOfBirth = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[4]/td[2]");
        if (dateOfBirth == null) return null;
        return dateOfBirth.InnerHtml;
    }

    public async Task<string> GetSchoolClass()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var schoolClass = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[6]/td[2]");
        if (schoolClass == null) return null;
        return schoolClass.InnerHtml;
    }


}
