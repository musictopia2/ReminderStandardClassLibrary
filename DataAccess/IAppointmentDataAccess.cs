using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.DataAccess
{
    public interface IAppointmentDataAccess : IProcessedReminder
    {
        Task<ReminderModel?> GetNextAppointmentReminderAsync();
    }
}