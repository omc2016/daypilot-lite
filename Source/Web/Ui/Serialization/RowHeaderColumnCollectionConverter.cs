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
using System.Collections;
using System.ComponentModel;
using Encoder=DayPilot.Utils.Encoder;

namespace DayPilot.Web.Ui.Serialization
{
    /// <summary>
    /// Internal class for serializing ResourceCollection (ViewState).
    /// </summary>
    public class RowHeaderColumnCollectionConverter : TypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            String state = value as String;
            if (state == null)
            {
                return base.ConvertFrom(context, culture, value);
            }

            if (state == String.Empty)
            {
                return new RowHeaderColumnCollection();
            }

            String[] parts = state.Split('&');

            RowHeaderColumnCollection collection = new RowHeaderColumnCollection();
            foreach (string encRes in parts)
            {
                string[] props = Encoder.UrlDecode(encRes).Split('&');

                RowHeaderColumn r = new RowHeaderColumn();
                r.Title = Encoder.UrlDecode(props[0]);
                r.Width = Convert.ToInt32(Encoder.UrlDecode(props[1]));

                collection.Add(r);
            }

            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentException("destinationType");
            RowHeaderColumnCollection collection = value as RowHeaderColumnCollection;

            if (collection == null)
            {
                return base.ConvertTo(context, culture, value, destinationType);
                
            }

            if (collection.designMode)
            {
                return "(Collection)";
            }

            ArrayList al = new ArrayList();

            foreach (RowHeaderColumn r in collection)
            {
                ArrayList properties = new ArrayList();
                properties.Add(r.Title);
                properties.Add(r.Width);

                al.Add(Encoder.UrlEncode(properties));
            }

            return Encoder.UrlEncode(al);
        }
    }
}
