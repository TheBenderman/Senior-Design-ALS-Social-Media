using System;
using CoreTweet;
using UnityEngine;

public static class Utilities
{ 
	public static string ElapsedTime(DateTime dtEvent)
	{
        int intYears = DateTime.Now.Year - dtEvent.Year;
		int intMonths = DateTime.Now.Month - dtEvent.Month;
		int intDays = DateTime.Now.Day - dtEvent.Day;
		int intHours = DateTime.Now.Hour - dtEvent.Hour;
		int intMinutes = DateTime.Now.Minute - dtEvent.Minute;
		int intSeconds = DateTime.Now.Second - dtEvent.Second;
		if (intYears > 0) return String.Format("{0} {1} ago", intYears, (intYears == 1) ? "year" : "years");
		else if (intMonths > 0) return String.Format("{0} {1} ago", intMonths, (intMonths == 1) ? "month" : "months");
		else if (intDays > 0) return String.Format("{0} {1} ago", intDays, (intDays == 1) ? "day" : "days");
		else if (intHours > 0) return String.Format("{0} {1} ago", intHours, (intHours == 1) ? "hour" : "hours");
		else if (intMinutes > 0) return String.Format("{0} {1} ago", intMinutes, (intMinutes == 1) ? "minute" : "minutes");
		else if (intSeconds > 0) return String.Format("{0} {1} ago", intSeconds, (intSeconds == 1) ? "second" : "seconds");
		else
		{
			return String.Format("{0} {1} ago", dtEvent.ToShortDateString(), dtEvent.ToShortTimeString());
		}
	}

    public static String cleanProfileImageURL(Status status)
	{
        return status.User.ProfileImageUrlHttps.Replace("_normal", "");
    }

	public static String cleanProfileImageURLForUser(User user)
	{
        return user.ProfileImageUrlHttps.Replace("_normal", "");
    }
}