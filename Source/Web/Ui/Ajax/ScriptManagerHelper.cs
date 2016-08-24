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
using System.Reflection;
using System.Web.UI;

namespace DayPilot.Web.Ui.Ajax
{
    internal static class ScriptManagerHelper
    {
        private static readonly object ReflectionLock = new object();
        private static bool MethodsInitialized;

        private static MethodInfo RegisterStartupScriptMethod;
        private static MethodInfo RegisterClientScriptIncludeMethod;

        private static void InitializeReflection()
        {
            if (!MethodsInitialized)
            {
                lock (ReflectionLock)
                {
                    if (!MethodsInitialized)
                    {

                        Type scriptManagerType = null;
                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            switch (assembly.FullName)
                            {
                                case "System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35":
                                    scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
                                    break;
                                case "System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35":
                                    scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
                                    break;
                                case "System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35":
                                    scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
                                    break;
                            }
                            if (scriptManagerType != null) break;
                        }

                        if (scriptManagerType != null)
                        {
                            RegisterStartupScriptMethod = scriptManagerType.GetMethod("RegisterStartupScript", new Type[] { typeof(Page), typeof(Type), typeof(String), typeof(String), typeof(Boolean) });
                            RegisterClientScriptIncludeMethod = scriptManagerType.GetMethod("RegisterClientScriptInclude", new Type[] { typeof(Page), typeof(Type), typeof(String), typeof(String) });
                        }

                        MethodsInitialized = true;
                    }
                }
            }
        }
        
        /*
        private static void InitializeReflectionOld()
        {
            if (!MethodsInitialized)
            {
                lock (ReflectionLock)
                {
                    if (!MethodsInitialized)
                    {
                        // try the original assembly
                        Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);

                        // if not found, try the .NET 3.5 assembly
                        if (scriptManagerType == null)
                        {
                            scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
                        }

                        if (scriptManagerType != null)
                        {
                            RegisterStartupScriptMethod = scriptManagerType.GetMethod("RegisterStartupScript", new Type[] {typeof(Control), typeof(Type), typeof(String), typeof(String), typeof(Boolean)});
                            //IsInAsyncPostBackMethod = scriptManagerType.GetMethod("IsInAsyncPostBack", new Type[] {});
                        }

                        MethodsInitialized = true;
                    }
                }
            }
        }
         */ 

        public static bool IsMicrosoftAjaxAvailable()
        {
            InitializeReflection();

            return (RegisterStartupScriptMethod != null);
        }

        public static void RegisterClientScriptInclude(Control control, Type type, string key, string url)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (control.Page == null)
            {
                throw new ArgumentException("The control must be on a page.", "control");
            }

            InitializeReflection();

            if (RegisterClientScriptIncludeMethod != null)
            {
                // ASP.NET AJAX exists, so we use the ScriptManager
                RegisterClientScriptIncludeMethod.Invoke(null, new object[] { control.Page, type, key, url });
            }
            else
            {
                // No ASP.NET AJAX, so we just call to the ASP.NET 2.0 method
                control.Page.ClientScript.RegisterClientScriptInclude(type, key, url);
            }
        }

        public static void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (control.Page == null)
            {
                throw new ArgumentException("The control must be on a page.", "control");
            }

            InitializeReflection();

            if (RegisterStartupScriptMethod != null)
            {
                // ASP.NET AJAX exists, so we use the ScriptManager
                RegisterStartupScriptMethod.Invoke(null, new object[] { control.Page, type, key, script, addScriptTags });
            }
            else
            {
                // No ASP.NET AJAX, so we just call to the ASP.NET 2.0 method
                control.Page.ClientScript.RegisterStartupScript(type, key, script, addScriptTags);
            }
        }
    }
}
