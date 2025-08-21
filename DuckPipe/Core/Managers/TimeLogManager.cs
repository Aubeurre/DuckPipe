
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using DuckPipe.Core;
using DuckPipe.Core.Models;
using DuckPipe.Core.Services;
using DuckPipe.Core.Utils;

public static class TimeLogStats
{
    private static double ParseHours(string hoursStr) { if (double.TryParse(hoursStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double result)) return result; return 0; }

    public static double GetTotalHours(string prodPath)
    {
        return TimeLogManager.GetAll(prodPath)
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalNodes(string prodPath)
    {
        return NodeService.CountByType(prodPath, "Characters") +
               NodeService.CountByType(prodPath, "Props");
    }

    public static double GetTotalShots(string prodPath)
    {
        return NodeService.CountByType(prodPath, "Shots");
    }

    public static double GetTotalHoursByArtist(string prodPath, string artist)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalHoursByNode(string prodPath, string nodeName)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.NodeName.Equals(nodeName, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalHoursByDepartment(string prodPath, string department)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));
    }

    public static double GetTotalHoursByNodeAndDept(string prodPath, string nodeName, string dept)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => log.NodeName.Equals(nodeName, StringComparison.OrdinalIgnoreCase))
            .Where(log => log.Department.Equals(dept, StringComparison.OrdinalIgnoreCase))
            .Sum(log => ParseHours(log.Hours));

    }

    public static List<string> GetAllDepartments(string prodPath)
    {
        return TimeLogManager.GetAll(prodPath)
            .Where(log => !string.IsNullOrWhiteSpace(log.Department))
            .Select(log => log.Department)
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }
}