using System.Text.Json.Serialization;

namespace NetPresence.Models;

public class AppConfig
{
    [JsonPropertyName("mode")]
    public int Mode { get; set; } = 0; // 0=Mouse, 1=Keyboard, 2=Both, 3=Scroll

    [JsonPropertyName("idle_interval_seconds")]
    public int IdleIntervalSeconds { get; set; } = 60;

    [JsonPropertyName("mouse_pixels")]
    public int MousePixels { get; set; } = 1;

    [JsonPropertyName("circular_mouse_movement")]
    public bool CircularMouseMovement { get; set; } = false;

    [JsonPropertyName("start_immediately_on_launch")]
    public bool StartImmediatelyOnLaunch { get; set; } = true;

    [JsonPropertyName("show_notification")]
    public bool ShowNotification { get; set; } = false;

    [JsonPropertyName("launch_on_startup")]
    public bool LaunchOnStartup { get; set; } = false;

    [JsonPropertyName("is_running")]
    public bool IsRunning { get; set; } = false;
}
