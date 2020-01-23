using static CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses.CustomTimeAttribute;
namespace ReminderStandardClassLibrary.Interfaces
{
	public interface IReminderSpecificRepeatableData : IReminderBasicData //refilling water will use this
	{
		int HowLongBetweenCycles { get; }
		string ReminderTitle { get; }
		string ReminderMessage { get; }
		EnumTimeFormat TimeFormat { get; } //this means i can have specialized behaviors here too.
	}
}