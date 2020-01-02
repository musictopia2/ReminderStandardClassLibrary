namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public enum EnumReminderStatus
	{
		None, WaitingForReminder, WaitingForUser
	}
	//waitingforuser means waiting for multiple step.
	//the waitingforreminder is waiting for reminder or waiting for popups to close
	//none means waiting for user action.
}
