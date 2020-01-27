using CommonBasicStandardLibraries.Messenging;
using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Logic
{
    public class AppointmentReminderProcesses : BasicSubReminderProcesses
    {
        private readonly IAppointmentDataAccess _data;

        public AppointmentReminderProcesses(IAppointmentDataAccess data, ISnoozeDataAccess snoozeData, IEventAggregator aggregator) : base(data, snoozeData, aggregator)
        {
            _data = data;
        }

        public override Task<ReminderModel?> GetNextReminderAsync()
        {
            return _data.GetNextAppointmentReminderAsync();
        }
    }
}
