using CommonBasicStandardLibraries.Exceptions;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Logic
{
    public abstract class BasicSubReminderProcesses : ISubReminder
    {
        public BasicSubReminderProcesses()
        {
            MainReminderProcesses.AddReminder(this);
        }
        public bool ShowSounds { get; set; } = true; //default is true.
        public int HowOftenToRepeat { get; set; } = 10;
        public DateTime? NextDate { get; private set; }
        protected bool Snoozing = false;
        private ReminderModel? _nextReminder;
        public virtual Task CloseReminderAsync(DateTime currentDate)
        {
            Snoozing = false;
            //for weekly reminders, does not care.
            //however, can be overrided if necessary.
            return Task.CompletedTask;
        }

        public Task SnoozeAsync(TimeSpan time, DateTime currentDate)
        {
            Snoozing = true;
            NextDate = currentDate.Add(time);
            return Task.CompletedTask;
        }
        /// <summary>
        /// this will run the custom processes to see what reminder is next.
        /// 
        /// </summary>
        /// <returns></returns>
        //protected abstract Task CheckListsAsync();


        protected abstract Task<ReminderModel?> GetNextReminderAsync();

        //this means if i need something else, can do.
        //hopefully i don't regret the part for next reminder.
        //if so, rethinking may be required.
        //or could allow it to be protected so other processes can do something else if needed.
        //not sure yet though.



        public virtual async Task<(bool needsReminder, string title, string message)> GetReminderInfoAsync(DateTime currentDate)
        {
            if (Snoozing == false)
            {
                //_nextReminder = null;
                _nextReminder = await GetNextReminderAsync();
                if (_nextReminder == null)
                {
                    NextDate = null;
                }
                else
                {
                    NextDate = _nextReminder.NextDate;
                }
                if (NextDate == null)
                {
                    return (false, "", "");
                }
            }
            else
            {
                if (NextDate == null)
                {
                    throw new BasicBlankException("Next date cannot be null when snoozing.  Rethink");
                }
            }
            if (_nextReminder !=null && currentDate >= NextDate)
            {
                return (true, _nextReminder.Title, _nextReminder.Message);
            }
            return (false, "", "");

        }

        public virtual Task ProcessedReminderAsync()
        {
            return Task.CompletedTask;
        }
    }
}
