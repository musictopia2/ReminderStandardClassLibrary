using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
namespace ReminderStandardClassLibrary.Models
{
    public class SnoozeModel : ISimpleDapperEntity
    {
        public int ID { get; set; }
        public string Key { get; set; } = "";
        public string Reminder { get; set; } = "";
    }
}