using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.DataAccess
{
    public interface ITemporaryReoccuringDataAccess : IProcessedReminder
    {
        Task<ReminderModel?> GetNextTemporaryReoccuringReminderAsync();
    }
}