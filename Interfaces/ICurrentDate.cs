using System;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Interfaces
{
    public interface ICurrentDate
    {
        //not sure if async is needed or not.

        Task<DateTime> GetCurrentDateAsync();
    }
}
