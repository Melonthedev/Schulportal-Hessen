﻿using System.Diagnostics;
using Microsoft.Extensions.Options;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Helpers;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Models;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Schulportal_Hessen.Services;

public class LocalSettingsService : ILocalSettingsService {
    private const string _defaultApplicationDataFolder = "Schulportal_Hessen\\ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _localsettingsFile;

    private IDictionary<string, object> _settings;

    private bool _isInitialized;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options) {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;
        if (RuntimeHelper.IsMSIX) {
            Debug.WriteLine($"Local data is stored in {ApplicationData.Current.LocalSettings.Locality}");
        } else {
            Debug.WriteLine($"Local data is stored in \"{_applicationDataFolder}\"");
        }
        _settings = new Dictionary<string, object>();
        Debug.WriteLine("Settings stored at: " + _applicationDataFolder + "/" + _localsettingsFile);

    }

    private async Task InitializeAsync() {
        if (!_isInitialized) {
            _settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(_applicationDataFolder, _localsettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key) {
        if (RuntimeHelper.IsMSIX) {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj)) {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        } else {
            await InitializeAsync();

            if (_settings != null && _settings.TryGetValue(key, out var obj)) {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value) {
        if (RuntimeHelper.IsMSIX) {
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
        } else {
            await InitializeAsync();

            _settings[key] = await Json.StringifyAsync(value);

            await Task.Run(() => _fileService.Save(_applicationDataFolder, _localsettingsFile, _settings));
        }
    }
}
