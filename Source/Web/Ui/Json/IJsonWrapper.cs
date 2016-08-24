/*
 * Thanks to http://litjson.sourceforge.net/
 */

using System.Collections;
using System.Collections.Specialized;


namespace DayPilot.Json
{
    /// <summary>
    /// Json types. Thanks to http://litjson.sourceforge.net/.
    /// </summary>
    public enum JsonType
    {
        /// <summary>
        /// No type.
        /// </summary>
        None,

        /// <summary>
        /// Object type.
        /// </summary>
        Object,

        /// <summary>
        /// Array type.
        /// </summary>
        Array,

        /// <summary>
        /// Strign type.
        /// </summary>
        String,

        /// <summary>
        /// Integer type.
        /// </summary>
        Int,

        /// <summary>
        /// Long type.
        /// </summary>
        Long,

        /// <summary>
        /// Double type.
        /// </summary>
        Double,

        /// <summary>
        /// Boolean type.
        /// </summary>
        Boolean
    }

    internal interface IJsonWrapper : IList, IOrderedDictionary
    {
        bool IsArray   { get; }
        bool IsBoolean { get; }
        bool IsDouble  { get; }
        bool IsInt     { get; }
        bool IsLong    { get; }
        bool IsObject  { get; }
        bool IsString  { get; }

        bool     GetBoolean ();
        double   GetDouble ();
        int      GetInt ();
        JsonType GetJsonType ();
        long     GetLong ();
        string   GetString ();

        void SetBoolean  (bool val);
        void SetDouble   (double val);
        void SetInt      (int val);
        void SetJsonType (JsonType type);
        void SetLong     (long val);
        void SetString   (string val);

        /*
        string ToJson ();
        void   ToJson (JsonWriter writer);
         */ 
    }
}
