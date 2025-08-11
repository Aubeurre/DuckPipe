using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

public class TimeLog
{
    public string AssetName { get; set; }
    public string Artist { get; set; }
    public string Hours { get; set; }
    public DateTime Date { get; set; }
    public string Department { get; set; }

    public TimeLog(string assetName, string department, string artist, string hours, DateTime date)
    {
        AssetName = assetName;
        Department = department;
        Artist = artist;
        Hours = hours;
        Date = date;
    }
}

public static class TimeLogManager
{
    private static string timelogPath = "project_timelog.json";
    private static List<TimeLog> timeLogs = new List<TimeLog>();

    public static void Load(string prodPath)
    {
        if (File.Exists(Path.Combine(prodPath, "Dev", "DangerZone", timelogPath)))
        {
            string json = File.ReadAllText(Path.Combine(prodPath, "Dev", "DangerZone", timelogPath));
            timeLogs = JsonSerializer.Deserialize<List<TimeLog>>(json) ?? new List<TimeLog>();
        }
    }

    public static void Save(string prodPath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(timeLogs, options);
        string fullPath = Path.Combine(prodPath, "Dev", "DangerZone", timelogPath);
        File.WriteAllText(fullPath, json);
    }

    public static void Add(TimeLog log, string prodPath)
    {
        timeLogs.Add(log);
        Save(prodPath);
    }

    public static List<TimeLog> GetAll(string prodPath)
    {
        Load(prodPath);
        return timeLogs;
    }
}

public static class TimeLogStats
{
    private static double ParseHours(string hoursStr)
    {
        if (double.TryParse(hoursStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            return result;
        return 0;
    }

    public static double GetTotalHours(string prodPath)
    {
        return TimeLogManager.GetAll(prodPath)
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalAssets(string prodPath)
    {
        string jsonPath = Path.Combine(prodPath, "Dev", "DangerZone", "allAssets.json");
        string json = File.ReadAllText(jsonPath);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        int count = 0;

        foreach (var element in doc.RootElement.EnumerateObject())
        {
            if (element.Value.TryGetProperty("Type", out var typChar) &&
                typChar.GetString()?.Equals("Characters", StringComparison.OrdinalIgnoreCase) == true)
            {
                count++;
            }
            if (element.Value.TryGetProperty("Type", out var typeProp) &&
                typeProp.GetString()?.Equals("Props", StringComparison.OrdinalIgnoreCase) == true)
            {
                count++;
            }
        }

        return count;
    }

    public static double GetTotalShots(string prodPath)
    {
        string jsonPath = Path.Combine(prodPath, "Dev", "DangerZone", "allAssets.json");
        string json = File.ReadAllText(jsonPath);
        using var doc = JsonDocument.Parse(json);

        int count = 0;

        foreach (var element in doc.RootElement.EnumerateObject())
        {
            if (element.Value.TryGetProperty("Type", out var typeProp) &&
                typeProp.GetString()?.Equals("Shots", StringComparison.OrdinalIgnoreCase) == true)
            {
                count++;
            }
        }

        return count;
    }

    public static double GetTotalHoursByArtist(string prodPath, string artist)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalHoursByAsset(string prodPath, string assetName)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.AssetName.Equals(assetName, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalHoursByDepartment(string prodPath, string department)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }
}
