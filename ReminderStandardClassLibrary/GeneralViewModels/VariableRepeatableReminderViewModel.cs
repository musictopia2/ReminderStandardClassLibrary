using System;
using System.Collections.Generic;
using System.Text;
using ReminderStandardClassLibrary.DataClasses;
using ReminderStandardClassLibrary.Interfaces;
using CommonBasicStandardLibraries.CollectionClasses;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public class VariableRepeatableReminderViewModel : RepeaterReminderViewModel<IReminderVariableData>
	{

		//private readonly CustomBasicList<VariableCycleClass> VariableList;

		public VariableRepeatableReminderViewModel(IReminderVariableData _Behave) : base(_Behave)
		{
			//VariableList = CustomReminderBehavior.GetVariableList();
		}

		protected override VariableCycleClass GetNextReminderDuration()
		{
			var ThisList = CustomReminderBehavior.GetVariableList(); //it has to do at runtime.
			//because there can be an implementation that would vary depending on what is selected.
			return (ThisList[RepeatData.UpTo - 1]); //because its 0 based
		}

		protected override int HowManyCycles()
		{
			return CustomReminderBehavior.HowManyCycles;
			//return VariableList.Count; //this time, it won't even use the interface.   some need it though.  not sure how i could have better done that part.
		}
	}
}
