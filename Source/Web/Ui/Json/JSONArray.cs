using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Json
{
    public class JSONArray
    {
        private JsonData _data;

        public JSONArray()
        {
            _data = new JsonData();
            _data.SetJsonType(JsonType.Array);
        }

        public JSONArray(JsonData data)
        {
            if (!data.IsArray)
            {
                throw new ArgumentException("JsonData array expected.");
            }
            _data = data;
        }

        public int length()
        {
            return _data.Count;
        }

        public string getString(int i)
        {
            return (string)_data[i];
        }

        public ArrayList ToList()
        {
            ArrayList list = new ArrayList();
            foreach (JsonData item in _data)
            {
                if (item.GetJsonType() == JsonType.Array)
                {
                    list.Add(new JSONArray(item));
                }
                else
                {
                    list.Add(new JSONObject(item));
                }
            }
            return list;
        }
    }
}
