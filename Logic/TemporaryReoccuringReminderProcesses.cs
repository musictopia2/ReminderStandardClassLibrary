using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
using ReminderStandardClassLibrary.Models;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.DataAccess;
//i think this is the most common things i like to do
namespace ReminderStandardClassLibrary.Logic
{
    /// <summary>
    /// this is for cases where something is reoccuring but only temporarily.
    /// there can be several of them.
    /// </summary>
    public class TemporaryReoccuringReminderProcesses : BasicSubReminderProcesses
    {

        public TemporaryReoccuringReminderProcesses(IProcessedReminder processed, ISnoozeDataAccess snoozeData) : base(processed, snoozeData)
        {
        }

        protected override Task<ReminderModel?> GetNextReminderAsync()
        {
            throw new NotImplementedException();
        }
    }
}
