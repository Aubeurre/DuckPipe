using System.Text.Json;

public static class TimeLogManager
{
    private static string timelogPath = "project_timelog.json";
    private static List<TimeLogData> timeLogs = new List<TimeLogData>();

    public static void Load(string prodPath)
    {
        string path = Path.Combine(prodPath, "Dev", "DangerZone", timelogPath);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            timeLogs = JsonSerializer.Deserialize<List<TimeLogData>>(json) ?? new List<TimeLogData>();
        }
    }

    public static void Save(string prodPath)
    {
        string fullPath = Path.Combine(prodPath, "Dev", "DangerZone", timelogPath);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(timeLogs, options);
        File.WriteAllText(fullPath, json);
    }

    public static void Add(TimeLogData log, string prodPath)
    {
        timeLogs.Add(log);
        Save(prodPath);
    }

    public static List<TimeLogData> GetAll(string prodPath)
    {
        Load(prodPath);
        return timeLogs;
    }
}