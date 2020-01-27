using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using System;
namespace ReminderStandardClassLibrary.Models
{
    public class AppointmentModel : ISimpleDapperEntity
    {
        public int ID { get; set; }
        public string Title { get; set; } = ""; //this is what would show up for the reminder
        public string Notes { get; set; } = ""; //this is the notes
        public DateTime AppointmentDate { get; set; } //this is the date of the appointment.
        public string ReminderTime { get; set; } = ""; //decided that time should be separate field.
    }
}