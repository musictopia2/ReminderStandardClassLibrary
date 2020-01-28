﻿using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Logic;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace ReminderStandardClassLibrary.MiscClasses
{
    public class MockDate : ICurrentDate
    {

        private readonly Timer _timer;
        //private DateTime _firstTime;
        private static DateTime _currentTime; //since its mock, then can go ahead and make static.  if not using the class, then no problem.
        private readonly IEventAggregator _aggregator;

        public static DateTime FutureDate;

        public MockDate(IEventAggregator aggregator, DateTime startDate)
        {
            //DateTime firstTime = new DateTime(2020, 1, 26, 8, 12, 56);
            _currentTime = startDate;
            FutureDate = _currentTime; //i think this is fine.
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            //you can make it autoreset which means it repeats.
            //otherwise, 
            _aggregator = aggregator;
            _timer.Start();
        }

        public static void ChangeDate(DateTime date)
        {
            _currentTime = date;
            FutureDate = _currentTime; //set the future date so you can change dates later again if need to.
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
