using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System;
namespace ReminderStandardClassLibrary.Models
{
	public class BaseReminderModel : ObservableObject
	{
		private EnumReminderStatus _reminderStatus = EnumReminderStatus.None;

		public EnumReminderStatus ReminderStatus
		{
			get { return _reminderStatus; }
			set
			{
				if (SetProperty(ref _reminderStatus, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _upTo;

		public int UpTo
		{
			get { return _upTo; }
			set
			{
				if (SetProperty(ref _upTo, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime? _timeStarted;

		public DateTime? TimeStarted
		{
			get { return _timeStarted; }
			set
			{
				if (SetProperty(ref _timeStarted, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime? _timeEnded;

		public DateTime? TimeEnded
		{
			get { return _timeEnded; }
			set
			{
				if (SetProperty(ref _timeEnded, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		public VariableCycleModel Currentreminder { get; set; } = new VariableCycleModel();

	}
}
