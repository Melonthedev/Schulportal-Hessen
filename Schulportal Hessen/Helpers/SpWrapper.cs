using ColorCode.Compilation.Languages;
using HtmlAgilityPack;
using Schulportal_Hessen.Models;
using Schulportal_Hessen.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Helpers;
public class SpWrapper {
    private readonly NetworkService _networkService;
    private readonly AuthService _authService;

    public SpWrapper(NetworkService networkService, AuthService authService) {
        _networkService = networkService;
        _authService = authService;
    }

    public async Task<bool> AutoLoginAsync() {
        return await _authService.AutoLoginAsync();
    }

    public AuthService GetAuthService() {

        return _authService;
    }

    public async Task LogoutAsync() {
        await _authService.LogoutAsync();
        return;
    }

    public async Task<HtmlDocument?> GetHtmlAsync(string url) {
        var response = await _networkService.GetAsync(url);
        if (!response.IsSuccessStatusCode) {
            _networkService.ShowNetworkError();
        }
        var html = await response.Content.ReadAsStringAsync();
        if (html == null) return null;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }


    // TODO: Save XPaths in config
    public async Task<List<(string, int)>?> GetSchoolIdsAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/index.php");
        var schoolList = doc.DocumentNode.SelectSingleNode("//*[@id=\"accordion\"]");
        if (schoolList == null) { return null; }

        var nodes = schoolList.SelectNodes("//div[@class='list-group']/a");
        if (nodes == null || nodes.Count == 0) { return null; }

        var schools = new List<(string, int)>();

        foreach (var node in nodes) {
            // Extrahiere den Schulnamen (innerer Text des <a> Tags ohne <small>)
            var schoolName = node.SelectSingleNode(".//text()[normalize-space()]").InnerText.Trim();
            // Extrahiere die Schul-ID aus dem data-id Attribut
            var schoolId = node.GetAttributeValue("data-id", "");
            // Füge den Namen und die ID zur Liste hinzu
            schools.Add((schoolName, int.Parse(schoolId)));
        }

        return schools;
    }

    public void PingSchulportal() {
        GetHtmlAsync("https://start.schulportal.hessen.de/index.php");
    }

    public async Task<string?> GetFullNameAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");

        var nachname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[2]/td[2]");
        var vorname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[3]/td[2]");
        if (nachname == null || vorname == null) return null;
        var name = vorname.InnerText + " " + nachname.InnerText;
        return name;
    }

    public async Task<string?> GetSurNameAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var vorname = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[3]/td[2]");
        if (vorname == null) return null;
        var name = vorname.InnerText;
        return name;
    }


    public async Task<string?> GetDateOfBirthAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var dateOfBirth = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[4]/td[2]");
        if (dateOfBirth == null) return null;
        return dateOfBirth.InnerHtml;
    }

    public async Task<string?> GetSchoolClassAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/benutzerverwaltung.php?a=userData");
        var schoolClass = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div/table/tbody/tr[6]/td[2]");
        if (schoolClass == null) return null;
        return schoolClass.InnerHtml;
    }

    public async Task<List<TimeTableLesson>> GetTimetableAsync() {
        var doc = await GetHtmlAsync("https://start.schulportal.hessen.de/stundenplan.php");
        var timetableBody = doc.DocumentNode.SelectSingleNode("//*[@id=\"all\"]/div[1]/div/div[3]/table/tbody");
        var output = new List<TimeTableLesson>();
        if (timetableBody == null) return output;

        for (var i = 1; i < 10; i++) {
            var tr = timetableBody.SelectSingleNode($"tr[{i + 1}]");
            if (tr == null) continue;

            for (var d = 1; d < 6; d++) {
                var lessonParent = tr.SelectSingleNode($"td[{d + 1}]");
                if (lessonParent == null) continue;
                for (int y = 1; y <= lessonParent.ChildNodes.Count; y++) {
                    var lesson = lessonParent.SelectSingleNode($"div[{y}]");
                    if (lesson == null) continue;
                    var subject = lesson.SelectSingleNode("b").InnerText.Trim();
                    var room = lesson.SelectSingleNode("small").InnerText.Trim();
                    lesson.RemoveChild(lesson.SelectSingleNode("b"));
                    lesson.RemoveChild(lesson.SelectSingleNode("small"));
                    var teacher = lesson.InnerText.Trim();
                    var timeTableLesson = new TimeTableLesson() {
                        Day = d,
                        Hour = i,
                        Room = room,
                        Course = new Course() {
                            SchulPortalTimeTableName = subject,
                            Teacher = teacher
                        }
                    };
                    output.Add(timeTableLesson);
                }
            }
        }

        return output;
    }

}
