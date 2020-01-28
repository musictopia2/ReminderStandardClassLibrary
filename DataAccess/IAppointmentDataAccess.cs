using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.DataAccess
{
    public interface IAppointmentDataAccess : IProcessedReminder
    {
        Task<ReminderModel?> GetNextAppointmentReminderAsync();
        Task AddNewAppointmentAsync(AppointmentModel model);
        Task<CustomBasicList<AppointmentModel>> GetAppointmentListAsync();
        Task DeleteAppointmentAsync(AppointmentModel model);
        Task UpdateWeeklyReminderAsync(AppointmentModel model);

    }
}