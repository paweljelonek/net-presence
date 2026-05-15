using System;
using System.IO;
using System.Text.Json;
using NetPresence.Models;

namespace NetPresence.Services;

public static class ConfigManager
{
    private const string AppName = "net-presence";
    private const string ConfigFileName = "config.json";

    public static string GetConfigDirectory()
    {
        // Linux:   ~/.config/net-presence/
        // macOS:   ~/.config/net-presence/
        // Windows: %APPDATA%\net-presence\
        var configDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(configDir, AppName);
    }

    public static AppConfig Load()
        => Load(Path.Combine(GetConfigDirectory(), ConfigFileName));

    public static AppConfig Load(string configPath)
    {
        if (!File.Exists(configPath))
            return new AppConfig();

        try
        {
            var json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        catch
        {
            return new AppConfig();
        }
    }

    public static void Save(AppConfig config)
        => Save(config, Path.Combine(GetConfigDirectory(), ConfigFileName));

    public static void Save(AppConfig config, string configPath)
    {
        var dir = Path.GetDirectoryName(configPath)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(configPath, json);
    }
}
