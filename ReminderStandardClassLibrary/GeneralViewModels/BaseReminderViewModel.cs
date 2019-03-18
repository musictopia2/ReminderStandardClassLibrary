using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels;
using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;

namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public abstract class BaseReminderViewModel : TimeViewModel //i don't think it should open by itself.  intended to use dependency injection engine to resolve it.
	{
		IProgress<int> SecondProgress;
		protected IView CurrentView;
		private IPopUp CurrentPopUp;

		private bool _IsDisabled;

        public BaseReminderViewModel(IFocusOnFirst TempFocus) : base(TempFocus) { }

        public bool IsDisabled
		{
			get { return _IsDisabled; }
			set
			{
				if (SetProperty(ref _IsDisabled, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _ShowSounds = true; //can have a choice of whether it will show sounds or not.   default to true

		public bool ShowSounds
		{
			get { return _ShowSounds; }
			set
			{
				if (SetProperty(ref _ShowSounds, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		protected bool HiddenBusy { get; set; }
		protected bool ReminderBusy { get; set; }
		public static  bool IsTesting { get; set; } //try to make it here.
		protected bool WaitingForUser { get; set; }

		private int _HowOftenToRepeat = 6; //this is the sounds part

		public int HowOftenToRepeat
		{
			get { return _HowOftenToRepeat; }
			set
			{
				if (SetProperty(ref _HowOftenToRepeat, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _PopUpVisible;

		public bool PopUpVisible
		{
			get { return _PopUpVisible; }
			set
			{
				if (SetProperty(ref _PopUpVisible, value, onChanged: PopUpChange))
				{
					if (value == false)
					{
						
					}
				}

			}
		}

		private async void PopUpChange() //i think
		{
			if (ShowSounds == true)
				CurrentPopUp.StopPlay();
			TimeSpan? ThisSpan = CurrentPopUp.HowLongToDelay();
			CurrentPopUp.ClosePopUp();
			CurrentPopUp = null;
			if (ThisSpan.HasValue == true)
				Snooze(ThisSpan.Value);
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
					SecondProgress.Report(0);
				} while (true);
			});
		}

		public virtual void Init(IView _View)
		{
			SecondProgress = new Progress<int>(x =>
			{
				if (IsDisabled == true || ReminderBusy == true || WaitingForUser == true || PopUpVisible == true)
					return;
				ReminderBusy = true;
				CheckReminders();

			}
			);
			RunSecondConstantTask();
			CurrentView = _View;
		}

		public void ManuallyClosePopUp()
		{
			PopUpVisible = false;
		}

		protected abstract void CheckReminders();


		protected void ShowReminder(string Title, string Message)
		{
			PopUpVisible = true;
			CurrentPopUp = CurrentView.GeneratePopUpReminder();
			CurrentPopUp.Load(Title, Message);
			if (ShowSounds == true)
				CurrentPopUp.PlaySound(HowOftenToRepeat);
			MoniterReminder();
		}

		private async void MoniterReminder()
		{
			await Task.Run(() =>
			{
				do
				{
					Thread.Sleep(100);
					if (CurrentPopUp.IsLoaded() == false)
						break;

				} while (true);
			});
			if (CurrentPopUp.IsLoaded() == true)
			{
				ThisMessage.ShowError("Can't be loaded");
				return;
			}
			PopUpVisible = false;
		}

		protected virtual void Snooze(TimeSpan ThisTime)
		{
			//default with doing nothing.
		}

		protected abstract Task AfterReminderClosedOut();
	}
}
