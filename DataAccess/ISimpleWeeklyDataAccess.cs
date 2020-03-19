using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.DataAccess
{
    public interface ISimpleWeeklyDataAccess : IProcessedReminder
    {
        Task<ReminderModel?> GetNextWeeklyReminderAsync();
        Task AddNewWeeklyReminderAsync(WeeklyReminderModel model);
        Task<CustomBasicList<WeeklyReminderModel>> GetWeeklyReminderListAsync();
        Task DeleteWeeklyReminderAsync(WeeklyReminderModel model);
        Task UpdateWeeklyReminderAsync(WeeklyReminderModel model);
    }
}