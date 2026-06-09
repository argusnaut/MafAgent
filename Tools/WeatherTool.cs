using System.ComponentModel;

namespace MafAgent.Tools;

public static class WeatherTool
{
    [Description("Obtém a temperatura atual de uma determinada localização")]
    public static string GetWeather(
        [Description("Cidade de onde a temperatura será consultada")]
        string location) => $"A temperatura atual em {location} é 30°C";
}