using System;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.DataClasses;
using static CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses.CustomTimeAttribute;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
using System.Threading;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.ListsExtensions;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public abstract  class RepeaterReminderViewModel<T> : BaseReminderViewModel where T:IReminderBasicData
	{
		protected abstract int HowManyCycles();
		protected BaseReminderClass RepeatData = new BaseReminderClass();

		public Command StartReminderCommand { get; set; }
		public Command ContinueReminderCommand { get; set; }

		private string _NextDisplayDate;

		protected T CustomReminderBehavior;


		public RepeaterReminderViewModel(T _Behave)
		{	CustomReminderBehavior = _Behave;
			StartNew();
			Title = CustomReminderBehavior.Title;
			StartReminderCommand = new Command( x =>
			{
				StartNextCycle();
			}, x =>
			{
				return CanStartReminder();
			}, this);

			ContinueReminderCommand = new Command(x =>
			{
				StartNextCycle();
			}, x =>
			{
				if (RepeatData.ReminderStatus == EnumReminderStatus.WaitingForUser)
					return true;
				else
					return false;
			}, this);
		}

		protected virtual void StartNew() { }

		protected virtual bool CanStartReminder()
		{
			if (RepeatData.ReminderStatus == EnumReminderStatus.None)
				return true;
			else
				return false;
		}


		public string NextDisplayDate
		{
			get { return _NextDisplayDate; }
			set
			{
				if (SetProperty(ref _NextDisplayDate, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		public virtual string StartText { get => CustomReminderBehavior.StartText; } //sometimes the start text has to be different.

		public string ContinueText { get => CustomReminderBehavior.ContinueText; }
		
		private void StartReminderProcess()
		{
			if (RepeatData.ReminderStatus == EnumReminderStatus.None)
				ClearReminderData();
			else if (RepeatData.ReminderStatus == EnumReminderStatus.WaitingForReminder)
				WaitForReminder();
		}

		protected virtual bool NeedsToWaitForUserInputForNextReminder()
		{
			return true;
		}

		private void ClearReminderData()
		{
			if (PopUpVisible == true)
			{
				ThisMessage.ShowError("Can't show popup being visible when clearing reminder data.");
				return;
			}
			RepeatData.Currentreminder = new VariableCycleClass();
			RepeatData.ReminderStatus = EnumReminderStatus.None;
			NextDisplayDate = "None";
			WaitingForUser = NeedsToWaitForUserInputForNextReminder();
			RepeatData.UpTo = 0;
			StartReminderCommand.ReportCanExecuteChange();
			ContinueReminderCommand.ReportCanExecuteChange();
		}

		protected virtual void ReminderFinished() { }

		protected override async  Task AfterReminderClosedOut()
		{
			await WaitBlank(); //some need it. if not needed, just do this to prevent the warnings.
			if (RepeatData.UpTo == CustomReminderBehavior.HowManyCycles)
			{
				ClearReminderData();
				ReminderFinished();
				//IsBusy = false;
				return;
			}
			WaitingForUser = NeedsToWaitForUserInputForNextReminder();
			RepeatData.ReminderStatus = EnumReminderStatus.WaitingForUser;
			NextDisplayDate = "Start New Cycle";
			ContinueReminderCommand.ReportCanExecuteChange();
			RepeatData.Currentreminder = new VariableCycleClass();
		}

		private void StartNextCycle()
		{
			RepeatData.UpTo++;
			RepeatData.ReminderStatus = EnumReminderStatus.WaitingForReminder;
			StartReminderCommand.ReportCanExecuteChange();
			ContinueReminderCommand.ReportCanExecuteChange();
			RepeatData.TimeStarted = DateTime.Now;
			WaitForReminder();
		}

		protected abstract VariableCycleClass GetNextReminderDuration();

		private void WaitForReminder()
		{
			if (PopUpVisible == true)
			{
				ThisMessage.ShowError("Can't wait for reminder because popups shows visible");
				return;
			}
			var ThisRemind = GetNextReminderDuration();
			if (ThisRemind.Message == "" || ThisRemind.Title == "")
			{
				ThisMessage.ShowError("Must have a title and a message in order to wait for reminders");
				return;
			}
			RepeatData.Currentreminder = ThisRemind;
			RepeatData.TimeEnded = WhenToShowReminder(ThisRemind);
			if (RepeatData.TimeEnded.HasValue == false)
				return;
			NextDisplayDate = RepeatData.TimeEnded.ToString();
			WaitingForUser = false;
			ReminderBusy = false;
		}

		protected override void CheckReminders()
		{
			if (RepeatData.TimeEnded.HasValue == false)
			{
				ReminderBusy = false;
				return;
			}
			DateTime ActualDate = DateTime.Now;
			if (ActualDate > RepeatData.TimeEnded.Value)
			{
				ShowReminder(RepeatData.Currentreminder.Title, RepeatData.Currentreminder.Message);
				return;
			}
			ReminderBusy = false;
		}

		private DateTime? WhenToShowReminder(VariableCycleClass ThisRemind)
		{
			if (RepeatData.TimeStarted.HasValue == false)
			{
				ThisMessage.ShowError("Must know the time it started to get the day of the next reminder");
				return null;
			}
			if (ThisRemind.TimeFormat == EnumTimeFormat.None)
			{
				var (Days, Hours, Minutes) = ThisRemind.HowLong.GetTime();
				TimeSpan ThisSpan = new TimeSpan(Days, Hours, Minutes);
				var TotalSeconds = ThisSpan.TotalSeconds;
				return RepeatData.TimeStarted.Value.AddSeconds(TotalSeconds);
			}
			bool rets;
			rets = int.TryParse(ThisRemind.HowLong, out int NewInt);
			if (rets == false)
			{
				ThisMessage.ShowError("Must be in proper number format if the date status is not none");
				return null;
			}
			switch (ThisRemind.TimeFormat)
			{
					
				case EnumTimeFormat.Minutes:
					return RepeatData.TimeStarted.Value.AddMinutes(NewInt);
				case EnumTimeFormat.Hours:
					return RepeatData.TimeStarted.Value.AddHours(NewInt);
				case EnumTimeFormat.Days:
					return RepeatData.TimeStarted.Value.AddDays(NewInt);
				case EnumTimeFormat.Seconds:
					return RepeatData.TimeStarted.Value.AddSeconds(NewInt);
				default:
					ThisMessage.ShowError("Can't figure out the next time");
					return null;
			}
		}

	}
}
