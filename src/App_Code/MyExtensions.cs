using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml;

/// <summary>
/// Summary description for MyExtensions
/// </summary>
public static class MyExtensions
{
    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }

    /// <summary>
    /// DataTable 转 List
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
    {
        List<PropertyInfo> prlist = new List<PropertyInfo>();
        Type t = typeof(TResult);
        Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
        List<TResult> oblist = new List<TResult>();

        foreach (DataRow row in dt.Rows)
        {
            TResult ob = new TResult();
            prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
            oblist.Add(ob);
        }
        return oblist;
    }

    public static void ForEach(this IEnumerable<IGrouping<char, char>> group, Action<IGrouping<char, char>> action)
    {
        var values = group.Select(g => g).ToArray();
        Array.ForEach(values, action);
    }

    public static void ForEach(this IEnumerable<IGrouping<int, string>> group, Action<IGrouping<int, string>> action)
    {
        var values = group.Select(g => g).ToArray();
        Array.ForEach(values, action);
    }

    public static string ToCHN(this int num)
    {
        try
        {
            string Chns = "零一二三四五六七八九";
            string Nums = "0123456789";
            return Chns[Nums.IndexOf(num.ToString())].ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

    #region JSON Utility
    public static string ToJSON(this object obj)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }

    public static string ToJSON(this object obj, int recursionDepth)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.RecursionLimit = recursionDepth;
        return serializer.Serialize(obj);
    }

    public static T FromJsonTo<T>(this string strJson)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<T>(strJson);
    }
    #endregion

    public static string ToMd5(this string input)
    {
        MD5 md5Hasher = MD5.Create();
        byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }

    /// <summary>
    /// Unix时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static double ToUnixTimeStamp(this DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }

    public static string ToUTF8(this string s)
    {
        return HttpUtility.UrlEncode(s, Encoding.UTF8);
    }

    public static XDocument ToXDocument(this XmlDocument document)
    {
        return document.ToXDocument(LoadOptions.None);
    }

    public static XDocument ToXDocument(this XmlDocument document, LoadOptions options)
    {
        using (XmlNodeReader reader = new XmlNodeReader(document))
        {
            return XDocument.Load(reader, options);
        }
    }

    public static DateTime FromUTCToDateTime(this string time)
    {
        string[] cx = time.Split(' ');
        System.Globalization.DateTimeFormatInfo g = new System.Globalization.DateTimeFormatInfo();
        g.LongDatePattern = "dd MMMM yyyy";
        DateTime datetime = DateTime.Parse(string.Format("{0} {1} {2} {3}", cx[2], cx[1], cx[5], cx[3]), g);
        return datetime;
    }
}
