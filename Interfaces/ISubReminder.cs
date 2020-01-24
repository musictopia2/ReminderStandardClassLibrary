using System;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Interfaces
{
    /// <summary>
    /// this is a sub reminder.  can be for bible, etc.
    /// however, can't be a view model because data may be shown or not visible.
    /// but the behind the scenes must still keep working.
    /// view model is ui specific only.
    /// </summary>
    public interface ISubReminder : IProcessedReminder
    {
        //i propose the parent view model which is always visible would be responsible for figuring out order.

        bool ShowSounds { get; set; }
        int HowOftenToRepeat { get; set; }
        //hopefully the business logic can handle the snooze.
        /// <summary>
        /// this is when you are snoozing.
        /// after this process runs, then all others will continue.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        Task SnoozeAsync(TimeSpan time, DateTime currentDate); //this means you are snoozing.  its up to each individual one to decide how to handle this.

        Task CloseReminderAsync(DateTime currentDate);

        Task<(bool needsReminder, string title, string message)> GetReminderInfoAsync(DateTime currentDate);

        DateTime? NextDate { get; }

        

    }
}
