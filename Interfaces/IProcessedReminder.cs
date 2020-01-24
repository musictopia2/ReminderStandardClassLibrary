using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Interfaces
{
    public interface IProcessedReminder
    {
        /// <summary>
        /// i think this is necessary for cases where it was processed.  sometimes nothing is needed.
        /// but reoccuring reminders needs to know its being taken off like weekly reminders so it can do the next weekly reminder.
        /// 
        /// </summary>
        /// <returns></returns>
        Task ProcessedReminderAsync();
    }
}
