using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.Logic
{
    public abstract class BasicSubReminderProcesses : ISubReminder, IAdjustNextDate
    {
        private bool _started = false;
        public BasicSubReminderProcesses(IProcessedReminder processed, ISnoozeDataAccess snoozeData, IEventAggregator aggregator)
        {
            _processed = processed;
            _snoozeData = snoozeData;
            _aggregator = aggregator;
            InitAsync();
            //FinishInitAsync().Wait();
        }
        private async void InitAsync()
        {

            MainReminderProcesses.AddReminder(this);
            _nextReminder = await _snoozeData.GetSnoozedReminderAsync(ToString()); //can be null if there was nothing
            if (_nextReminder != null)
            {
                Snoozing = true;
                NextDate = _nextReminder.NextDate; //i think.
            }
            _started = true;
            //at this point needs to refresh.

        }
        public BasicSubReminderProcesses(ISnoozeDataAccess snoozeData, IEventAggregator aggregator)
        {
            _snoozeData = snoozeData;
            _aggregator = aggregator;
            InitAsync();
        }

        public bool ShowSounds { get; set; } = true; //default is true.
        public int HowOftenToRepeat { get; set; } = 10;
        public DateTime? NextDate { get; private set; }
        protected bool Snoozing;
        private ReminderModel? _nextReminder;
        private readonly IProcessedReminder? _processed;
        private readonly ISnoozeDataAccess _snoozeData;
        private readonly IEventAggregator _aggregator;

        protected Task RefreshAsync()
        {
            return _aggregator.PublishAsync(this, ToString()); //hopefully this will work.
        }

        public virtual async Task CloseReminderAsync(DateTime currentDate)
        {
            if (Snoozing)
            {
                await _snoozeData.DeleteSnoozeAsync(ToString()); //i like the tostring being the key.
            }
            await RefreshAsync();
            Snoozing = false;
            //for weekly reminders, does not care.
            //however, can be overrided if necessary.
        }


        public Task SnoozeAsync(TimeSpan time, DateTime currentDate)
        {

            NextDate = currentDate.Add(time);
            if (_nextReminder == null)
            {
                throw new BasicBlankException("There was no next reminder to even save.  Rethink");
            }
            if (Snoozing)
            {
                return _snoozeData.UpdateSnooozeAsync(ToString(), NextDate.Value);
            }
            else
            {
                Snoozing = true;

                return _snoozeData.SaveSnoozeAsync(ToString(), _nextReminder, NextDate.Value);

            }
            //if (Snoozing)
            //{

            //    await _snoozeData.DeleteSnoozeAsync(ToString()); //because brand new one.
            //}
        }
        /// <summary>
        /// this will run the custom processes to see what reminder is next.
        /// 
        /// </summary>
        /// <returns></returns>
        //protected abstract Task CheckListsAsync();


        //decided to make it public so any of the individual screens can know when the next reminder is.  if none is found, then can act accordingly as well.


        public abstract Task<ReminderModel?> GetNextReminderAsync();

        //this means if i need something else, can do.
        //hopefully i don't regret the part for next reminder.
        //if so, rethinking may be required.
        //or could allow it to be protected so other processes can do something else if needed.
        //not sure yet though.



        public virtual async Task<(bool needsReminder, string title, string message)> GetReminderInfoAsync(DateTime currentDate)
        {
            if (_started == false)
            {
                do
                {
                    if (_started)
                    {
                        break;
                    }
                    await Task.Delay(10);
                } while (true);
                //return (false, "", ""); //because not finished yet.
            }
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
            if (_nextReminder != null && currentDate >= NextDate)
            {
                return (true, _nextReminder.Title, _nextReminder.Message);
            }
            return (false, "", "");

        }
        //i see lots of repeating.
        public virtual Task ProcessedReminderAsync()
        {
            if (_processed == null)
            {
                return Task.CompletedTask;
            }
            return _processed.ProcessedReminderAsync();
        }

        protected async Task AdjustMinutesAsync(int minutes)
        {
            if (Snoozing)
            {
                //await _snoozeData.DeleteSnoozeAsync(ToString()); //i think its best to just delete the previous one if any.
                //i think just add to the snooze.
                if (NextDate == null)
                {
                    throw new BasicBlankException("Can't adjust minutes for snooze because next date is null.  Rethink");
                }
                NextDate = NextDate.Value.AddMinutes(minutes);
                await _snoozeData.UpdateSnooozeAsync(ToString(), NextDate.Value);
                MainReminderProcesses.Refresh();
                return;
            }
            _nextReminder = await GetNextReminderAsync();
            //must check for next one first.
            if (_nextReminder == null)
            {
                throw new BasicBlankException("Can't snooze minutes because no reminder.  Rethink");
            }
            Snoozing = true;
            NextDate = _nextReminder.NextDate.AddMinutes(minutes);
            await _snoozeData.SaveSnoozeAsync(ToString(), _nextReminder, NextDate.Value);
            MainReminderProcesses.Refresh();
        }

        Task IAdjustNextDate.AdjustMinutesAsync(int minutes)
        {
            return AdjustMinutesAsync(minutes);
        }

        protected async Task AdjustTimeAsync(DateTime time)
        {
            DateTime tempDate;
            if (Snoozing)
            {
                //await _snoozeData.DeleteSnoozeAsync(ToString()); //i think its best to just delete the previous one if any.
                //i think just add to the snooze.
                if (NextDate == null)
                {
                    throw new BasicBlankException("Can't adjust minutes for snooze because next date is null.  Rethink");
                }
                tempDate = NextDate.Value;
                //NextDate = NextDate.Value.AddMinutes(minutes);
                NextDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hour, time.Minute, 0);
                await _snoozeData.UpdateSnooozeAsync(ToString(), NextDate.Value);
                MainReminderProcesses.Refresh();
                return;
            }
            _nextReminder = await GetNextReminderAsync();
            //must check for next one first.
            if (_nextReminder == null)
            {
                throw new BasicBlankException("Can't snooze minutes because no reminder.  Rethink");
            }
            Snoozing = true;
            NextDate = _nextReminder.NextDate;
            tempDate = NextDate.Value;
            NextDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hour, time.Minute, 0);
            await _snoozeData.SaveSnoozeAsync(ToString(), _nextReminder, NextDate.Value);
            MainReminderProcesses.Refresh();
        }

        Task IAdjustNextDate.AdjustTimeAsync(DateTime time)
        {
            return AdjustTimeAsync(time);
        }
    }
}
