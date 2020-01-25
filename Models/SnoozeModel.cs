using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
namespace ReminderStandardClassLibrary.Models
{
    public class SnoozeModel : ISimpleDapperEntity
    {
        //will be string because has to be translated back.
        public int ID { get; set; }
        public string Key { get; set; } = "";
        public string Reminder { get; set; } = "";
    }
}