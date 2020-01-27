using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Logic
{
    public class AppointmentReminderProcesses : BasicSubReminderProcesses
    {
        private readonly IAppointmentDataAccess _data;

        public AppointmentReminderProcesses(IAppointmentDataAccess data, ISnoozeDataAccess snoozeData) : base(data, snoozeData)
        {
            _data = data;
        }

        public override Task<ReminderModel?> GetNextReminderAsync()
        {
            return _data.GetNextAppointmentReminderAsync();
        }
    }
}
