using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using ReminderStandardClassLibrary.DataClasses;
using ReminderStandardClassLibrary.Interfaces;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public class SpecificRepeatableReminderViewModel : RepeaterReminderViewModel //: RepeaterReminderViewModel<IReminderSpecificRepeatableData>
	{

		//public SpecificRepeatableReminderViewModel(IFocusOnFirst TempFocus, IReminderBasicData TempRemind) : base(TempFocus, TempRemind)
		//{
		//    CustomReminderBehavior = (IReminderSpecificRepeatableData) TempRemind;
		//}

		private readonly IReminderSpecificRepeatableData _customReminderBehavior; //not sure if i need protected or not.

		public SpecificRepeatableReminderViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI, IReminderBasicData tempremind) : base(tempFocus, tempUI, tempremind)
		{
			_customReminderBehavior = (IReminderSpecificRepeatableData)tempremind;
		}

		//public SpecificRepeatableReminderViewModel(IReminderSpecificRepeatableData _Behave) : base(_Behave)
		//{
		//}

		protected override VariableCycleClass GetNextReminderDuration()
		{
			VariableCycleClass ThisV = new VariableCycleClass()
			{
				Title = _customReminderBehavior.ReminderTitle,
				Message = $"{_customReminderBehavior.ReminderMessage} cyle {RepeatData.UpTo} of {HowManyCycles()}",
				HowLong = _customReminderBehavior.HowLongBetweenCycles.ToString(),
				TimeFormat = _customReminderBehavior.TimeFormat
			};
			return ThisV;
		}

		protected override int HowManyCycles()
		{
			return _customReminderBehavior.HowManyCycles;
		}
	}
}
