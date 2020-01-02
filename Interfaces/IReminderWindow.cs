using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
namespace ReminderStandardClassLibrary.Interfaces
{
    public interface IReminderWindow : IFocusOnFirst
	{
		void NewContentForCombo();
		void FocusOnCombo();
	}
}