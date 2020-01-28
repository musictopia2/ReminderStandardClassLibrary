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

        public int HowMany { get; set; } //this is how many days or how many hours.

        public string Message { get; set; } = ""; //this is what needs to be done.

        public override string ToString()
        {
            if (TimeMode == EnumTimeFormat.Days)
            {
                return $"{Message} starting {StartDate} ending {EndDate} every {HowMany} days";
            }
            if (TimeMode == EnumTimeFormat.Hours)
            {
                return $"{Message} starting {StartDate} ending {EndDate} every {HowMany} hours";
            }
            return base.ToString(); //can later see what the message should be in other cases.
        }


    }
}
