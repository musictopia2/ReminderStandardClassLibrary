namespace ReminderStandardClassLibrary.Interfaces
{
    //looks like another interface should be done here.
    public interface IReminderPaths
	{
		string GetReminderParentPath(); //will return what is needed for doing weekly meals.  probably use dependency injection for this.
	}
}