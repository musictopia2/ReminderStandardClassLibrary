using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.DataAccess
{
    /// <summary>
    /// this is a case where you just want a one time reminder but not an appointment.
    /// </summary>
    public interface ISameDayReminderDataAccess : IProcessedReminder
    {
        Task<ReminderModel?> GetNextReminderAsync();
        Task AddNewSameDayReminderAsync(SameDayModel model);
        Task<CustomBasicList<SameDayModel>> GetSameDayReminderListAsync();
        Task DeleteSameDayReminderAsync(SameDayModel model);
        Task UpdateSameDayReminderAsync(SameDayModel model);
    }
}