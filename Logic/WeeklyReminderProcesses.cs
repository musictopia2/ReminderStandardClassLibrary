using ReminderStandardClassLibrary.DataAccess;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
//i think this is the most common things i like to do
namespace ReminderStandardClassLibrary.Logic
{
    public class WeeklyReminderProcesses : BasicSubReminderProcesses
    {
        private readonly ISimpleWeeklyDataAccess _data;
        public WeeklyReminderProcesses(ISimpleWeeklyDataAccess data)
        {
            _data = data;
        }

        protected override Task<ReminderModel> GetNextReminderAsync()
        {
            return _data.GetNextWeeklyReminderAsync()!;
        }

    }
}
