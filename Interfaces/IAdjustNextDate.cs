using System;
using System.Threading.Tasks;
namespace AndyCristinaBibleStudyCPLibrary.DataAccess
{
    /// <summary>
    /// this is when i am adjusting when something can be done again.
    /// at first, will be the minutes part of it.
    /// however, will eventually have another method that would show the time in text form like 9:30 AM, etc.
    /// </summary>
    public interface IAdjustNextDate
    {
        /// <summary>
        /// can be plus or minus
        /// plus means add time until next event
        /// minus means to subtract time until next event.
        /// if there is a snooze, the snooze has to be increased.
        /// 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        Task AdjustMinutesAsync(int minutes);


        Task AdjustTimeAsync(DateTime time);
    }
}