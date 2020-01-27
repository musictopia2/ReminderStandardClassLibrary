using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderStandardClassLibrary.Models
{
    public class SameDayModel : ISimpleDapperEntity
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public DateTime ReminderDate { get; set; } //this is the date/time of the reminder.
    }
}
