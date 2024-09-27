using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Services;
public class NetworkService
{

    private HttpClient _httpClient;
    public bool IsOffline
    {
        get; private set;
    }

    public NetworkService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent? content)
    {
        return await SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content });
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return await SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        try
        {
            return await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("No such host is known"))
        {
            IsOffline = true;
            return new HttpResponseMessage(0);
        }
        catch (Exception ex)
        {
            HandleUnexpectedError(ex);
            return new HttpResponseMessage();
        }
    }

    public void ShowNetworkError()
    {
        App.MainWindow.ShowMessageDialogAsync("Please reconnect to the internet.", "No Connection");
    }

    private void HandleUnexpectedError(Exception ex)
    {
        Debug.WriteLine("Internal Exception (I dont care): " + ex.Message);    
    }

    public HttpClient GetHttpClient()
    {
        return _httpClient;
    }

    public void ResetHttpClient()
    {
        _httpClient.Dispose();
        _httpClient = new HttpClient();
    }


}
