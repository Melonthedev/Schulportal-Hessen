using System.Diagnostics;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Schulportal_Hessen.Services;

public class AuthService
{

    private readonly HttpClient _httpClient;
    public string SPHSession = null;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<bool> HandleAuthorizationRequestAsync()
    {
        var loginUrl = GetLoginUrl();
        if (string.IsNullOrEmpty(loginUrl)) return false;
        Debug.WriteLine("Authenticating to " + loginUrl + " ...");

        var (username, password) = GetCredentials();
        if (username == null || password == null) return false;
        var user = GetSchoolId() + "." + username;
        var postData = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("user2", username),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("stayconnected", "1"),
                new KeyValuePair<string, string>("password", password)
        });

        // 1. GET SID REQUEST
        HttpResponseMessage responseSIDReq = await _httpClient.PostAsync(loginUrl, postData);
        if (!responseSIDReq.Headers.Contains("Set-Cookie")) return false;
        var cookies = responseSIDReq.Headers.GetValues("Set-Cookie");
        _httpClient.DefaultRequestHeaders.Add("Cookie", string.Join(";", cookies));

        HttpResponseMessage responseLoginReq = await _httpClient.PostAsync(loginUrl, postData);
        if (!responseLoginReq.Headers.Contains("Set-Cookie")) return false;
        var loginCookies = responseLoginReq.Headers.GetValues("Set-Cookie");
        _httpClient.DefaultRequestHeaders.Add("Cookie", string.Join(";", loginCookies));

        foreach (var cookie in _httpClient.DefaultRequestHeaders.GetValues("Cookie"))
        {
            //Debug.WriteLine($"Cookie: {cookie}");
            if (cookie.Contains("SPH-Session"))
            {
                var sessionCookie = cookie.Split(';')[0];
                SPHSession = sessionCookie.Split('=')[1];
                Debug.WriteLine($"Session Cookie: {SPHSession}");
            }
        }

        var content2 = await responseLoginReq.Content.ReadAsStringAsync();
        Debug.WriteLine(content2);
        Debug.WriteLine(loginCookies.ToString());
        Debug.WriteLine(responseLoginReq.StatusCode);

        return true;
    }

    public void SaveCredentials(string username, string password)
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["UserName"] = username;
        var vault = new PasswordVault();
        var credential = new PasswordCredential("SchulportalHessen", username, password);
        vault.Add(credential);
    }

    public (string Username, string Password) GetCredentials()
    {
        var vault = new PasswordVault();
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var userName = localSettings.Values["UserName"] as string;
            if (string.IsNullOrEmpty(userName)) return (null, null);
            var credential = vault.Retrieve("SchulportalHessen", userName);
            credential.RetrievePassword();
            return (credential.UserName, credential.Password);
        }
        catch (Exception)
        {
            return (null, null);
        }
    }

    public void DeleteCredentials()
    {
        var vault = new PasswordVault();
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var userName = localSettings.Values["UserName"] as string;
            if (string.IsNullOrEmpty(userName)) return;
            var credential = vault.Retrieve("SchulportalHessen", userName);
            vault.Remove(credential);
            localSettings.Values["UserName"] = null;
        }
        catch (Exception)
        {
        }
    }

    public void SaveSchoolId(string schoolId)
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["SelectedSchoolId"] = schoolId;
    }

    public string? GetSchoolId()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        return localSettings.Values["SelectedSchoolId"] as string;
    }

    public void SaveLoginUrl(string loginUrl)
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["LoginUrl"] = loginUrl;
    }

    public string? GetLoginUrl()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        return localSettings.Values["LoginUrl"] as string;
    }
}