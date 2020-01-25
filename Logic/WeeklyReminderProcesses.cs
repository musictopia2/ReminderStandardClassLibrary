﻿using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
//i think this is the most common things i like to do
namespace ReminderStandardClassLibrary.Logic
{
    public class WeeklyReminderProcesses : BasicSubReminderProcesses
    {
        private readonly ISimpleWeeklyDataAccess _data;

        public WeeklyReminderProcesses(ISimpleWeeklyDataAccess data, ISnoozeDataAccess snoozeData) : base(data, snoozeData)
        {
            _data = data;
        }

        //public WeeklyReminderProcesses(ISimpleWeeklyDataAccess data) : base(data)
        //{
        //    _data = data;
        //}



        protected override Task<ReminderModel?> GetNextReminderAsync()
        {
            //return Task.FromResult<ReminderModel?>(null);
            return _data.GetNextWeeklyReminderAsync()!;
        }

        //public override Task ProcessedReminderAsync()
        //{
        //    return _data.ProcessedReminderAsync();
        //}

    }
}