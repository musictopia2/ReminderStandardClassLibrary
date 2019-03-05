using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.DataClasses;
using static CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses.CustomTimeAttribute;

namespace ReminderStandardClassLibrary.Interfaces
{
	public interface IReminderWindow : IFocusOnFirst
	{
		void NewContentForCombo();
		void FocusOnCombo();
	}

	public interface IView
	{
		IPopUp GeneratePopUpReminder();
	}

	public interface IPopUp
	{
		void Load(string Title, string Message);

		bool IsLoaded();

		void PlaySound(int HowOftenToRepeat);

		TimeSpan? HowLongToDelay();
		void ClosePopUp();
		void StopPlay();
	}

	//looks like another interface should be done here.
	public interface IReminderPaths
	{
		string GetReminderParentPath(); //will return what is needed for doing weekly meals.  probably use dependency injection for this.
	}

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

	public interface IReminderSpecificRepeatableData : IReminderBasicData //refilling water will use this
	{
		int HowLongBetweenCycles { get; }
		string ReminderTitle { get; }
		string ReminderMessage { get; }
		EnumTimeFormat TimeFormat { get; } //this means i can have specialized behaviors here too.

	}

	public interface IReminderVariableData: IReminderBasicData
	{
		CustomBasicList<VariableCycleClass> GetVariableList();
	}

	//well worry later about the variable ones.
	//figure out a pattern so it can be through interfaces as well.
	//the repeatable class will be abstract but the other 2 will not be abstract.
	//but will require a constructor to put in an interface (was going to use dependency injection)
	//but that would keep from having the option of having as one larger app.

}
