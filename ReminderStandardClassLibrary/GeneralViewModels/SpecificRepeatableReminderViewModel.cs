using System;
using System.Collections.Generic;
using System.Text;
using ReminderStandardClassLibrary.DataClasses;
using ReminderStandardClassLibrary.Interfaces;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public class SpecificRepeatableReminderViewModel : RepeaterReminderViewModel<IReminderSpecificRepeatableData>
	{
		public SpecificRepeatableReminderViewModel(IReminderSpecificRepeatableData _Behave) : base(_Behave)
		{
		}

		protected override VariableCycleClass GetNextReminderDuration()
		{
			VariableCycleClass ThisV = new VariableCycleClass()
			{
				Title = CustomReminderBehavior.ReminderTitle,
				Message = $"{CustomReminderBehavior.ReminderMessage} cyle {RepeatData.UpTo} of {HowManyCycles()}",
				HowLong = CustomReminderBehavior.HowLongBetweenCycles.ToString(),
				TimeFormat = CustomReminderBehavior.TimeFormat
			};
			return ThisV;
		}

		protected override int HowManyCycles()
		{
			return CustomReminderBehavior.HowManyCycles;
		}
	}
}
