using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.Models;

namespace ReminderStandardClassLibrary.Interfaces
{
	public interface IReminderVariableData : IReminderBasicData
	{
		CustomBasicList<VariableCycleModel> GetVariableList();
	}
}
