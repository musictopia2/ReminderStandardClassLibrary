using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.DataAccess
{
    public interface ISimpleWeeklyDataAccess : IProcessedReminder
    {


        //looks like i need the entire weekly list.

        //i think this could be good.
        Task<ReminderModel?> GetNextWeeklyReminderAsync();


        //public Task<CustomBasicList<WeeklyReminderClass>> GetWeeklyListAsync();
        //public Task AddToWeeklyListAsync(WeeklyReminderClass weekly); //so if i wanted to add something to the list, i can.

        //public Task UpdateWeeklyItemAsync(WeeklyReminderClass weekly);

        //public Task DeleteWeeklyItemAsync(WeeklyReminderClass weekly);

        //public Task AddToWeeklyListAsync(CustomBasicList<WeeklyReminderClass> list);


        //public Task SaveWeeklyListAsync(CustomBasicList<WeeklyReminderClass> reminders);
        //for now, these 3 are enough.  if i need more, will rethink.

    }
}