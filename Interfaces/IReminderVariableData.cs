using CommonBasicStandardLibraries.CollectionClasses;
using ReminderStandardClassLibrary.DataClasses;
namespace ReminderStandardClassLibrary.Interfaces
{
    public interface IReminderVariableData : IReminderBasicData
	{
		CustomBasicList<VariableCycleClass> GetVariableList();
	}
}
