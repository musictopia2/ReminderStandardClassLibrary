using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReminderStandardClassLibrary.Interfaces
{
	public interface IPopUp
	{
		Task LoadAsync(string title, string message);
		void PlaySound(int howOftenToRepeat);
		event Func<Task> ClosedAsync;
		event Func<TimeSpan, Task> SnoozedAsync;
	}
}
