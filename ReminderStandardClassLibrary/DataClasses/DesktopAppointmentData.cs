using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.CollectionClasses;
namespace ReminderStandardClassLibrary.DataClasses
{
	public enum EnumAppointmentMode
	{
		None, Weekly, Manuel, Appointment
	}
	public class DesktopAppointmentData : ObservableObject
	{
		private EnumAppointmentMode _AppointmentMode;

		public EnumAppointmentMode AppointmentMode
		{
			get { return _AppointmentMode; }
			set
			{
				if (SetProperty(ref _AppointmentMode, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime _NextReminder;

		public DateTime NextReminder
		{
			get { return _NextReminder; }
			set
			{
				if (SetProperty(ref _NextReminder, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _Title;

		public string Title
		{
			get { return _Title; }
			set
			{
				if (SetProperty(ref _Title, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _Message;

		public string Message
		{
			get { return _Message; }
			set
			{
				if (SetProperty(ref _Message, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _Minutes;

		public int Minutes
		{
			get { return _Minutes; }
			set
			{
				if (SetProperty(ref _Minutes, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _Hours;

		public int Hours
		{
			get { return _Hours; }
			set
			{
				if (SetProperty(ref _Hours, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _Days;

		public int Days
		{
			get { return _Days; }
			set
			{
				if (SetProperty(ref _Days, value))
				{
					//can decide what to do when property changes
				}

			}
		}


		public string Display
		{
			get {
				if (Hours == 0 && Days == 0)
					return $"{Minutes} Minutes";
				else if (Days == 0)
					return $"{Hours} Hours, {Minutes} Minutes";
				else
					return $"{Days} Days, {Hours} Hours, {Minutes} Minutes";
			}
		}

		internal int TotalSeconds
		{
			get
			{
				TimeSpan ThisSpan = new TimeSpan(Days, Hours, Minutes);
				return (int) ThisSpan.TotalSeconds;
			}
		}

	}
}
