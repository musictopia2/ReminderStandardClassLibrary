using System;
namespace ReminderStandardClassLibrary.Interfaces
{

	public interface IPopUp
	{
		void Load(string title, string message);
		bool IsLoaded();
		void PlaySound(int howOftenToRepeat);
		TimeSpan? HowLongToDelay();
		void ClosePopUp();
		void StopPlay();
	}
	//well worry later about the variable ones.
	//figure out a pattern so it can be through interfaces as well.
	//the repeatable class will be abstract but the other 2 will not be abstract.
	//but will require a constructor to put in an interface (was going to use dependency injection)
	//but that would keep from having the option of having as one larger app.
}