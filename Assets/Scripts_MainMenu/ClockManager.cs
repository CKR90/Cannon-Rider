using UnityEngine;
using System;

public static class ClockManager
{
    public static string GetTomorrow()
    {
        DateTime date = DateTime.Now.AddDays(1);
        return date.ToString();
    }
    public static string GetRemainingTime(string dateTimeString)
    {
        DateTime savedDateTime;
        if (!DateTime.TryParse(dateTimeString, out savedDateTime))
        {
            return null;
        }
        TimeSpan remainingTime = savedDateTime.Subtract(DateTime.Now);

        return string.Format("{0:00}:{1:00}:{2:00}",
            Math.Abs(remainingTime.Hours), Math.Abs(remainingTime.Minutes), Math.Abs(remainingTime.Seconds));
    }
    public static int GetRemainingSeconds(string dateTimeString)
    {
        DateTime savedDateTime;
        if (!DateTime.TryParse(dateTimeString, out savedDateTime))
        {
            return 0;
        }

        TimeSpan remainingTime = savedDateTime.Subtract(DateTime.Now);
        int remainingSeconds = (int)remainingTime.TotalSeconds;

        return Math.Max(0, remainingSeconds);
    }
    public static int GetTimeDifferenceInSeconds(string dateTimeString)
    {
        DateTime savedDateTime;
        if (!DateTime.TryParse(dateTimeString, out savedDateTime))
        {
            return 0;
        }

        TimeSpan timeDifference = savedDateTime - DateTime.Now;

        return (int)timeDifference.TotalSeconds;
    }
}
