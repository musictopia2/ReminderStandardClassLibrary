using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using ReminderStandardClassLibrary.DataClasses;
using ReminderStandardClassLibrary.Interfaces;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public class VariableRepeatableReminderViewModel : RepeaterReminderViewModel //: RepeaterReminderViewModel<IReminderVariableData>
	{
		private readonly IReminderVariableData _customReminderBehavior;
		public VariableRepeatableReminderViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI, IReminderBasicData tempremind) : base(tempFocus, tempUI, tempremind)
		{
			_customReminderBehavior = (IReminderVariableData)tempremind;
		}
		protected override VariableCycleClass GetNextReminderDuration()
		{
			var thisList = _customReminderBehavior.GetVariableList(); //it has to do at runtime.
																	  //because there can be an implementation that would vary depending on what is selected.
			return (thisList[RepeatData.UpTo - 1]); //because its 0 based
		}
		protected override int HowManyCycles()
		{
			return _customReminderBehavior.HowManyCycles;
			//return VariableList.Count; //this time, it won't even use the interface.   some need it though.  not sure how i could have better done that part.
		}
	}
}