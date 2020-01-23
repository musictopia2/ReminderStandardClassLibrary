using static CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses.CustomTimeAttribute;
namespace ReminderStandardClassLibrary.Models
{
	public class VariableCycleModel
	{
		public EnumTimeFormat TimeFormat { get; set; }
		public string HowLong { get; set; } = "";
		public string Title { get; set; } = "";
		public string Message { get; set; } = "";
	}
}