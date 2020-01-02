using CommonBasicStandardLibraries.MVVMHelpers;
using System;
namespace ReminderStandardClassLibrary.DataClasses
{
	public enum EnumAppointmentMode
	{
		None, Weekly, Manuel, Appointment
	}
	public class DesktopAppointmentData : ObservableObject
	{
		private EnumAppointmentMode _appointmentMode;

		public EnumAppointmentMode AppointmentMode
		{
			get { return _appointmentMode; }
			set
			{
				if (SetProperty(ref _appointmentMode, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private DateTime _nextReminder;

		public DateTime NextReminder
		{
			get { return _nextReminder; }
			set
			{
				if (SetProperty(ref _nextReminder, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _title = "";

		public string Title
		{
			get { return _title; }
			set
			{
				if (SetProperty(ref _title, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _message = "";

		public string Message
		{
			get { return _message; }
			set
			{
				if (SetProperty(ref _message, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _minutes;

		public int Minutes
		{
			get { return _minutes; }
			set
			{
				if (SetProperty(ref _minutes, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _hours;

		public int Hours
		{
			get { return _hours; }
			set
			{
				if (SetProperty(ref _hours, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _days;

		public int Days
		{
			get { return _days; }
			set
			{
				if (SetProperty(ref _days, value))
				{
					//can decide what to do when property changes
				}

			}
		}
		public string Display
		{
			get
			{
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
				return (int)ThisSpan.TotalSeconds;
			}
		}
	}
}