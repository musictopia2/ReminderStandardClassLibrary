using CommonBasicStandardLibraries.Messenging;
using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
namespace ReminderStandardClassLibrary.Logic
{
    public class WeeklyReminderProcesses : BasicSubReminderProcesses
    {
        private readonly ISimpleWeeklyDataAccess _data;

        public WeeklyReminderProcesses(ISimpleWeeklyDataAccess data, ISnoozeDataAccess snoozeData, IEventAggregator aggregator) : base(data, snoozeData, aggregator)
        {
            _data = data;
        }




        public override Task<ReminderModel?> GetNextReminderAsync()
        {
            return _data.GetNextWeeklyReminderAsync()!;
        }

    }
}