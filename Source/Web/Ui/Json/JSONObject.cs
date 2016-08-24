using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Json
{
    /// <summary>
    /// Wrapper to make the API compatible with the Java version. 
    /// For internal use.
    /// </summary>
    public class JSONObject
    {
        private JsonData _data;

        public JSONObject(JsonData data)
        {
            _data = data;
        }

        public JSONObject()
        {
            _data = new JsonData();
        }

        public string getString(string key)
        {
            return (string)_data[key];
        }

        public string optString(string key)
        {
            if (_data[key] != null && _data[key].IsString)
            {
                return getString(key);
            }
            return null;
        }

        public JSONObject getJSONObject(string key)
        {
            return new JSONObject(_data[key]);
        }

        public JSONObject optJSONObject(string key)
        {
            if (_data[key] != null && _data[key].IsObject)
            {
                return getJSONObject(key);
            }
            return null;
        }

        public JSONArray optJSONArray(string key)
        {
            if (_data[key] != null && _data[key].IsArray)
            {
                return getJSONArray(key);
            }
            return null;
        }

        public int getInt(string key)
        {
            return (int)_data[key];
        }

        public int optInt(string key)
        {
            if (_data[key] != null && _data[key].IsInt)
            {
                return getInt(key);
            }
            return 0;
        }

        public bool getBoolean(string key)
        {
            return (bool)_data[key];
        }

        public bool optBoolean(string key)
        {
            if (_data[key] != null && _data[key].IsBoolean)
            {
                return getBoolean(key);
            }
            return false;
        }

        public JSONArray getJSONArray(string key)
        {
            return new JSONArray(_data[key]);
        }

        public JsonData getJsonData(string key)
        {
            return _data[key];
        }

        public JsonData optJsonData(string key)
        {
            return _data[key];
        }

        public DateTime getDateTime(string key)
        {
            return Convert.ToDateTime(getString(key));
        }

        public void put(string key, string value)
        {
            _data[key] = value;
        }

        public void put(string key, DateTime value)
        {
            put(key, value.ToString("s"));
        }

        public void put(string key, int value)
        {
            _data[key] = value;
        }

        public JsonData ToJsonData()
        {
            return _data;
        }

        public Hashtable ToHashtable()
        {
            Hashtable result = new Hashtable();
            foreach (string key in _data.Keys)
            {
                switch (_data[key].GetJsonType())
                {
                    case JsonType.None:
                        break;
                    case JsonType.Object:
                        result[key] = new JSONObject(_data[key]).ToHashtable();
                        break;
                    case JsonType.Array:
                        result[key] = new JSONArray(_data[key]).ToList();
                        break;
                    case JsonType.String:
                        result[key] = (string)_data[key];
                        break;
                    case JsonType.Int:
                        result[key] = (int)_data[key];
                        break;
                    case JsonType.Long:
                        result[key] = (long)_data[key];
                        break;
                    case JsonType.Double:
                        result[key] = (double)_data[key];
                        break;
                    case JsonType.Boolean:
                        result[key] = (bool)_data[key];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            return result;
        }
    }
}
