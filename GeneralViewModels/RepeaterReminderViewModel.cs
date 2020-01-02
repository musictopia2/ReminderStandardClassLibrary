using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using ReminderStandardClassLibrary.DataClasses;
using ReminderStandardClassLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
using static CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses.CustomTimeAttribute;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public abstract class RepeaterReminderViewModel : BaseReminderViewModel
	{
		protected abstract int HowManyCycles();
		protected BaseReminderClass RepeatData = new BaseReminderClass();

		public Command StartReminderCommand { get; set; }
		public Command ContinueReminderCommand { get; set; }

		private string _nextDisplayDate = "";

		private readonly IReminderBasicData _customReminderBehavior; //hopefully this will work.  can cast it if necessary.

		public RepeaterReminderViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI, IReminderBasicData tempremind) : base(tempFocus, tempUI)
		{
			_customReminderBehavior = tempremind;
			if (_customReminderBehavior == null)
				throw new BasicBlankException("You never sent in the custom reminder behavior.  Should have used dependency injection instead of generics");
			//CustomReminderBehavior = _Behave;
			StartNew();
			Title = _customReminderBehavior.Title;
			StartReminderCommand = new Command(x =>
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
			get { return _nextDisplayDate; }
			set
			{
				if (SetProperty(ref _nextDisplayDate, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		public virtual string StartText { get => _customReminderBehavior.StartText; } //sometimes the start text has to be different.

		public string ContinueText { get => _customReminderBehavior.ContinueText; }

		//maybe we don't need it  if i am wrong, rethink
		//private void StartReminderProcess()
		//{
		//	if (RepeatData.ReminderStatus == EnumReminderStatus.None)
		//		ClearReminderData();
		//	else if (RepeatData.ReminderStatus == EnumReminderStatus.WaitingForReminder)
		//		WaitForReminder();
		//}

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

		protected override async Task AfterReminderClosedOut()
		{
			await Task.CompletedTask;
			if (RepeatData.UpTo == _customReminderBehavior.HowManyCycles)
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
			var thisRemind = GetNextReminderDuration();
			if (thisRemind.Message == "" || thisRemind.Title == "")
			{
				ThisMessage.ShowError("Must have a title and a message in order to wait for reminders");
				return;
			}
			RepeatData.Currentreminder = thisRemind;
			RepeatData.TimeEnded = WhenToShowReminder(thisRemind);
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
			DateTime actualDate = DateTime.Now;
			if (actualDate > RepeatData.TimeEnded!.Value)
			{
				ShowReminder(RepeatData.Currentreminder.Title, RepeatData.Currentreminder.Message);
				return;
			}
			ReminderBusy = false;
		}

		private DateTime? WhenToShowReminder(VariableCycleClass thisRemind)
		{
			if (RepeatData.TimeStarted.HasValue == false)
			{
				ThisMessage.ShowError("Must know the time it started to get the day of the next reminder");
				return null;
			}
			if (thisRemind.TimeFormat == EnumTimeFormat.None)
			{
				var (days, hours, minutes) = thisRemind.HowLong.GetTime();
				TimeSpan thisSpan = new TimeSpan(days, hours, minutes);
				var totalSeconds = thisSpan.TotalSeconds;
				return RepeatData.TimeStarted!.Value.AddSeconds(totalSeconds);
			}
			bool rets;
			rets = int.TryParse(thisRemind.HowLong, out int NewInt);
			if (rets == false)
			{
				ThisMessage.ShowError("Must be in proper number format if the date status is not none");
				return null;
			}
			switch (thisRemind.TimeFormat)
			{

				case EnumTimeFormat.Minutes:
					return RepeatData.TimeStarted!.Value.AddMinutes(NewInt);
				case EnumTimeFormat.Hours:
					return RepeatData.TimeStarted!.Value.AddHours(NewInt);
				case EnumTimeFormat.Days:
					return RepeatData.TimeStarted!.Value.AddDays(NewInt);
				case EnumTimeFormat.Seconds:
					return RepeatData.TimeStarted!.Value.AddSeconds(NewInt);
				default:
					ThisMessage.ShowError("Can't figure out the next time");
					return null;
			}
		}
	}
}