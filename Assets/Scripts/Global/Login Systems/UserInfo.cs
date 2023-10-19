
using System;
using System.Collections.Generic;

public class UserInfo
{
    public string userID;
    public string email;
    public string displayname;
    public int coin;
    public int apple;
    public int pear;
    public int orange;
    public int lastcompletedlevelindex = -1;
    public int lastpaidlevelindex = 0;
    public int score;
    public int cannonlevel = 1;
    public int velocitylevel = 1;
    public int tirelevel = 1;
    public int suspensionlevel = 1;

    public List<double> times = new List<double>();
    public List<int> fps = new List<int>();
    public string device = null;
}