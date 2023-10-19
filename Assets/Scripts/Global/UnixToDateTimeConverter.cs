using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnixToDateTimeConverter
{
    public static DateTime Convert(long unixTimeCode)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeCode).ToLocalTime();
    }
}
