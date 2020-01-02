using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels;
using ReminderStandardClassLibrary.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public abstract class BaseReminderViewModel : TimeViewModel //i don't think it should open by itself.  intended to use dependency injection engine to resolve it.
	{
		IProgress<int>? _secondProgress;
		protected IView? CurrentView;
		private IPopUp? _currentPopUp;
		private bool _isDisabled;
		public bool IsDisabled
		{
			get { return _isDisabled; }
			set
			{
				if (SetProperty(ref _isDisabled, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _showSounds = true; //can have a choice of whether it will show sounds or not.   default to true
		public bool ShowSounds
		{
			get { return _showSounds; }
			set
			{
				if (SetProperty(ref _showSounds, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		protected bool HiddenBusy { get; set; }
		protected bool ReminderBusy { get; set; }
		public static bool IsTesting { get; set; } //try to make it here.
		protected bool WaitingForUser { get; set; }

		private int _howOftenToRepeat = 6; //this is the sounds part

		public int HowOftenToRepeat
		{
			get { return _howOftenToRepeat; }
			set
			{
				if (SetProperty(ref _howOftenToRepeat, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _popUpVisible;

		public BaseReminderViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
		{
		}

		public bool PopUpVisible
		{
			get { return _popUpVisible; }
			set
			{
				if (SetProperty(ref _popUpVisible, value, onChanged: PopUpChange))
				{
				}

			}
		}

		private async void PopUpChange() //i think
		{
			if (PopUpVisible == false)
				return; //maybe this way.
			if (ShowSounds == true)
				_currentPopUp!.StopPlay();
			TimeSpan? thisSpan = _currentPopUp!.HowLongToDelay();
			_currentPopUp.ClosePopUp();
			_currentPopUp = null;
			if (thisSpan.HasValue == true)
				Snooze(thisSpan!.Value);
			else
				await AfterReminderClosedOut(); //this has to run before it can do the other part.

			//IsBusy = false;
		}

		private void RunSecondConstantTask()
		{
			Task.Run(() =>
			{
				do
				{
					Thread.Sleep(1000);
					_secondProgress!.Report(0);
				} while (true);
			});
		}

		public virtual Task InitAsync(IView view)
		{
			_secondProgress = new Progress<int>(x =>
			{
				if (IsDisabled == true || ReminderBusy == true || WaitingForUser == true || PopUpVisible == true)
					return;
				ReminderBusy = true;
				CheckReminders();

			}
			);
			RunSecondConstantTask();
			CurrentView = view;
			return Task.CompletedTask;
		}

		public void ManuallyClosePopUp()
		{
			PopUpVisible = false;
		}

		protected abstract void CheckReminders();


		protected void ShowReminder(string title, string message)
		{
			PopUpVisible = true;
			_currentPopUp = CurrentView!.GeneratePopUpReminder();
			_currentPopUp.Load(title, message);
			if (ShowSounds == true)
				_currentPopUp.PlaySound(HowOftenToRepeat);
			MoniterReminder();
		}

		private async void MoniterReminder()
		{
			await Task.Run(() =>
			{
				do
				{
					Thread.Sleep(100);
					if (_currentPopUp!.IsLoaded() == false)
						break;

				} while (true);
			});
			if (_currentPopUp!.IsLoaded() == true)
			{
				ThisMessage.ShowError("Can't be loaded");
				return;
			}
			PopUpVisible = false;
		}

		protected virtual void Snooze(TimeSpan thisTime)
		{
			//default with doing nothing.
		}

		protected abstract Task AfterReminderClosedOut();
	}
}