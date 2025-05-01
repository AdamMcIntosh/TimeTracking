namespace TimeTracking;

public static class DataUtil
{
    public static long GetInt64(object dbvalue)
    {
        long tmp;
        try
        {
            tmp = Convert.ToInt64(dbvalue);

        }
        catch
        {
            tmp = 0;
        }

        return tmp;
    }

    public static int GetInt32(object? dbvalue)
    {
        if (dbvalue == null) return 0;

        int tmp;
        try
        {
            tmp = Convert.ToInt32(dbvalue);

        }
        catch
        {
            tmp = 0;
        }

        return tmp;
    }

    public static double GetDouble(object? dbvalue)
    {
        if (null == dbvalue) return 0;
        double rtn;
        try
        {
            rtn = Convert.ToDouble(dbvalue);
        }
        catch
        {
            rtn = 0;
        }

        return rtn;
    }

    public static decimal GetDecimal(object? dbvalue)
    {
        if (null == dbvalue) return 0;

        decimal rtn;
        try
        {
            rtn = Convert.ToDecimal(dbvalue);
        }
        catch
        {
            rtn = 0;
        }

        return Math.Round(rtn, 2, MidpointRounding.AwayFromZero);
    }

    public static DateTime GetDateTime(object? dbvalue)
    {
        if (null == dbvalue) return DateTime.Now;

        DateTime rtn;
        try
        {
            rtn = Convert.ToDateTime(dbvalue);
        }
        catch
        {
            rtn = DateTime.Now;
        }

        return rtn;

    }

    public static string GetString(object? dbvalue)
    {
        if (null == dbvalue) return string.Empty;

        string rtn;
        try
        {
            rtn = Convert.ToString(dbvalue);
        }
        catch
        {
            rtn = string.Empty;
        }

        return rtn;
    }

    public static bool GetBoolean(object dbvalue)
    {
        bool rtn;
        try
        {
            rtn = Convert.ToBoolean(dbvalue);
        }
        catch
        {
            rtn = false;
        }

        return rtn;
    }

    public static string FormatDateTimeToYYYYMMDD(object? dbObj)
    {
        var rtn = DateTime.Now.ToString("yyyy-MM-dd");
        try
        {
            DateTime.TryParse(dbObj.ToString(), out var result);
            rtn = result.ToString("yyyy-MM-dd");
        }
        catch
        {
            //an error occurred
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        return rtn;
    }

    public static DateTime ToDateTime(object? dbObj)
    {
        var rtn = DateTime.Now;

        try
        {
            DateTime.TryParse(dbObj.ToString(), out var result);
            rtn = result;

        }
        catch (Exception e)
        {
            //an error occurred
        }

        return rtn;
    }
}