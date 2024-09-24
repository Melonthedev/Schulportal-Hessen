using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Schulportal_Hessen.Services;

namespace Schulportal_Hessen.Helpers;
public class SpWrapper
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    public bool isLoggedIn = false;

    public SpWrapper(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
        //AutoLoginAsync();
    }

    public async Task<string> GetHtmlAsync(string url)
    {
           var response = await _httpClient.GetAsync(url);
           response.EnsureSuccessStatusCode();
           return await response.Content.ReadAsStringAsync();
    }


    public async Task AutoLoginAsync()
    {
        if (isLoggedIn) return;
        isLoggedIn = await _authService.HandleAuthorizationRequestAsync();
        Debug.WriteLine(isLoggedIn);
        Debug.WriteLine(await GetFullNameAsync());
    }

    public async Task<List<(string, int)>?> GetSchoolIdsAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/index.php");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var schoolList = doc.DocumentNode.SelectSingleNode("//*[@id=\"accordion\"]");

        var nodes = schoolList.SelectNodes("//div[@class='list-group']/a");
        if (nodes == null || nodes.Count == 0) { return null; }

        var schools = new List<(string, int)>();

        foreach (var node in nodes)
        {
            // Extrahiere den Schulnamen (innerer Text des <a> Tags ohne <small>)
            var schoolName = node.SelectSingleNode(".//text()[normalize-space()]").InnerText.Trim();

            // Extrahiere die Schul-ID aus dem data-id Attribut
            var schoolId = node.GetAttributeValue("data-id", "");

            // Füge den Namen und die ID zur Liste hinzu
            schools.Add((schoolName, int.Parse(schoolId)));
        }   

        return schools;
    }

    public async Task<string?> GetFullNameAsync()
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


    public async Task<string?> GetDateOfBirthAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var dateOfBirth = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[4]/td[2]");
        if (dateOfBirth == null) return null;
        return dateOfBirth.InnerHtml;
    }

    public async Task<string?> GetSchoolClassAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var schoolClass = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[6]/td[2]");
        if (schoolClass == null) return null;
        return schoolClass.InnerHtml;
    }


}
