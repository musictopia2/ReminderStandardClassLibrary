using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.GeneralViewModels;
namespace ReminderStandardClassLibrary.DataClasses
{
	public class BaseReminderClass : ObservableObject
	{
		private EnumReminderStatus _ReminderStatus = EnumReminderStatus.None;

		public EnumReminderStatus ReminderStatus
		{
			get { return _ReminderStatus; }
			set
			{
				if (SetProperty(ref _ReminderStatus, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _UpTo;

		public int UpTo
		{
			get { return _UpTo; }
			set
			{
				if (SetProperty(ref _UpTo, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime? _TimeStarted;

		public DateTime? TimeStarted
		{
			get { return _TimeStarted; }
			set
			{
				if (SetProperty(ref _TimeStarted, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime? _TimeEnded;

		public DateTime? TimeEnded
		{
			get { return _TimeEnded; }
			set
			{
				if (SetProperty(ref _TimeEnded, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		public VariableCycleClass Currentreminder { get; set; } = new VariableCycleClass();

	}
}
