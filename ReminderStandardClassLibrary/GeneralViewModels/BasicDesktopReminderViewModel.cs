using System;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Interfaces;
using ReminderStandardClassLibrary.DataClasses;
using static CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses.CustomTimeAttribute;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
using System.Threading;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.ListsExtensions;
using static CommonBasicStandardLibraries.MVVMHelpers.Command;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;

namespace ReminderStandardClassLibrary.GeneralViewModels
{
	public class BasicDesktopReminderViewModel : BaseReminderViewModel
	{
		public IReminderWindow UIWindow;
		private IProgress<int> MinuteProgress;
		private DesktopAppointmentData _SelectedItem;
		private CustomBasicList<DesktopAppointmentData> AppointmentList = new CustomBasicList<DesktopAppointmentData>();

		public CustomBasicList<DesktopAppointmentData> ManuelList
		{
			get
			{
                return AppointmentList.Where(items => items.AppointmentMode == EnumAppointmentMode.Manuel).OrderBy(items => items.TotalSeconds).ToCustomBasicList();
			}
		}
		public DesktopAppointmentData SelectedItem
		{
			get { return _SelectedItem; }
			set
			{
				if (SetProperty(ref _SelectedItem, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _IsDebug;

		public bool IsDebug
		{
			get { return _IsDebug; }
			set
			{
				if (SetProperty(ref _IsDebug, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _MainScreenVisible;

		public bool MainScreenVisible
		{
			get { return _MainScreenVisible; }
			set
			{
				if (SetProperty(ref _MainScreenVisible, value))
				{
					//can decide what to do when property changes
					CanEnableTimeFeatures = value;
				}

			}
		}

		protected override async Task ProcessSave(object ThisObj)
		{
			//IsBusy = true;
			DesktopAppointmentData ThisAppointment = new DesktopAppointmentData()
			{
				AppointmentMode = EnumAppointmentMode.Manuel,
				Message = ReminderMessage,
				Title = "Manuel Reminder"
			};

			DateTime ModifiedDate = DateTime.Now;
			DateTime NewDate = default;
			if (TimeCategory !=  EnumTimeFormat.None)
			{
				switch (TimeCategory)
				{
					case EnumTimeFormat.None:
						ThisMessage.ShowError("Could not been None.  Really Rethink");
						return;
					case EnumTimeFormat.Minutes:
						NewDate = ModifiedDate.AddMinutes(int.Parse(TimeString));
						ThisAppointment.Minutes = int.Parse(TimeString);
						break;
					case EnumTimeFormat.Hours:
						NewDate = ModifiedDate.AddHours(int.Parse(TimeString));
						ThisAppointment.Hours = int.Parse(TimeString);
						break;
					case EnumTimeFormat.Days:
						NewDate = ModifiedDate.AddDays(int.Parse(TimeString));
						ThisAppointment.Days = int.Parse(TimeString);
						break;
					case EnumTimeFormat.Seconds:
						ThisMessage.ShowError("Seconds are not supported.  Some things use seconds but not reminders");
						break;
					default:
						ThisMessage.ShowError("Only supported Categories Are None, Minutes, Hours, and Days");
						return;
				}
			}
			else
			{
				try
				{
					var (Days, Hours, Minutes) = TimeString.GetTime();
					int TotalSeconds = TimeString.GetTotalSeconds();
					NewDate = ModifiedDate.AddSeconds(TotalSeconds);
					ThisAppointment.Hours = Hours;
					ThisAppointment.Minutes = Minutes;
					ThisAppointment.Days = Days;
				}
				catch (Exception ex)
				{
					ThisMessage.ShowError(ex.Message); //i think it should quit out if exception
					return;
				}
			}
			ThisAppointment.NextReminder = NewDate;
			AppointmentList.Add(ThisAppointment);
			UIWindow.NewContentForCombo();
			await SaveResume();
			RefreshReminderList();

            //once i know what to clear, then has to do a different way (new attribute for this).
            //ClearPropertiesWithAttributes();
            //there are 2 things to clear out.
            TimeString = "";
            ReminderMessage = "";
			RaiseFinish(); //will set isbusy to false and notify to set focus back on expected text
		}

		public Command RemoveAppointmentCommand { get; set; }
		public Command FocusComboCommand { get; set; }

		private string _ComboText;

		public string ComboText
		{
			get { return _ComboText; }
			set
			{
				if (SetProperty(ref _ComboText, value))
				{
					//can decide what to do when property changes
				}

			}
		}

        private void StartFirst()
        {
            Title = "Simple Reminder (Version 4)";



            RemoveAppointmentCommand = new Command(async x =>
            {
                //IsBusy = true;
                AppointmentList.RemoveSpecificItem(SelectedItem);
                SelectedItem = null;
                UIWindow.NewContentForCombo();
                await SaveResume();
                //IsBusy = false;
                ComboText = "";
                RefreshReminderList();
                UIWindow.FocusOnFirstControl();
            },
            x =>
            {
                //if (IsBusy == true)
                //	return false;
                return !(SelectedItem == null);

            }, this);

            FocusComboCommand = new Command(x =>
            {
                UIWindow.FocusOnCombo();
            }, x =>
            {
                return true;
            }, this);
        }

		//public BasicDesktopReminderViewModel()
		//{
			


		//}

		private void RunMinuteConstantTask()
		{
			Task.Run(() =>
			{
				do
				{
					TimeSpan ThisSpan = new TimeSpan(0, 2, 0);
					Thread.Sleep(ThisSpan);
					if (CurrentlyExecuting() == false)
						MinuteProgress.Report(0);
					else
					{
						do
						{
							if (CurrentlyExecuting() == false)
								break;
							Thread.Sleep(100);
						} while (true);
					}
				} while (true);
			});
		}

		private string _NextDateText;

		public string NextDateText
		{
			get { return _NextDateText; }
			set
			{
				if (SetProperty(ref _NextDateText, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _ReminderMessage;

		public string ReminderMessage
		{
			get { return _ReminderMessage; }
			set
			{
				if (SetProperty(ref _ReminderMessage, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private async void LoadWeeklyList()
		{
			if (IsDisabled == true)
				return;
			
			if (string.IsNullOrWhiteSpace(WeeklyPath) == true)
				return; //because its not even there
			do
			{
				if (HiddenBusy == false)
					break;
				await Task.Delay(10);
			} while (true);
			if (FileExists(WeeklyPath) == false)
				return;
			CustomBasicList<WeeklyReminderClass> ThisList = await RetrieveSavedObjectAsync<CustomBasicList<WeeklyReminderClass>>(WeeklyPath);
			AppointmentList.RemoveAllOnly(y => y.AppointmentMode == EnumAppointmentMode.Weekly); //this will remove all the weekly ones from the appointment list
			ThisList.ForEach(ThisItem =>
			{
				DesktopAppointmentData ThisAppointment = new DesktopAppointmentData()
				{
					AppointmentMode = EnumAppointmentMode.Weekly
				};
				DateTime CurrentDate = DateTime.Now;
				DateTime NextDate = default;
				int x = 0;
				do
				{
					x++;
					if (CurrentDate.DayOfWeek == ThisItem.DayOfWeek)
					{
						NextDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, ThisItem.Hour, ThisItem.Minute, 0);
						if (CurrentDate > NextDate && x == 1)
							NextDate = NextDate.AddDays(7);
						break;
						
					}
					CurrentDate = CurrentDate.AddDays(1);
				} while (true);
				ThisAppointment.NextReminder = NextDate;
				ThisAppointment.Message = ThisItem.Text;
				ThisAppointment.Title = "Weekly Appointment";
				AppointmentList.Add(ThisAppointment);
				OnPropertyChanged(nameof(ManuelList));
				
			}
			);
            await SaveResume(); //try to save after finished
            RefreshReminderList();
			HiddenBusy = false;
		}

		private void RefreshReminderList()
		{
			if (AppointmentList.Count == 0)
				NextDateText = "No Reminders Set";
			else
			{
				var ThisItem = AppointmentList.OrderBy(Items => Items.NextReminder).Take(1).Single();
				NextDateText = ThisItem.NextReminder.ToString();
			}
		}

		public async Task SaveResume() //rethinking is required because there is more than one.  this means the interface needs to be more complex.
		{
			if (ParentPath == null)
				return;
			await SaveObjectAsync(ResumePath, AppointmentList);
		}

		private async Task LoadSpecificAppointments()
		{
			if (IsDisabled == true)
				return;

			if (string.IsNullOrWhiteSpace(SpecificPath) == true)
				return; //because its not even there
			do
			{
				if (HiddenBusy == false)
					break;
				await Task.Delay(10);
			} while (true);
			HiddenBusy = true;
			CustomBasicList<string> ThisList = await TextFromFileListAsync(SpecificPath);
			CustomBasicList<DateTime> DateList = new CustomBasicList<DateTime>();
			DateTime CurrentDate = DateTime.Now;
			ThisList.ForEach(ThisItem =>
			{
				CustomBasicList<string> TempList = ThisItem.Split(Constants.vbTab).ToCustomBasicList();
				if (TempList.Count == 1 || TempList.Count == 2)
				{
					bool rets = DateTime.TryParse(TempList.First(), out DateTime NewDate);
					if (rets == true && CurrentDate<NewDate &&
					AppointmentList.Exists(xx => xx.NextReminder == NewDate && xx.AppointmentMode  == EnumAppointmentMode.Appointment)
					)
					{
						DesktopAppointmentData ThisAppointment = new DesktopAppointmentData()
						{
							AppointmentMode = EnumAppointmentMode.Appointment,
							Title = "Appointment To Do Specific Activity",
							NextReminder = NewDate
						};
						if (TempList.Count == 2)
							ThisAppointment.Message = TempList.Last();
						AppointmentList.Add(ThisAppointment);
					}
				}
			});
			RefreshReminderList();
			HiddenBusy = false;
		}

		private string ParentPath = "";
		private string ResumePath = "";
		private string WeeklyPath = "";
		private string SpecificPath = "";

        public BasicDesktopReminderViewModel(IFocusOnFirst TempFocus) : base(TempFocus)
        {
            StartFirst();
        }

        private void GetPaths()
		{
			IReminderPaths Temps = cons.Resolve<IReminderPaths>();
			if (Temps != null)
			{
				ParentPath = Temps.GetReminderParentPath();
				ResumePath = $"{ParentPath}resumedata.json";
				WeeklyPath = $"{ParentPath}weeklyreminders.json";
				SpecificPath = $"{ParentPath}appointmentlist.txt";
			}
		}

		public override async  void Init(IView _View)
		{
			GetPaths();
			if (string.IsNullOrWhiteSpace(ParentPath) == true)
			{
				ThisMessage.ShowError("You must decide where to place the reminder data.  Otherwise, no autoresume and you will be disappointed");
				return;
			}
			if (IsDebug == false)
			{
				MinuteProgress = new Progress<int>(async x =>
				{
					LoadWeeklyList();
					await LoadSpecificAppointments();
				});
			}
			RunMinuteConstantTask();
			base.Init(_View);
			if (FileExists(ResumePath) == true)
			{
				AppointmentList = await RetrieveSavedObjectAsync<CustomBasicList<DesktopAppointmentData>> (ResumePath);
				if (AppointmentList == null)
					AppointmentList = new CustomBasicList<DesktopAppointmentData>();

			}
			LoadWeeklyList();
			await LoadSpecificAppointments();
			UIWindow.NewContentForCombo();
		}

		protected override void CheckReminders()
		{
			if (AppointmentList.Count == 0)
			{
				ReminderBusy = false;
				return;
			}
			DateTime CurrentDate = DateTime.Now;
			var ThisList = AppointmentList.Where(Items => CurrentDate > Items.NextReminder).OrderBy(Items => Items.NextReminder).ToCustomBasicCollection();
			if (ThisList.Count == 0)
			{
				ReminderBusy = false;
				return;
			}
			var ThisReminder = ThisList.First();
			AppointmentList.RemoveGivenList(ThisList); //since no observable, then does not matter
			ShowReminder(ThisReminder.Title, ThisReminder.Message);
		}

		protected override async Task AfterReminderClosedOut()
		{
			await SaveResume();
			UIWindow.NewContentForCombo();
			IsDisabled = false;
			RefreshReminderList();
			UIWindow.FocusOnFirstControl();
			ReminderBusy = false;
		}
	}
}
