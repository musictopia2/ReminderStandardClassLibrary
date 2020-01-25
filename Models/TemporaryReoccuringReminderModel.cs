using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using System;
using static CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses.CustomTimeAttribute;
namespace ReminderStandardClassLibrary.Models
{
    public class TemporaryReoccuringReminderModel : ISimpleDapperEntity
    {
        public int ID { get; set; }
        public EnumTimeFormat TimeMode { get; set; } //will be hours or days but this allowed extra flexibility.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //i think its okay to delete if its past time. and already fulfilled all.
        //for now, this may be okay.
        //can test the mock version.

        public int HowMany { get; set; } //this is how many days or how many hours.

        public string Message { get; set; } = ""; //this is what needs to be done.





    }
}
