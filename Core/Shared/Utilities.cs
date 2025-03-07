﻿using Core.Application.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Core.Shared
{
    public class Utilities
    {
        public static bool isAlphaNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^(?=.+\\d)(?=.+[a-zA-Z]).*$");
            return r.IsMatch(str);
        }

        public static bool isValidUsername(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^[a-zA-Z0-9_]*$");
            return r.IsMatch(str);
        }

        public static bool isNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^[0-9]*$");
            return r.IsMatch(str);
        }
        public static bool isMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^[a-f0-9]{32}$");
            return r.IsMatch(str);
        }
        public static bool isHexStr(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^[a-fA-F0-9]+$");
            return r.IsMatch(str);
        }
        public static bool passwordCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            Regex r = new Regex("^(?=.*\\d)(?=.*[A-Z])(?=.*[_.@$!#%^&()_+=]).*$");
            return r.IsMatch(str);
        }

        public static bool isEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            Regex r = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            return r.IsMatch(email);
        }
        public static bool isPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            Regex r = new Regex(@"^[07981]{3}[0-9]{8}|234[79801]{2}[0-9]{8}$");
            return r.IsMatch(phone);
        }

        public static bool isValidName(string name)
        {
            if (name is null)
                return false;
            if (name.Split(' ').Length < 2)
                return false;
            Regex r = new Regex(@"^([a-zA-Z '\-])*$");
            return r.IsMatch(name);
        }
        public static DateTime getRandomDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
        public static long getTimeStamp(
                        int year, int month, int day,
                        int hour, int minute, int second, int milliseconds)
        {
            DateTime value = new DateTime(year, month, day);
            var date = new DateTime(1970, 1, 1, 0, 0, 0, value.Kind);
            var unixTimestamp = System.Convert.ToInt64((value - date).TotalSeconds);
            return unixTimestamp;
        }
        public static (DateTime modernDate, long unixTimestamp, long unixTimestampMS) getTodayDate()
        {
            long totalSeconds = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long totalmSecs = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(totalSeconds).ToLocalTime();
            return (modernDate: dtDateTime, unixTimestamp: totalSeconds, unixTimestampMS: totalmSecs);
        }
        public static DateTime unixTimeStampToDateTime(double unixTimeStamp, bool dateOnly = false)
        {
            try
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                if (!dateOnly)
                    return dtDateTime;
                return dtDateTime.Date;
            }
            catch (Exception)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            }
        }

        public static JObject asJObject(object obj)
        {
            JObject json = JObject.Parse(JsonConvert.SerializeObject(obj));
            return json;
        }
        public static string objectToString(object obj)
        {
            try
            {
                JObject json = JObject.FromObject(obj);
                return json.ToString();
            }
            catch { }
            return null;
        }
        public static T toObject<T>(string obj)
        {
            JObject json = JObject.Parse(obj);
            return json.ToObject<T>();
        }
        public static string findString(JObject json, string needle)
        {
            try
            {
                return json[needle].ToString();
            }
            catch
            {
                return null;
            }
        }
        public static JArray findArray(JObject json, string needle)
        {
            try
            {
                return JArray.Parse(json[needle].ToString());
            }
            catch
            {
                return null;
            }
        }
        public static JObject findObj(JObject json, string needle)
        {
            try
            {
                return JObject.Parse(json[needle].ToString());
            }
            catch
            {
                return null;
            }
        }
        public static double? findNumber(JObject json, string needle)
        {
            try
            {
                return double.Parse(json[needle].ToString());
            }
            catch
            {
                return null;
            }
        }

        public static string validBase64(string raw)
        {
            int pads = raw.Length % 4;
            if (pads > 0)
            {
                raw += new string('=', 4 - pads);
            }
            return raw;
        }

        public static bool noNullValue(object obj)
        {
            foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var data = prop.GetValue(obj);
                if (data == null)
                    return false;
            }
            return true;
        }

        public static bool noNullValue(object obj, List<string> properties)
        {
            try
            {
                foreach (string propertyName in properties)
                {
                    var data = obj.GetType().GetProperty(propertyName).GetValue(obj, null);
                    if (data == null)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static long getFileSize(IFormFile file)
        {
            return file.Length;
        }

        public static string getFileFormat(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            return extension;
        }

        public static void validateEntityFields(params string[] properties)
        {
            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property))
                {
                    throw new InputError($"The {property} is invalid");
                }
            }
        }

        public static double degreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static string randomUUID()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
