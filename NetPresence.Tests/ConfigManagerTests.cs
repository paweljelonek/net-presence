using System;
using System.IO;
using System.Text.Json;
using NetPresence.Models;
using NetPresence.Services;
using Xunit;

namespace NetPresence.Tests;

public class ConfigManagerTests : IDisposable
{
    private readonly string _testDir;
    private readonly string _testConfigPath;

    public ConfigManagerTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), $"NetPresenceTests_{Guid.NewGuid()}");
        _testConfigPath = Path.Combine(_testDir, "config.json");
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, recursive: true);
    }

    // ── AppConfig defaults ──────────────────────────────────────────────────

    [Fact]
    public void AppConfig_DefaultMode_IsZero()
        => Assert.Equal(0, new AppConfig().Mode);

    [Fact]
    public void AppConfig_DefaultIdleInterval_Is60()
        => Assert.Equal(60, new AppConfig().IdleIntervalSeconds);

    [Fact]
    public void AppConfig_DefaultMousePixels_IsOne()
        => Assert.Equal(1, new AppConfig().MousePixels);

    [Fact]
    public void AppConfig_DefaultCircularMouseMovement_IsFalse()
        => Assert.False(new AppConfig().CircularMouseMovement);

    [Fact]
    public void AppConfig_DefaultStartImmediatelyOnLaunch_IsTrue()
        => Assert.True(new AppConfig().StartImmediatelyOnLaunch);

    [Fact]
    public void AppConfig_DefaultShowNotification_IsFalse()
        => Assert.False(new AppConfig().ShowNotification);

    [Fact]
    public void AppConfig_DefaultLaunchOnStartup_IsFalse()
        => Assert.False(new AppConfig().LaunchOnStartup);

    // ── ConfigManager.Load ──────────────────────────────────────────────────

    [Fact]
    public void Load_WhenFileDoesNotExist_ReturnsDefaultConfig()
    {
        var config = ConfigManager.Load(_testConfigPath);

        Assert.NotNull(config);
        Assert.Equal(0, config.Mode);
        Assert.Equal(60, config.IdleIntervalSeconds);
    }

    [Fact]
    public void Load_WhenFileIsCorrupted_ReturnsDefaultConfig()
    {
        Directory.CreateDirectory(_testDir);
        File.WriteAllText(_testConfigPath, "{ this is not valid json !!! }");

        var config = ConfigManager.Load(_testConfigPath);

        Assert.NotNull(config);
        Assert.Equal(60, config.IdleIntervalSeconds);
    }

    [Fact]
    public void Load_WhenFileIsEmpty_ReturnsDefaultConfig()
    {
        Directory.CreateDirectory(_testDir);
        File.WriteAllText(_testConfigPath, string.Empty);

        var config = ConfigManager.Load(_testConfigPath);

        Assert.NotNull(config);
    }

    [Fact]
    public void Load_WhenFileIsValid_ReturnsCorrectValues()
    {
        var expected = new AppConfig
        {
            Mode = 2,
            IdleIntervalSeconds = 120,
            MousePixels = 10,
            CircularMouseMovement = true,
            StartImmediatelyOnLaunch = false,
            ShowNotification = true,
            LaunchOnStartup = true
        };

        Directory.CreateDirectory(_testDir);
        File.WriteAllText(_testConfigPath,
            JsonSerializer.Serialize(expected, new JsonSerializerOptions { WriteIndented = true }));

        var loaded = ConfigManager.Load(_testConfigPath);

        Assert.Equal(expected.Mode, loaded.Mode);
        Assert.Equal(expected.IdleIntervalSeconds, loaded.IdleIntervalSeconds);
        Assert.Equal(expected.MousePixels, loaded.MousePixels);
        Assert.Equal(expected.CircularMouseMovement, loaded.CircularMouseMovement);
        Assert.Equal(expected.StartImmediatelyOnLaunch, loaded.StartImmediatelyOnLaunch);
        Assert.Equal(expected.ShowNotification, loaded.ShowNotification);
        Assert.Equal(expected.LaunchOnStartup, loaded.LaunchOnStartup);
    }

    // ── ConfigManager.Save ──────────────────────────────────────────────────

    [Fact]
    public void Save_CreatesFileIfNotExists()
    {
        ConfigManager.Save(new AppConfig(), _testConfigPath);

        Assert.True(File.Exists(_testConfigPath));
    }

    [Fact]
    public void Save_CreatesDirectoryIfNotExists()
    {
        Assert.False(Directory.Exists(_testDir));

        ConfigManager.Save(new AppConfig(), _testConfigPath);

        Assert.True(Directory.Exists(_testDir));
    }

    [Fact]
    public void Save_WritesValidJson()
    {
        ConfigManager.Save(new AppConfig(), _testConfigPath);

        var raw = File.ReadAllText(_testConfigPath);
        var parsed = JsonSerializer.Deserialize<AppConfig>(raw);

        Assert.NotNull(parsed);
    }

    // ── Round-trip (Save → Load) ────────────────────────────────────────────

    [Fact]
    public void SaveThenLoad_PreservesAllValues()
    {
        var original = new AppConfig
        {
            Mode = 3,
            IdleIntervalSeconds = 300,
            MousePixels = 5,
            CircularMouseMovement = true,
            StartImmediatelyOnLaunch = false,
            ShowNotification = true,
            LaunchOnStartup = true
        };

        ConfigManager.Save(original, _testConfigPath);
        var restored = ConfigManager.Load(_testConfigPath);

        Assert.Equal(original.Mode, restored.Mode);
        Assert.Equal(original.IdleIntervalSeconds, restored.IdleIntervalSeconds);
        Assert.Equal(original.MousePixels, restored.MousePixels);
        Assert.Equal(original.CircularMouseMovement, restored.CircularMouseMovement);
        Assert.Equal(original.StartImmediatelyOnLaunch, restored.StartImmediatelyOnLaunch);
        Assert.Equal(original.ShowNotification, restored.ShowNotification);
        Assert.Equal(original.LaunchOnStartup, restored.LaunchOnStartup);
    }

    [Fact]
    public void SaveThenLoad_OverwritesPreviousValues()
    {
        var first = new AppConfig { IdleIntervalSeconds = 30 };
        ConfigManager.Save(first, _testConfigPath);

        var second = new AppConfig { IdleIntervalSeconds = 999 };
        ConfigManager.Save(second, _testConfigPath);

        var restored = ConfigManager.Load(_testConfigPath);

        Assert.Equal(999, restored.IdleIntervalSeconds);
    }

    // ── JSON property names ─────────────────────────────────────────────────

    [Fact]
    public void Save_UsesSnakeCasePropertyNames()
    {
        var config = new AppConfig
        {
            Mode = 2,
            IdleIntervalSeconds = 90,
            MousePixels = 3,
            CircularMouseMovement = true,
            StartImmediatelyOnLaunch = false,
            ShowNotification = true,
            LaunchOnStartup = true
        };

        ConfigManager.Save(config, _testConfigPath);
        var raw = File.ReadAllText(_testConfigPath);

        Assert.Contains("\"mode\"", raw);
        Assert.Contains("\"idle_interval_seconds\"", raw);
        Assert.Contains("\"mouse_pixels\"", raw);
        Assert.Contains("\"circular_mouse_movement\"", raw);
        Assert.Contains("\"start_immediately_on_launch\"", raw);
        Assert.Contains("\"show_notification\"", raw);
        Assert.Contains("\"launch_on_startup\"", raw);
    }

    [Fact]
    public void Save_WritesIndentedJson()
    {
        ConfigManager.Save(new AppConfig(), _testConfigPath);
        var raw = File.ReadAllText(_testConfigPath);

        Assert.Contains("\n", raw);
    }

    // ── Partial / forward-compatible JSON ───────────────────────────────────

    [Fact]
    public void Load_WhenOnlySomeFieldsPresent_MissingFieldsUseDefaults()
    {
        Directory.CreateDirectory(_testDir);
        File.WriteAllText(_testConfigPath, "{\"mode\": 1}");

        var config = ConfigManager.Load(_testConfigPath);

        Assert.Equal(1, config.Mode);
        Assert.Equal(60, config.IdleIntervalSeconds);
        Assert.Equal(1, config.MousePixels);
        Assert.False(config.CircularMouseMovement);
        Assert.True(config.StartImmediatelyOnLaunch);
        Assert.False(config.ShowNotification);
        Assert.False(config.LaunchOnStartup);
    }

    [Fact]
    public void Load_WhenJsonContainsUnknownFields_DoesNotThrow()
    {
        Directory.CreateDirectory(_testDir);
        File.WriteAllText(_testConfigPath, "{\"mode\": 0, \"unknown_field\": \"value\"}");

        var config = ConfigManager.Load(_testConfigPath);

        Assert.NotNull(config);
    }

    // ── GetConfigDirectory ──────────────────────────────────────────────────

    [Fact]
    public void GetConfigDirectory_ReturnsPathEndingWithAppName()
    {
        var dir = ConfigManager.GetConfigDirectory();

        Assert.EndsWith("net-presence", dir);
    }

    [Fact]
    public void GetConfigDirectory_ReturnsNonEmptyPath()
    {
        var dir = ConfigManager.GetConfigDirectory();

        Assert.False(string.IsNullOrWhiteSpace(dir));
    }
}
