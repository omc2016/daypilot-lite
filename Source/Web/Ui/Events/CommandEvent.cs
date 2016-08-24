using System;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Command event delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CommandEventHandler(object sender, CommandEventArgs e);

    /// <summary>
    /// Command event arguments.
    /// </summary>
    public class CommandEventArgs : DayPilotEventArgs
    {
        private string command;

        /// <summary>
        /// Gets the command parameter of the client-side .commandCallBack() call.
        /// </summary>
        public string Command
        {
            get { return command; }
        }

        internal CommandEventArgs(JsonData parameters, JsonData data)
        {
            this.Data = data;
            this.command = (string)parameters["command"];
        }

    }
}
