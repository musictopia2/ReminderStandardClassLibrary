using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderStandardClassLibrary.MiscClasses
{
    public static class GlobalHelpers
    {
        public static EnumPopupMode PopUpMode { get; set; } = EnumPopupMode.Minutes; //you can set differently so the popups would show in seconds, etc for testing.
    }
}