using System;
namespace ReminderStandardClassLibrary.Models
{
	public class WeeklyReminderModel
	{
		public DayOfWeek DayOfWeek { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public string Text { get; set; } = "";
	}
}