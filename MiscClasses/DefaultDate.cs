using ReminderStandardClassLibrary.Interfaces;
using System;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.MiscClasses
{
    public class DefaultDate : ICurrentDate
    {
        Task<DateTime> ICurrentDate.GetCurrentDateAsync()
        {
            return Task.FromResult(DateTime.Now);
        }
    }
}