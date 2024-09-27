using System.Diagnostics;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Schulportal_Hessen.Services;

public class AuthService
{

    private readonly NetworkService _networkService;
    public string? SPHSession = null;
    public bool isLoggedIn = false;
    public Task<bool>? AutoLoginTask = null;

    public AuthService(NetworkService networkService)
    {
        _networkService = networkService;
        AutoLoginTask = AutoLoginAsync();
    }

    public async ValueTask<bool> HandleAuthorizationRequestAsync()
    {
        var loginUrl = GetLoginUrl();

        if (string.IsNullOrEmpty(loginUrl))
        {
            return false;
        }

        Debug.WriteLine("Authenticating to " + loginUrl + " ...");

        var (username, password) = GetCredentials();

        if (username == null || password == null)
        {
            return false;
        }

        var user = GetSchoolId() + "." + username;
        var postData = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("user2", username),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("stayconnected", "1"),
                new KeyValuePair<string, string>("password", password)
        });

        // 1. GET SID REQUEST
        var responseSIDReq = await _networkService.PostAsync(loginUrl, postData);
        if (!responseSIDReq.Headers.Contains("Set-Cookie")) {
            return false;
        }
        var cookies = responseSIDReq.Headers.GetValues("Set-Cookie");
        _networkService.GetHttpClient().DefaultRequestHeaders.Add("Cookie", string.Join(";", cookies));

        // 2. LOGIN REQUEST
        var responseLoginReq = await _networkService.PostAsync(loginUrl, postData);
        if (!responseLoginReq.Headers.Contains("Set-Cookie")) {
            return false;
        }
        var loginCookies = responseLoginReq.Headers.GetValues("Set-Cookie");
        _networkService.GetHttpClient().DefaultRequestHeaders.Add("Cookie", string.Join(";", loginCookies));

        foreach (var cookie in _networkService.GetHttpClient().DefaultRequestHeaders.GetValues("Cookie"))
        {
            if (cookie.Contains("SPH-Session"))
            {
                var sessionCookie = cookie.Split(';')[0];
                SPHSession = sessionCookie.Split('=')[1];
                Debug.WriteLine($"Obtained Session Cookie: {SPHSession}");
                break;
            }
        }

        var html = await responseLoginReq.Content.ReadAsStringAsync();
        Debug.WriteLine(html);
        Debug.WriteLine("Login Response: " + responseLoginReq.StatusCode);

        return true;
    }

    public async Task<bool> AutoLoginAsync()
    {
        if (AutoLoginTask != null && !AutoLoginTask.IsCompleted)
        {
            return await AutoLoginTask;
        }
        if (isLoggedIn)
        {
            return true;
        }
        isLoggedIn = await HandleAuthorizationRequestAsync();
        Debug.WriteLine(isLoggedIn ? "Logged in successfully" : "Login failed");
        AutoLoginTask = null;
        return isLoggedIn;
    }

    public async Task LogoutAsync()
    {
        var responseLogout = await _networkService.GetAsync("https://login.schulportal.hessen.de/?logout=1");
        Debug.WriteLine($"{responseLogout.StatusCode}");
        _networkService.GetHttpClient().DefaultRequestHeaders.Remove("Cookie");
        isLoggedIn = false;
        _networkService.ResetHttpClient();
        DeleteCredentials();
        Debug.WriteLine("Logged out successfully");
        return;
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