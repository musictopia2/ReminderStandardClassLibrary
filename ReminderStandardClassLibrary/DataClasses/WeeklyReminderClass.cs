using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderStandardClassLibrary.DataClasses
{
	public class WeeklyReminderClass
	{
		public DayOfWeek DayOfWeek { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public string Text { get; set; }
	}
}
