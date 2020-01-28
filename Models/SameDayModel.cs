using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using System;
namespace ReminderStandardClassLibrary.Models
{
    public class SameDayModel : ISimpleDapperEntity
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public DateTime ReminderDate { get; set; } //this is the date/time of the reminder.

        public override string ToString()
        {
            return $"{Title} on {ReminderDate}";
        }
    }
}