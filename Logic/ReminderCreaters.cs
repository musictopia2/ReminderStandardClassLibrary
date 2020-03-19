using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using ReminderStandardClassLibrary.MiscClasses;
using ReminderStandardClassLibrary.Models;
using System;

namespace ReminderStandardClassLibrary.Logic
{
    public static class ReminderCreaters
    {

        public static void AppendSameDayReminders(CustomBasicList<ReminderModel> reminders, CustomBasicList<SameDayModel> days)
        {
            days.ForEach(d => AppendSameDayReminder(reminders, d));
        }

        public static void AppendSameDayReminder(CustomBasicList<ReminderModel> reminders, SameDayModel day)
        {
            ReminderModel reminder = new ReminderModel()
            {
                NextDate = day.ReminderDate,
                Message = day.Title
            };
            reminders.Add(reminder);

        }


        public static void AppendAppointments(CustomBasicList<ReminderModel> reminders, CustomBasicList<AppointmentModel> appointments, DateTime currentDate)
        {
            appointments.ForEach(a => AppendNewAppointment(reminders, a, currentDate));
        }

        public static void AppendNewAppointment(CustomBasicList<ReminderModel> reminders, AppointmentModel appointment, DateTime currentDate)
        {
            //this is easier this time.

            DateTime nextDate = appointment.AppointmentDate;

            DateTime time = DateTime.Parse(appointment.ReminderTime);

            DateTime remindDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, time.Hour, time.Minute, 0);
            if (remindDate > currentDate)
            {
                ReminderModel reminder = new ReminderModel()
                {
                    Message = appointment.Title,
                    NextDate = remindDate
                };
                reminders.Add(reminder);
            }
        }

        public static void AppendNewTemporaryReoccuringActivity(CustomBasicList<ReminderModel> reminders, TemporaryReoccuringReminderModel activity, DateTime currentDate)
        {
            DateTime tryDate;
            tryDate = activity.StartDate;

            do
            {
                //has to do at least one time.
                ReminderModel reminder = new ReminderModel()
                {
                    Message = activity.Message,
                    NextDate = tryDate
                };
                if (currentDate <= tryDate)
                {
                    reminders.Add(reminder);
                }
                switch (activity.TimeMode)
                {
                    case EnumTimeFormat.None:
                        break;
                    case EnumTimeFormat.Minutes:
                        tryDate = tryDate.AddMinutes(activity.HowMany);
                        break;
                    case EnumTimeFormat.Hours:
                        tryDate = tryDate.AddHours(activity.HowMany);
                        break;
                    case EnumTimeFormat.Days:
                        tryDate = tryDate.AddDays(activity.HowMany);
                        break;
                    case EnumTimeFormat.Seconds:
                        tryDate = tryDate.AddSeconds(activity.HowMany);
                        break;
                    default:
                        throw new BasicBlankException("Does not support mode.  Rethink");
                }
                if (tryDate > activity.EndDate)
                {
                    return;
                }


            } while (true);
        }

        public static void AppendReminders(CustomBasicList<ReminderModel> reminders, CustomBasicList<TemporaryReoccuringReminderModel> activities, DateTime currentDate)
        {

            foreach (var activity in activities)
            {

                AppendNewTemporaryReoccuringActivity(reminders, activity, currentDate);


            }
        }
    }
}
