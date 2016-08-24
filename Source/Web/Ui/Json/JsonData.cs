/*
 * Thanks to http://litjson.sourceforge.net/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using DayPilot.Web.Ui.Json;


namespace DayPilot.Json
{
    /// <summary>
    /// Universal class for holding a transformed JSON value. Thanks to http://litjson.sourceforge.net/.
    /// </summary>
    public class JsonData : IJsonWrapper, IEquatable<JsonData>
    {
        #region Fields
        private IList<JsonData>               inst_array;
        private bool                          inst_boolean;
        private double                        inst_double;
        private int                           inst_int;
        private long                          inst_long;
        private IDictionary<string, JsonData> inst_object;
        private string                        inst_string;
        private string                        json;
        private JsonType                      type;

        // Used to implement the IOrderedDictionary interface
        private IList<KeyValuePair<string, JsonData>> object_list;
        #endregion


        #region Properties

        /// <summary>
        /// Gets the number of items in a collection.
        /// </summary>
        public int Count {
            get { return EnsureCollection ().Count; }
        }

        /// <summary>
        /// Gets true for array values.
        /// </summary>
        public bool IsArray {
            get { return type == JsonType.Array; }
        }

        /// <summary>
        /// Gets true for Boolean values.
        /// </summary>
        public bool IsBoolean {
            get { return type == JsonType.Boolean; }
        }

        /// <summary>
        /// Gets true for Double values.
        /// </summary>
        public bool IsDouble {
            get { return type == JsonType.Double; }
        }

        /// <summary>
        /// Gets true for Int values
        /// </summary>
        public bool IsInt {
            get { return type == JsonType.Int; }
        }

        /// <summary>
        /// Gets true for Long values.
        /// </summary>
        public bool IsLong {
            get { return type == JsonType.Long; }
        }

        /// <summary>
        /// Gets true for Object values.
        /// </summary>
        public bool IsObject {
            get { return type == JsonType.Object; }
        }

        /// <summary>
        /// Gets true for String values.
        /// </summary>
        public bool IsString {
            get { return type == JsonType.String; }
        }

        /// <summary>
        /// Gets true for null values.
        /// </summary>
        public bool IsNull
        {
            get { return type == JsonType.None;  }
        }

        #endregion


        #region ICollection Properties
        int ICollection.Count {
            get {
                return Count;
            }
        }

        bool ICollection.IsSynchronized {
            get {
                return EnsureCollection ().IsSynchronized;
            }
        }

        object ICollection.SyncRoot {
            get {
                return EnsureCollection ().SyncRoot;
            }
        }
        #endregion


        #region IDictionary Properties
        bool IDictionary.IsFixedSize {
            get {
                return EnsureDictionary ().IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly {
            get {
                return EnsureDictionary ().IsReadOnly;
            }
        }

        ICollection IDictionary.Keys {
            get {
                EnsureDictionary ();
                IList<string> keys = new List<string> ();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list) {
                    keys.Add (entry.Key);
                }

                return (ICollection) keys;
            }
        }

        ICollection IDictionary.Values {
            get {
                EnsureDictionary ();
                IList<JsonData> values = new List<JsonData> ();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list) {
                    values.Add (entry.Value);
                }

                return (ICollection) values;
            }
        }
        #endregion



        #region IJsonWrapper Properties
        bool IJsonWrapper.IsArray {
            get { return IsArray; }
        }

        bool IJsonWrapper.IsBoolean {
            get { return IsBoolean; }
        }

        bool IJsonWrapper.IsDouble {
            get { return IsDouble; }
        }

        bool IJsonWrapper.IsInt {
            get { return IsInt; }
        }

        bool IJsonWrapper.IsLong {
            get { return IsLong; }
        }

        bool IJsonWrapper.IsObject {
            get { return IsObject; }
        }

        bool IJsonWrapper.IsString {
            get { return IsString; }
        }
        #endregion


        #region IList Properties
        bool IList.IsFixedSize {
            get {
                return EnsureList ().IsFixedSize;
            }
        }

        bool IList.IsReadOnly {
            get {
                return EnsureList ().IsReadOnly;
            }
        }
        #endregion


        #region IDictionary Indexer
        object IDictionary.this[object key] {
            get {
                return EnsureDictionary ()[key];
            }

            set {
                if (! (key is String))
                    throw new ArgumentException (
                        "The key has to be a string");

                JsonData data = ToJsonData (value);

                this[(string) key] = data;
            }
        }
        #endregion


        #region IOrderedDictionary Indexer
        object IOrderedDictionary.this[int idx] {
            get {
                EnsureDictionary ();
                return object_list[idx].Value;
            }

            set {
                EnsureDictionary ();
                JsonData data = ToJsonData (value);

                KeyValuePair<string, JsonData> old_entry = object_list[idx];

                inst_object[old_entry.Key] = data;

                KeyValuePair<string, JsonData> entry =
                    new KeyValuePair<string, JsonData> (old_entry.Key, data);

                object_list[idx] = entry;
            }
        }
        #endregion


        #region IList Indexer
        object IList.this[int index] {
            get {
                return EnsureList ()[index];
            }

            set {
                EnsureList ();
                JsonData data = ToJsonData (value);

                this[index] = data;
            }
        }
        #endregion


        #region Public Indexers
        /// <summary>
        /// Accesses a dictionary value.
        /// </summary>
        /// <param name="prop_name"></param>
        /// <returns></returns>
        public JsonData this[string prop_name] {
            get {
                EnsureDictionary ();

                if (!inst_object.ContainsKey(prop_name))
                {
                    return null;
                }

                return inst_object[prop_name];
            }

            set {
                EnsureDictionary ();

                KeyValuePair<string, JsonData> entry =
                    new KeyValuePair<string, JsonData> (prop_name, value);

                if (inst_object.ContainsKey (prop_name)) {
                    for (int i = 0; i < object_list.Count; i++) {
                        if (object_list[i].Key == prop_name) {
                            object_list[i] = entry;
                            break;
                        }
                    }
                } else
                    object_list.Add (entry);

                inst_object[prop_name] = value;

                json = null;
            }
        }

        /// <summary>
        /// Accesses a collection value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonData this[int index] {
            get {
                EnsureCollection ();

                if (type == JsonType.Array)
                    return inst_array[index];

                return object_list[index].Value;
            }

            set {
                EnsureCollection ();

                if (type == JsonType.Array)
                    inst_array[index] = value;
                else {
                    KeyValuePair<string, JsonData> entry = object_list[index];
                    KeyValuePair<string, JsonData> new_entry =
                        new KeyValuePair<string, JsonData> (entry.Key, value);

                    object_list[index] = new_entry;
                    inst_object[entry.Key] = value;
                }

                json = null;
            }
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public JsonData ()
        {
            string justToGetRidOfTheUnusedWarning = json;
            if (justToGetRidOfTheUnusedWarning == null)
            {
            }
        }

        /// <summary>
        /// Initializes a JsonData class with a boolean value.
        /// </summary>
        /// <param name="boolean"></param>
        public JsonData (bool boolean)
        {
            type = JsonType.Boolean;
            inst_boolean = boolean;
        }

        /// <summary>
        /// Initializes a JsonData class with a double value.
        /// </summary>
        /// <param name="number"></param>
        public JsonData (double number)
        {
            type = JsonType.Double;
            inst_double = number;
        }

        /// <summary>
        /// Initializes a JsonData class with an integer value.
        /// </summary>
        /// <param name="number"></param>
        public JsonData (int number)
        {
            type = JsonType.Int;
            inst_int = number;
        }

        /// <summary>
        /// Initializes a JsonData class with a long value.
        /// </summary>
        /// <param name="number"></param>
        public JsonData (long number)
        {
            type = JsonType.Long;
            inst_long = number;
        }

        /// <summary>
        /// Initializes a JsonData class with an object. Tries to detect the type.
        /// </summary>
        /// <param name="obj"></param>
        public JsonData (object obj)
        {
            if (obj is Boolean) {
                type = JsonType.Boolean;
                inst_boolean = (bool) obj;
                return;
            }

            if (obj is Double) {
                type = JsonType.Double;
                inst_double = (double) obj;
                return;
            }

            if (obj is Int32) {
                type = JsonType.Int;
                inst_int = (int) obj;
                return;
            }

            if (obj is Int64) {
                type = JsonType.Long;
                inst_long = (long) obj;
                return;
            }

            if (obj is String) {
                type = JsonType.String;
                inst_string = (string) obj;
                return;
            }

            throw new ArgumentException (
                "Unable to wrap the given object with JsonData");
        }

        /// <summary>
        /// Initializes a JsonData class with a string value.
        /// </summary>
        /// <param name="str"></param>
        public JsonData (string str)
        {
            type = JsonType.String;
            inst_string = str;
        }
        #endregion


        #region Implicit Conversions
        /// <summary>
        /// Implicit conversion from Boolean.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData (Boolean data)
        {
            return new JsonData (data);
        }

        /// <summary>
        /// Implicit conversion from Double.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData (Double data)
        {
            return new JsonData (data);
        }

        /// <summary>
        /// Implicit conversion from Int32.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData (Int32 data)
        {
            return new JsonData (data);
        }

        /// <summary>
        /// Implicit conversion from Int64.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData (Int64 data)
        {
            return new JsonData (data);
        }

        /// <summary>
        /// Implicit conversion from String.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData (String data)
        {
            return new JsonData (data);
        }
        #endregion


        #region Explicit Conversions
        /// <summary>
        /// Explicit conversion to Boolean.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Boolean (JsonData data)
        {
            if (data.type != JsonType.Boolean)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold a double");

            return data.inst_boolean;
        }

        /// <summary>
        /// Explicit conversion to Double.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Double (JsonData data)
        {
            if (data.type != JsonType.Double)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold a double");

            return data.inst_double;
        }

        // DayPilot modified
        /// <summary>
        /// Explicit conversion to Int32.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Int32 (JsonData data)
        {
            switch (data.type)
            {
                case JsonType.Int:
                    return data.inst_int;
                case JsonType.Long:
                    return (int) data.inst_long;
                case JsonType.Double:
                    return (int) data.inst_double;
                case JsonType.String:
                case JsonType.None:
                case JsonType.Object:
                case JsonType.Boolean:
                case JsonType.Array:
                default:
                    throw new InvalidCastException("Instance of JsonData can't be cast to Int32.");
            }
        }

        // DayPilot modified
        /// <summary>
        /// Explicit conversion to Int64.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Int64 (JsonData data)
        {
            switch (data.type)
            {
                case JsonType.Int:
                    return data.inst_int;
                case JsonType.Long:
                    return data.inst_long;
                case JsonType.Double:
                    return (long) data.inst_double;
                case JsonType.String:
                case JsonType.None:
                case JsonType.Object:
                case JsonType.Boolean:
                case JsonType.Array:
                default:
                    throw new InvalidCastException("Instance of JsonData can't be cast to Int64.");
            }
        }

        /// <summary>
        /// Explicit conversion to String.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator String (JsonData data)
        {
            if (data == null)
            {
                return null;
            }

            switch (data.type)
            {
                case JsonType.Int:
                    return data.inst_int.ToString();
                case JsonType.Long:
                    return data.inst_long.ToString();
                case JsonType.Double:
                    return data.inst_double.ToString();
                case JsonType.String:
                    return data.inst_string;
                case JsonType.Boolean:
                    return data.inst_boolean.ToString();
                case JsonType.None:
                case JsonType.Object:
                case JsonType.Array:
                default:
                    throw new InvalidCastException("Instance of JsonData can't be cast to String.");
            }
        }

        // DayPilot added
        /// <summary>
        /// Explicit conversion to DateTime.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator DateTime(JsonData data)
        {
            if (data.type != JsonType.String)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a DateTime");

            return Convert.ToDateTime(data.inst_string);
        }


        #endregion


        #region ICollection Methods
        void ICollection.CopyTo (Array array, int index)
        {
            EnsureCollection ().CopyTo (array, index);
        }
        #endregion


        #region IDictionary Methods
        void IDictionary.Add (object key, object value)
        {
            JsonData data = ToJsonData (value);

            EnsureDictionary ().Add (key, data);

            KeyValuePair<string, JsonData> entry =
                new KeyValuePair<string, JsonData> ((string) key, data);
            object_list.Add (entry);

            json = null;
        }

        void IDictionary.Clear ()
        {
            EnsureDictionary ().Clear ();
            object_list.Clear ();
            json = null;
        }

        bool IDictionary.Contains (object key)
        {
            return EnsureDictionary ().Contains (key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            return ((IOrderedDictionary) this).GetEnumerator ();
        }

        void IDictionary.Remove (object key)
        {
            EnsureDictionary ().Remove (key);

            for (int i = 0; i < object_list.Count; i++) {
                if (object_list[i].Key == (string) key) {
                    object_list.RemoveAt (i);
                    break;
                }
            }

            json = null;
        }
        #endregion

        /// <summary>
        /// List of dictionary keys.
        /// </summary>
        public ICollection Keys
        {
            get
            {
                return EnsureDictionary().Keys;
            }
        }




        #region IEnumerable Methods
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return EnsureCollection ().GetEnumerator ();
        }
        #endregion


        #region IJsonWrapper Methods
        bool IJsonWrapper.GetBoolean ()
        {
            if (type != JsonType.Boolean)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a boolean");

            return inst_boolean;
        }

        double IJsonWrapper.GetDouble ()
        {
            if (type != JsonType.Double)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a double");

            return inst_double;
        }

        int IJsonWrapper.GetInt ()
        {
            if (type != JsonType.Int)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold an int");

            return inst_int;
        }

        long IJsonWrapper.GetLong ()
        {
            if (type != JsonType.Long)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a long");

            return inst_long;
        }

        string IJsonWrapper.GetString ()
        {
            if (type != JsonType.String)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a string");

            return inst_string;
        }

        void IJsonWrapper.SetBoolean (bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
            json = null;
        }

        void IJsonWrapper.SetDouble (double val)
        {
            type = JsonType.Double;
            inst_double = val;
            json = null;
        }

        void IJsonWrapper.SetInt (int val)
        {
            type = JsonType.Int;
            inst_int = val;
            json = null;
        }

        void IJsonWrapper.SetLong (long val)
        {
            type = JsonType.Long;
            inst_long = val;
            json = null;
        }

        void IJsonWrapper.SetString (string val)
        {
            type = JsonType.String;
            inst_string = val;
            json = null;
        }

/*
        string IJsonWrapper.ToJson ()
        {
            return ToJson ();
        }
*/

/*
        void IJsonWrapper.ToJson (JsonWriter writer)
        {
            ToJson (writer);
        }
*/
        #endregion


        #region IList Methods
        int IList.Add (object value)
        {
            return Add (value);
        }

        void IList.Clear ()
        {
            EnsureList ().Clear ();
            json = null;
        }

        bool IList.Contains (object value)
        {
            return EnsureList ().Contains (value);
        }

        int IList.IndexOf (object value)
        {
            return EnsureList ().IndexOf (value);
        }

        void IList.Insert (int index, object value)
        {
            EnsureList ().Insert (index, value);
            json = null;
        }

        void IList.Remove (object value)
        {
            EnsureList ().Remove (value);
            json = null;
        }

        void IList.RemoveAt (int index)
        {
            EnsureList ().RemoveAt (index);
            json = null;
        }
        #endregion


        #region IOrderedDictionary Methods
        IDictionaryEnumerator IOrderedDictionary.GetEnumerator ()
        {
            EnsureDictionary ();

            return new OrderedDictionaryEnumerator (
                object_list.GetEnumerator ());
        }

        void IOrderedDictionary.Insert (int idx, object key, object value)
        {
            string property = (string) key;
            JsonData data  = ToJsonData (value);

            this[property] = data;

            KeyValuePair<string, JsonData> entry =
                new KeyValuePair<string, JsonData> (property, data);

            object_list.Insert (idx, entry);
        }

        void IOrderedDictionary.RemoveAt (int idx)
        {
            EnsureDictionary ();

            inst_object.Remove (object_list[idx].Key);
            object_list.RemoveAt (idx);
        }
        #endregion


        #region Private Methods
        private ICollection EnsureCollection ()
        {
            if (type == JsonType.Array)
                return (ICollection) inst_array;

            if (type == JsonType.Object)
                return (ICollection) inst_object;

            throw new InvalidOperationException (
                "The JsonData instance has to be initialized first");
        }

        private IDictionary EnsureDictionary ()
        {
            if (type == JsonType.Object)
                return (IDictionary) inst_object;

            if (type != JsonType.None)
                throw new InvalidOperationException (
                    "Instance of JsonData is not a dictionary");

            type = JsonType.Object;
            inst_object = new Dictionary<string, JsonData> ();
            object_list = new List<KeyValuePair<string, JsonData>> ();

            return (IDictionary) inst_object;
        }

        private IList EnsureList ()
        {
            if (type == JsonType.Array)
                return (IList) inst_array;

            if (type != JsonType.None)
                throw new InvalidOperationException (
                    "Instance of JsonData is not a list");

            type = JsonType.Array;
            inst_array = new List<JsonData> ();

            return (IList) inst_array;
        }

        private JsonData ToJsonData (object obj)
        {
            if (obj == null)
                return null;

            if (obj is JsonData)
                return (JsonData) obj;

            return new JsonData (obj);
        }


        private static void WriteJson (IJsonWrapper obj, JsonWriter writer)
        {
            if (obj.IsString) {
                writer.Write (obj.GetString ());
                return;
            }

            if (obj.IsBoolean) {
                writer.Write (obj.GetBoolean ());
                return;
            }

            if (obj.IsDouble) {
                writer.Write (obj.GetDouble ());
                return;
            }

            if (obj.IsInt) {
                writer.Write (obj.GetInt ());
                return;
            }

            if (obj.IsLong) {
                writer.Write (obj.GetLong ());
                return;
            }

            if (obj.IsArray) {
                writer.WriteArrayStart ();
                foreach (object elem in (IList) obj)
                    WriteJson ((JsonData) elem, writer);
                writer.WriteArrayEnd ();

                return;
            }

            if (obj.IsObject) {
                writer.WriteObjectStart ();

                foreach (DictionaryEntry entry in ((IDictionary) obj)) {
                    writer.WritePropertyName ((string) entry.Key);
                    WriteJson ((JsonData) entry.Value, writer);
                }
                writer.WriteObjectEnd ();

                return;
            }
        }

        #endregion


        /// <summary>
        /// Adds a new object to the collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add (object value)
        {
            JsonData data = ToJsonData (value);

            json = null;

            return EnsureList ().Add (data);
        }

        /// <summary>
        /// Clears object/array.
        /// </summary>
        public void Clear ()
        {
            if (IsObject) {
                ((IDictionary) this).Clear ();
                return;
            }

            if (IsArray) {
                ((IList) this).Clear ();
                return;
            }
        }

        /// <summary>
        /// Compares the value to another JsonData object.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool Equals (JsonData x)
        {
            if (x == null)
                return false;

            if (x.type != this.type)
                return false;

            switch (this.type) {
            case JsonType.None:
                return true;

            case JsonType.Object:
                return this.inst_object.Equals (x.inst_object);

            case JsonType.Array:
                return this.inst_array.Equals (x.inst_array);

            case JsonType.String:
                return this.inst_string.Equals (x.inst_string);

            case JsonType.Int:
                return this.inst_int.Equals (x.inst_int);

            case JsonType.Long:
                return this.inst_long.Equals (x.inst_long);

            case JsonType.Double:
                return this.inst_double.Equals (x.inst_double);

            case JsonType.Boolean:
                return this.inst_boolean.Equals (x.inst_boolean);
            }

            return false;
        }

        /// <summary>
        /// Returns the internal type.
        /// </summary>
        /// <returns></returns>
        public JsonType GetJsonType ()
        {
            return type;
        }
        /// <summary>
        /// Sets the internal type.
        /// </summary>
        /// <param name="type"></param>
        public void SetJsonType (JsonType type)
        {
            if (this.type == type)
                return;

            switch (type) {
            case JsonType.None:
                break;

            case JsonType.Object:
                inst_object = new Dictionary<string, JsonData> ();
                object_list = new List<KeyValuePair<string, JsonData>> ();
                break;

            case JsonType.Array:
                inst_array = new List<JsonData> ();
                break;

            case JsonType.String:
                inst_string = default (String);
                break;

            case JsonType.Int:
                inst_int = default (Int32);
                break;

            case JsonType.Long:
                inst_long = default (Int64);
                break;

            case JsonType.Double:
                inst_double = default (Double);
                break;

            case JsonType.Boolean:
                inst_boolean = default (Boolean);
                break;
            }

            this.type = type;
        }

        /// <summary>
        /// Serializes this object to JSON string.
        /// </summary>
        /// <returns></returns>
        public string ToJson ()
        {
            if (json != null)
                return json;

            StringWriter sw = new StringWriter ();
            JsonWriter writer = new JsonWriter (sw);
            writer.Validate = false;

            WriteJson (this, writer);
            json = sw.ToString ();

            return json;
        }


/*
        public void ToJson (JsonWriter writer)
        {
            bool old_validate = writer.Validate;

            writer.Validate = false;

            WriteJson (this, writer);

            writer.Validate = old_validate;
        }
*/

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            switch (type) {
            case JsonType.Array:
                return "JsonData array";

            case JsonType.Boolean:
                return inst_boolean.ToString ();

            case JsonType.Double:
                return inst_double.ToString ();

            case JsonType.Int:
                return inst_int.ToString ();

            case JsonType.Long:
                return inst_long.ToString ();

            case JsonType.Object:
                return "JsonData object";

            case JsonType.String:
                return inst_string;
            }

            return "Uninitialized JsonData";
        }
    }


    internal class OrderedDictionaryEnumerator : IDictionaryEnumerator
    {
        IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;


        public object Current {
            get { return Entry; }
        }

        public DictionaryEntry Entry {
            get {
                KeyValuePair<string, JsonData> curr = list_enumerator.Current;
                return new DictionaryEntry (curr.Key, curr.Value);
            }
        }

        public object Key {
            get { return list_enumerator.Current.Key; }
        }

        public object Value {
            get { return list_enumerator.Current.Value; }
        }


        public OrderedDictionaryEnumerator (
            IEnumerator<KeyValuePair<string, JsonData>> enumerator)
        {
            list_enumerator = enumerator;
        }


        public bool MoveNext ()
        {
            return list_enumerator.MoveNext ();
        }

        public void Reset ()
        {
            list_enumerator.Reset ();
        }
    }
}
