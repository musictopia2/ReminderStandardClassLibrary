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
		protected IReminderWindow UIWindow; //try this way
		private IProgress<int>? _minuteProgress;
		private DesktopAppointmentData? _selectedItem;
		private CustomBasicList<DesktopAppointmentData> _appointmentList = new CustomBasicList<DesktopAppointmentData>();

		public CustomBasicList<DesktopAppointmentData> ManuelList
		{
			get
			{
                return _appointmentList.Where(items => items.AppointmentMode == EnumAppointmentMode.Manuel).OrderBy(items => items.TotalSeconds).ToCustomBasicList();
			}
		}
		public DesktopAppointmentData? SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (SetProperty(ref _selectedItem, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _isDebug;

		public bool IsDebug
		{
			get { return _isDebug; }
			set
			{
				if (SetProperty(ref _isDebug, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _mainScreenVisible;

		public bool MainScreenVisible
		{
			get { return _mainScreenVisible; }
			set
			{
				if (SetProperty(ref _mainScreenVisible, value))
				{
					//can decide what to do when property changes
					CanEnableTimeFeatures = value;
				}

			}
		}

		protected override async Task ProcessSave(object thisObj)
		{
			//IsBusy = true;
			DesktopAppointmentData thisAppointment = new DesktopAppointmentData()
			{
				AppointmentMode = EnumAppointmentMode.Manuel,
				Message = ReminderMessage,
				Title = "Manuel Reminder"
			};
			DateTime modifiedDate = DateTime.Now;
			DateTime newDate = default;
			if (TimeCategory !=  EnumTimeFormat.None)
			{
				switch (TimeCategory)
				{
					case EnumTimeFormat.None:
						ThisMessage.ShowError("Could not been None.  Really Rethink");
						return;
					case EnumTimeFormat.Minutes:
						newDate = modifiedDate.AddMinutes(int.Parse(TimeString));
						thisAppointment.Minutes = int.Parse(TimeString);
						break;
					case EnumTimeFormat.Hours:
						newDate = modifiedDate.AddHours(int.Parse(TimeString));
						thisAppointment.Hours = int.Parse(TimeString);
						break;
					case EnumTimeFormat.Days:
						newDate = modifiedDate.AddDays(int.Parse(TimeString));
						thisAppointment.Days = int.Parse(TimeString);
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
					var (days, hours, minutes) = TimeString.GetTime();
					int totalSeconds = TimeString.GetTotalSeconds();
					newDate = modifiedDate.AddSeconds(totalSeconds);
					thisAppointment.Hours = hours;
					thisAppointment.Minutes = minutes;
					thisAppointment.Days = days;
				}
				catch (Exception ex)
				{
					ThisMessage.ShowError(ex.Message); //i think it should quit out if exception
					return;
				}
			}
			thisAppointment.NextReminder = newDate;
			_appointmentList.Add(thisAppointment);
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

		private string _comboText = "";

		public string ComboText
		{
			get { return _comboText; }
			set
			{
				if (SetProperty(ref _comboText, value))
				{
					//can decide what to do when property changes
				}

			}
		}


		private void RunMinuteConstantTask()
		{
			Task.Run(() =>
			{
				do
				{
					TimeSpan thisSpan = new TimeSpan(0, 2, 0);
					Thread.Sleep(thisSpan);
					if (CurrentlyExecuting() == false)
						_minuteProgress!.Report(0);
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

		private string _nextDateText = "";

		public string NextDateText
		{
			get { return _nextDateText; }
			set
			{
				if (SetProperty(ref _nextDateText, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _reminderMessage = "";

		public string ReminderMessage
		{
			get { return _reminderMessage; }
			set
			{
				if (SetProperty(ref _reminderMessage, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private async void LoadWeeklyList()
		{
			if (IsDisabled == true)
				return;
			
			if (string.IsNullOrWhiteSpace(_weeklyPath) == true)
				return; //because its not even there
			do
			{
				if (HiddenBusy == false)
					break;
				await Task.Delay(10);
			} while (true);
			if (FileExists(_weeklyPath) == false)
				return;
			CustomBasicList<WeeklyReminderClass> thisList = await RetrieveSavedObjectAsync<CustomBasicList<WeeklyReminderClass>>(_weeklyPath);
			_appointmentList.RemoveAllOnly(y => y.AppointmentMode == EnumAppointmentMode.Weekly); //this will remove all the weekly ones from the appointment list
			thisList.ForEach(thisItem =>
			{
				DesktopAppointmentData thisAppointment = new DesktopAppointmentData()
				{
					AppointmentMode = EnumAppointmentMode.Weekly
				};
				DateTime currentDate = DateTime.Now;
				DateTime nextDate = default;
				int x = 0;
				do
				{
					x++;
					if (currentDate.DayOfWeek == thisItem.DayOfWeek)
					{
						nextDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, thisItem.Hour, thisItem.Minute, 0);
						if (currentDate > nextDate && x == 1)
							nextDate = nextDate.AddDays(7);
						break;
						
					}
					currentDate = currentDate.AddDays(1);
				} while (true);
				thisAppointment.NextReminder = nextDate;
				thisAppointment.Message = thisItem.Text;
				thisAppointment.Title = "Weekly Appointment";
				_appointmentList.Add(thisAppointment);
				OnPropertyChanged(nameof(ManuelList));
				
			}
			);
            await SaveResume(); //try to save after finished
            RefreshReminderList();
			HiddenBusy = false;
		}

		private void RefreshReminderList()
		{
			if (_appointmentList.Count == 0)
				NextDateText = "No Reminders Set";
			else
			{
				var thisItem = _appointmentList.OrderBy(Items => Items.NextReminder).Take(1).Single();
				NextDateText = thisItem.NextReminder.ToString();
			}
		}

		public async Task SaveResume() //rethinking is required because there is more than one.  this means the interface needs to be more complex.
		{
			if (_parentPath == null)
				return;
			await SaveObjectAsync(_resumePath, _appointmentList);
		}

		private async Task LoadSpecificAppointments()
		{
			if (IsDisabled == true)
				return;

			if (string.IsNullOrWhiteSpace(_specificPath) == true)
				return; //because its not even there
			do
			{
				if (HiddenBusy == false)
					break;
				await Task.Delay(10);
			} while (true);
			HiddenBusy = true;
			CustomBasicList<string> thisList = await TextFromFileListAsync(_specificPath);
			CustomBasicList<DateTime> dateList = new CustomBasicList<DateTime>();
			DateTime CurrentDate = DateTime.Now;
			thisList.ForEach(thisItem =>
			{
				CustomBasicList<string> tempList = thisItem.Split(Constants.vbTab).ToCustomBasicList();
				if (tempList.Count == 1 || tempList.Count == 2)
				{
					bool rets = DateTime.TryParse(tempList.First(), out DateTime NewDate);
					if (rets == true && CurrentDate<NewDate &&
					_appointmentList.Exists(xx => xx.NextReminder == NewDate && xx.AppointmentMode  == EnumAppointmentMode.Appointment)
					)
					{
						DesktopAppointmentData ThisAppointment = new DesktopAppointmentData()
						{
							AppointmentMode = EnumAppointmentMode.Appointment,
							Title = "Appointment To Do Specific Activity",
							NextReminder = NewDate
						};
						if (tempList.Count == 2)
							ThisAppointment.Message = tempList.Last();
						_appointmentList.Add(ThisAppointment);
					}
				}
			});
			RefreshReminderList();
			HiddenBusy = false;
		}

		private string _parentPath = "";
		private string _resumePath = "";
		private string _weeklyPath = "";
		private string _specificPath = "";

		public BasicDesktopReminderViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI, IReminderWindow window) : base(tempFocus, tempUI)
		{
			Title = "Simple Reminder (Version 5)";
			UIWindow = window;
			RemoveAppointmentCommand = new Command(async x =>
			{
				//IsBusy = true;
				_appointmentList.RemoveSpecificItem(SelectedItem!);
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

		//public BasicDesktopReminderViewModel(IFocusOnFirst TempFocus) : base(TempFocus)
		//{
		//    StartFirst();
		//}

		private void GetPaths()
		{
			IReminderPaths temps = cons!.Resolve<IReminderPaths>();
			if (temps != null)
			{
				_parentPath = temps.GetReminderParentPath();
				_resumePath = $"{_parentPath}resumedata.json";
				_weeklyPath = $"{_parentPath}weeklyreminders.json";
				_specificPath = $"{_parentPath}appointmentlist.txt";
			}
		}

		public override async  Task InitAsync(IView view)
		{
			GetPaths();
			if (string.IsNullOrWhiteSpace(_parentPath) == true)
			{
				ThisMessage.ShowError("You must decide where to place the reminder data.  Otherwise, no autoresume and you will be disappointed");
				return;
			}
			if (IsDebug == false)
			{
				_minuteProgress = new Progress<int>(async x =>
				{
					LoadWeeklyList();
					await LoadSpecificAppointments();
				});
			}
			RunMinuteConstantTask();
			await base.InitAsync(view);
			if (FileExists(_resumePath) == true)
			{
				_appointmentList = await RetrieveSavedObjectAsync<CustomBasicList<DesktopAppointmentData>> (_resumePath);
				if (_appointmentList == null)
					_appointmentList = new CustomBasicList<DesktopAppointmentData>();

			}
			LoadWeeklyList();
			await LoadSpecificAppointments();
			UIWindow.NewContentForCombo();
		}

		protected override void CheckReminders()
		{
			if (_appointmentList.Count == 0)
			{
				ReminderBusy = false;
				return;
			}
			DateTime currentDate = DateTime.Now;
			var thisList = _appointmentList.Where(Items => currentDate > Items.NextReminder).OrderBy(Items => Items.NextReminder).ToCustomBasicCollection();
			if (thisList.Count == 0)
			{
				ReminderBusy = false;
				return;
			}
			var thisReminder = thisList.First();
			_appointmentList.RemoveGivenList(thisList); //since no observable, then does not matter
			ShowReminder(thisReminder.Title, thisReminder.Message);
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
