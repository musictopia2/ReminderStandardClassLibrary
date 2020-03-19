using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using ReminderStandardClassLibrary.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using System.Timers;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace ReminderStandardClassLibrary.Logic
{
    public static class MainReminderProcesses
    {


        static Timer? _timer;
        private static IPopUp? _currentPopUp;

        private static readonly CustomBasicList<ISubReminder> _reminderList = new CustomBasicList<ISubReminder>();

        private static ISubReminder? _currentReminder;

        public static Action<string>? ShowNextDate { get; set; }


        public static Func<Task>? UserCompletedAction { get; set; }

        private static ICurrentDate? _dateUsed; //i like setting via dependency injection so its only set once.

        private static bool _waitingForUser;
        //private static IResolver resolves;

        /// <summary>
        /// in the case of the bible program, when clicking, then would be set to false.
        /// no reminders can be processed while waiting for user.
        /// </summary>
        public static bool WaitingForUser
        {
            get
            {
                return _waitingForUser;
            }
            set
            {
                if (_waitingForUser == false && value == false)
                {
                    return;
                }
                if (value == true)
                {
                    _waitingForUser = true;
                    return;
                }
                _waitingForUser = false;
                if (UserCompletedAction == null)
                {
                    throw new BasicBlankException("No action can be invoked for waiting for user because nothing registered.  Rethink");
                }
                UserCompletedAction.Invoke();
                _timer!.Start(); //i think we can now start the timer again.
            }
        }

        public static void AddReminder(ISubReminder reminder)
        {
            _reminderList.Add(reminder);
        }
        public static void Refresh()
        {
            var reminder = _reminderList.Where(x => x.NextDate.HasValue == true).OrderBy(x => x.NextDate!.Value).FirstOrDefault();
            if (reminder == null)
            {

                Execute.OnUIThread(() =>
                {
                    ShowNextDate?.Invoke("No Reminders Set");
                });
                
                return;
            }
            Execute.OnUIThread(() =>
            {
                ShowNextDate?.Invoke(reminder.NextDate!.Value.ToString());
            });
        }
        public async static Task RecalculateRemindersAsync()
        {
            foreach (var rr in _reminderList)
            {
                await rr.GetReminderInfoAsync(await _dateUsed!.GetCurrentDateAsync());
            }
            Refresh();
        }
        public static async Task InitAsync() //this the exception worked.
        {
            //the shell view model will do this.
            _dateUsed = Resolve<ICurrentDate>();
            _timer = new Timer(1000)
            {
                AutoReset = false
            };
            _timer.Elapsed += OnTimerElapsed;
            _resolver = cons;
            await RecalculateRemindersAsync();



            //pill reminder showed using that time.  it would be good to see how it works.
            _timer.Start(); //you do have to start the timer no matter what.
            //did not need async for this part.
        }

        private async static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_currentPopUp != null || WaitingForUser)
            {
                return;
            }
            try
            {
                await Execute.OnUIThreadAsync(RunProcessAsync);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
        private static IResolver? _resolver;
        private static async Task RunProcessAsync()
        {
            foreach (var rr in _reminderList)
            {

                var (needsReminder, title, message) = await rr.GetReminderInfoAsync(await _dateUsed!.GetCurrentDateAsync());
                if (needsReminder)
                {
                    //do the popup for it now.
                    _currentReminder = rr;
                    await rr.ProcessedReminderAsync();
                    if (_resolver == null)
                    {
                        throw new BasicBlankException("Never set the IOC Container.  Rethink");
                    }
                    try
                    {
                        _currentPopUp = _resolver.Resolve<IPopUp>();

                    }
                    catch (Exception ex)
                    {

                        throw new BasicBlankException($"Failed to resolve.  Message was {ex.Message}");
                    }
                    _currentPopUp.ClosedAsync += CurrentPopupClosed;
                    _currentPopUp.SnoozedAsync += CurrentPopUpSnoozed;
                    await _currentPopUp!.LoadAsync(title, message);
                    if (_currentReminder.ShowSounds)
                    {
                        _currentPopUp.PlaySound(rr.HowOftenToRepeat);
                    }
                    return;
                }

            }

            _timer!.Start();
        }

        private static async Task CurrentPopUpSnoozed(TimeSpan arg)
        {
            ClosePopups();
            await _currentReminder!.SnoozeAsync(arg, await _dateUsed!.GetCurrentDateAsync());
            ContinueChecking();
            Refresh();
        }

        private static async Task CurrentPopupClosed()
        {

            ClosePopups();
            await _currentReminder!.CloseReminderAsync(await _dateUsed!.GetCurrentDateAsync());
            //CloseReminder?.Invoke();
            await RecalculateRemindersAsync();
            ContinueChecking();

            
        }
        private static void ContinueChecking()
        {
            _currentReminder = null;
            //Refresh(); //after you close out, refresh  i think even when snoozing same thing.
            //_timer!.Enabled = true;
            _timer!.Start(); //i think.
        }
        private static void ClosePopups()
        {
            if (_currentPopUp == null)
            {
                throw new BasicBlankException("There was no popup to even close.  Rethink");
            }
            _currentPopUp.ClosedAsync -= CurrentPopupClosed;
            _currentPopUp.SnoozedAsync -= CurrentPopUpSnoozed;
            _currentPopUp = null;
        }
    }
}