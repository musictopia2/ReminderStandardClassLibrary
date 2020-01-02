namespace ReminderStandardClassLibrary.Interfaces
{
    //decide what other interfaces we need for behaviors.
    //when we get to the repeater, etc will decide at that time.

    public interface IReminderBasicData
	{
		string StartText { get; }
		string ContinueText { get; }
		string Title { get; }
		int HowManyCycles { get; }
		//bool NeedsValidationBeforeStartingReminder(); //so one can specify must do this.
		//looks like no need for this because i am probably required to override for the corn beef anyways.  there is no other way around this.
	}
}
