/*
Copyright © 2005 - 2016 Annpoint, s.r.o.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

-------------------------------------------------------------------------

NOTE: Reuse requires the following acknowledgement (see also NOTICE):
This product includes DayPilot (http://www.daypilot.org) developed by Annpoint, s.r.o.
*/

using System;
using System.Globalization;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Json
{
    /// <summary>
    /// Just for parsing short strings (inefficient for large data).
    /// </summary>
    public class SimpleJsonDeserializer
    {
        private int _i = 0;
        private readonly string _input = null;
        //private IDictionary result;
        //private object current;

        /// <summary>
        /// Deserialized a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static JsonData Deserialize(string input)
        {
            SimpleJsonDeserializer sjd = new SimpleJsonDeserializer(input);
            return sjd.GetNextValue();
        }


        private SimpleJsonDeserializer(string input)
        {
            this._input = input;
        }


        private JsonData GetNextValue()
        {
            SkipWhiteSpace();
            ValueType vt = GetNextValueType();

            switch (vt)
            {
                case ValueType.String:
                    return GetString();
                case ValueType.Number:
                    return GetNumber();
                case ValueType.Object:
                    return GetObject();
                case ValueType.Array:
                    return GetArray();
                case ValueType.True:
                    SkipToken();
                    return true;
                case ValueType.False:
                    SkipToken();
                    return false;
                case ValueType.Null:
                    SkipToken();
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SkipToken()
        {
            // assuming that it was detected properly
            while (!Detector.IsValueSeparator(_input[_i]))
            {
                _i++;
            }
        }

        private JsonData GetString()
        {
            bool escaped = false;
            StringBuilder sb = new StringBuilder();

            if (_input[_i] != '"')
                throw new Exception("The string doesn't start with '\"'.");

            _i++;

            while (_input[_i] != '"' || escaped)
            {
                escaped = false;
                if (_input[_i] == '\\')
                    escaped = true;

                sb.Append(_input[_i]);

                _i++;
            }

            _i++;

            return new JsonData(Unescape(sb.ToString()));
        }

        private static char FromUniHex(string hex)
        {
            if (hex.Length != 4)
            {
                throw new ArgumentException("A four-character string expected. Characters supplied: " + hex.Length);
            }
            int unii = Convert.ToInt32(hex, 16);
            char unic = char.ConvertFromUtf32(unii).ToCharArray()[0];
            return unic;
        }

        private static string Unescape(string source)
        {
            bool escaped = false;
            StringBuilder sb = new StringBuilder();
            string uniHex = null;

            foreach (char c in source)
            {
                if (!escaped && c == '\\')
                {
                    escaped = true;
                }
                else
                {
                    if (uniHex != null)
                    {
                        uniHex += c;
                        if (uniHex.Length == 4)
                        {
                            //char uni = Char.Parse("\\u" + uniHex);
                            char uni = FromUniHex(uniHex);
                            sb.Append(uni);
                            uniHex = null;
                        }
                    }
                    else if (escaped)
                    {
                        switch (c)
                        {
                            case '"':
                                sb.Append('\"');
                                break;
                            case '\'':
                                sb.Append('\'');
                                break;
                            case '\\':
                                sb.Append('\\');
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break; 
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'u':
                                uniHex = "";
                                break;
                            case '/':
                                sb.Append('/');
                                break;
                            default:
                                throw new Exception("Unsupported escape sequence : \\" + c);
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    escaped = false;
                }

            }

            return sb.ToString();
        }


        private JsonData GetNumber()
        {
            //bool escaped = false;
            StringBuilder sb = new StringBuilder();

            while (!Detector.IsValueSeparator(_input[_i]) && !Detector.IsWhiteSpace(_input[_i]))
            {
                sb.Append(_input[_i]);
                _i++;
            }

            return new JsonData(Convert.ToDouble(sb.ToString(), CultureInfo.InvariantCulture));
        }

        private JsonData GetObject()
        {
            if (_input[_i] != '{')
                throw new Exception("The object doesn't start with '{'.");

            _i++;

            //Hashtable result = new Hashtable();
            JsonData result = new JsonData();

            do
            {
                SkipWhiteSpace();

                if (_input[_i] == '}')
                {
                    _i++;
                    break;
                }

                ValueType vt = GetNextValueType();
                if (vt != ValueType.String)
                {
                    throw new Exception("Object can't be parsed: Key is not a string.");
                }

                string key = (string) GetString();

                SkipWhiteSpace();

                if (_input[_i] != ':')
                {
                    throw new Exception("Colon expected as key/value separator.");
                }

                _i++;

                SkipWhiteSpace();

                result[key] = GetNextValue();

                SkipWhiteSpace();

            } while (_input[_i++] == ',');


            if (_input[_i - 1] != '}')
            {
                throw new Exception("'}}' expected.");
                //throw new Exception(String.Format("'}}' expected ({0}).", input.Substring(i-1, 30)));
            }

            return result;

        }

        private JsonData GetArray()
        {
            if (_input[_i] != '[')
                throw new Exception("The array doesn't start with '['.");

            _i++;

            JsonData result = new JsonData();


            do
            {
                SkipWhiteSpace();

                if (_input[_i] == ']')
                {
                    _i++;
                    break;
                }

                result.Add(GetNextValue());

                SkipWhiteSpace();

            } while (_input[_i++] == ',');

            if (_input[_i - 1] != ']')
            {
                /*
                int start = 0;
                if (i > 30)
                {
                    start = i - 30;
                }
                string sample = input.Substring(start, i - start);
                char c = input[i - 1];

                throw new Exception(String.Format("']' expected. '{0}' supplied. Here: {1}", c, sample));
                 */
                throw new Exception(String.Format("']' expected."));
 
            }

            return result;

        }


        private ValueType GetNextValueType()
        {
            // assumes whitespaces skipped

            // keep the position at the first character
            char c = _input[_i];
            switch (c)
            {
                case '{':
                    return ValueType.Object;
                case '[':
                    return ValueType.Array;
                case '"':
                    return ValueType.String;
            }

            if (Detector.IsNumber(c))
            {
                return ValueType.Number;
            }

            if (IsToken("true"))
            {
                return ValueType.True;
            }

            if (IsToken("false"))
            {
                return ValueType.False;
            }

            if (IsToken("null"))
            {
                return ValueType.Null;
            }

            throw new Exception(String.Format("Unrecognized value type ({0})", _input.Substring(_i, 30)));

        }

        private void SkipWhiteSpace()
        {
            while (Detector.IsWhiteSpace(_input[_i]))
            {
                _i++;
            }
        }

        private bool IsToken(string token)
        {
            string buffer = String.Empty;

            int tempI = _i;

            // read without moving the position
            while (!Detector.IsValueSeparator(_input[tempI]) && !Detector.IsWhiteSpace(_input[tempI]))
            {
                buffer += _input[tempI];
                tempI++;

                if (tempI >= _input.Length) // not found, too long
                {
                    return false;
                }

                if (tempI - _i > token.Length) // not found, EOF
                {
                    return false;
                }
            }

            return buffer == token;

        }



        internal class Detector
        {
            internal static bool IsWhiteSpace(char c)
            {
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        return true;
                    default:
                        return false;
                }
            }

            internal static bool IsValueSeparator(char c)
            {
                switch (c)
                {
                    case ',':
                    case '}':
                    case ']':
                        return true;
                    default:
                        return false;
                }
            }

            internal static bool IsNumber(char c)
            {
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return true;
                    default:
                        return false;
                }
            }


        }

        internal enum ValueType
        {
            String,
            Number,
            Object,
            Array,
            True,
            False,
            Null
        }

    }
}
