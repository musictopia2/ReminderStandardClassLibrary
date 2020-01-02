using static CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses.CustomTimeAttribute;
namespace ReminderStandardClassLibrary.DataClasses
{
	public class VariableCycleClass
	{
		public EnumTimeFormat TimeFormat { get; set; }
		public string HowLong { get; set; } = "";
		public string Title { get; set; } = "";
		public string Message { get; set; } = "";
	}
}
