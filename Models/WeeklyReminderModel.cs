using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using System;
namespace ReminderStandardClassLibrary.Models
{
	public class WeeklyReminderModel : ISimpleDapperEntity
	{
		public int ID { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public string Text { get; set; } = "";
	}
}