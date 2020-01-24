namespace ReminderStandardClassLibrary.Interfaces
{
	public interface IReminderPaths
	{
		string GetReminderParentPath(); //will return what is needed for doing weekly meals.  probably use dependency injection for this.
	}
}