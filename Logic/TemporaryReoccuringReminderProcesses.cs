using CommonBasicStandardLibraries.Messenging;
using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
//i think this is the most common things i like to do
namespace ReminderStandardClassLibrary.Logic
{
    /// <summary>
    /// this is for cases where something is reoccuring but only temporarily.
    /// there can be several of them.
    /// </summary>
    public class TemporaryReoccuringReminderProcesses : BasicSubReminderProcesses
    {
        private readonly ITemporaryReoccuringDataAccess _data;

        public TemporaryReoccuringReminderProcesses(ITemporaryReoccuringDataAccess data, ISnoozeDataAccess snoozeData, IEventAggregator aggregator) : base(data, snoozeData, aggregator)
        {
            _data = data;
        }

        public override Task<ReminderModel?> GetNextReminderAsync()
        {
            return _data.GetNextTemporaryReoccuringReminderAsync();
        }
    }
}
