using System;

namespace ReminderStandardClassLibrary.Models
{
    public class ReminderModel
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime NextDate { get; set; } //this is the next date its needed whatever it is.
    }
}