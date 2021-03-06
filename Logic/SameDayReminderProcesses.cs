﻿using CommonBasicStandardLibraries.Messenging;
using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Logic
{
    public class SameDayReminderProcesses : BasicSubReminderProcesses
    {
        private readonly ISameDayReminderDataAccess _data;

        public SameDayReminderProcesses(ISameDayReminderDataAccess data, ISnoozeDataAccess snoozeData, IEventAggregator aggregator) : base(data, snoozeData, aggregator)
        {
            _data = data;
        }

        public override Task<ReminderModel?> GetNextReminderAsync()
        {
            return _data.GetNextReminderAsync();
        }
    }
}
