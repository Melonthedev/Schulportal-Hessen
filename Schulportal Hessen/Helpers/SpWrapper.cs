using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Schulportal_Hessen.Models;
using Schulportal_Hessen.Services;

namespace Schulportal_Hessen.Helpers;
public class SpWrapper
{
    private readonly NetworkService _networkService;
    private readonly AuthService _authService;

    public SpWrapper(NetworkService networkService, AuthService authService)
    {
        _networkService = networkService;
        _authService = authService;
    }

    public async Task<bool> AutoLoginAsync()
    {
        return await _authService.AutoLoginAsync();
    }

    public AuthService GetAuthService()
    {
    
        return _authService;
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        return;
    }

    public async Task<string?> GetHtmlAsync(string url)
    {
        var response = await _networkService.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            _networkService.ShowNetworkError();
        }
        return await response.Content.ReadAsStringAsync();
    }


    // TODO: Save XPaths in config
    public async Task<List<(string, int)>?> GetSchoolIdsAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/index.php");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var schoolList = doc.DocumentNode.SelectSingleNode("//*[@id=\"accordion\"]");
        if (schoolList == null) { return null; }

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

    public void PingSchulportal()
    {
        GetHtmlAsync("https://start.schulportal.hessen.de/index.php");
    }

    public async Task<string?> GetFullNameAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var nachname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[4]/div/table/tbody/tr[2]/td[2]");
        var vorname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[4]/div/table/tbody/tr[3]/td[2]");
        if (nachname == null || vorname == null) return null;
        var name = vorname.InnerText.Trim() + " " + nachname.InnerText.Trim();
        return name;
    }

    public async Task<string?> GetSurNameAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var vorname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[4]/div/table/tbody/tr[3]/td[2]");
        if (vorname == null) return null;
        var name = vorname.InnerText.Trim();
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
        var schoolClass = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[4]/div/table/tbody/tr[6]/td[2]");
        if (schoolClass == null) return null;
        return schoolClass.InnerHtml.Trim();
    }

    public async Task<List<TimeTableLesson>> GetTimetableAsync()
    {
        var html = await GetHtmlAsync("https://start.schulportal.hessen.de/stundenplan.php");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var timetableBody = doc.DocumentNode.SelectSingleNode("//*[@id=\"all\"]/div[1]/div/div[3]/table/tbody");
        var output = new List<TimeTableLesson>();
        if (timetableBody == null) return output;

        for (var i = 1; i < 10; i++)
        {
            var tr = timetableBody.SelectSingleNode($"tr[{i + 1}]");
            if (tr == null) continue;
            
            for (var d = 1; d < 6; d++)
            {
                var lesson = tr.SelectSingleNode($"td[{d + 1}]/div");
                if (lesson == null) continue;
                var subject = lesson.SelectSingleNode("b").InnerText.Trim();
                var teacher = lesson.SelectSingleNode("small").InnerText.Trim();
                lesson.RemoveChild(lesson.SelectSingleNode("b"));
                lesson.RemoveChild(lesson.SelectSingleNode("small"));
                var room = lesson.InnerText.Trim();
                var timeTableLesson = new TimeTableLesson()
                {
                    Day = d,
                    Hour = i,
                    Room = room,
                    Subject = subject,
                    Teacher = teacher
                };
                output.Add(timeTableLesson);
                // TODO: Handle several lessons in one hour (F/L)
            }
        }

        return output;
    }

}
