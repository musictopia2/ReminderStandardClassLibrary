using ReminderStandardClassLibrary.Models;
using System;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.DataAccess
{
    public interface ISnoozeDataAccess
    {
        Task<ReminderModel?> GetSnoozedReminderAsync(string key);
        Task SaveSnoozeAsync(string key, ReminderModel model, DateTime date);

        Task DeleteSnoozeAsync(string key); //there can be only one of them period.

        Task UpdateSnooozeAsync(string key, DateTime date); //they should know what to do.
    }
}
