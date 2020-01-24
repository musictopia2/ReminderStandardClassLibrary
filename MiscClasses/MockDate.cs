using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using ReminderStandardClassLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace ReminderStandardClassLibrary.MiscClasses
{
    public class MockDate : ICurrentDate
    {

        private readonly Timer _timer;
        //private DateTime _firstTime;
        private DateTime _currentTime;
        private readonly IEventAggregator _aggregator;

        public MockDate(IEventAggregator aggregator, DateTime startDate)
        {
            //DateTime firstTime = new DateTime(2020, 1, 26, 8, 12, 56);
            _currentTime = startDate;
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            //you can make it autoreset which means it repeats.
            //otherwise, 
            _aggregator = aggregator;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _currentTime = _currentTime.AddSeconds(1);
            Execute.OnUIThread(() =>
            {
                _aggregator.Publish(_currentTime);
            });
        }

        Task<DateTime> ICurrentDate.GetCurrentDateAsync()
        {
            return Task.FromResult(_currentTime);
        }
    }
}
