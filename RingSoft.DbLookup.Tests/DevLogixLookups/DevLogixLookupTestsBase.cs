using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.DevLogix;
using RingSoft.DbLookup.App.Library.DevLogix.LookupModel;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Tests.DevLogixLookups
{
    public abstract class DevLogixLookupTestsBase
    {
        protected abstract IDevLogixLookupContext LookupContext { get; }

        public void Base_ErrorsLookupSortByErrorNumber_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(137, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(138, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(153, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(124, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("e-2");
            Assert.AreEqual(200, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }

        public void Base_ErrorsLookupSortByErrorNumber_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.ErrorNumber); //Sets to descending.
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(231, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(229, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(210, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(269, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("e-2");
            Assert.AreEqual(199, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }

        public void Base_ErrorsLookupSortByDate_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Date);
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(137, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(138, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(153, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(124, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("01/01/2015");
            Assert.AreEqual(178, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");
        }

        public void Base_ErrorsLookupSortByDate_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Date);
            lookupData.OnColumnClick(p => p.Date);  //Sets to descending.
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(231, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(229, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(210, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(269, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("01/01/2015");
            Assert.AreEqual(177, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");
        }

        public void Base_ErrorsLookupSortByDeveloperType_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.DeveloperType);
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(137, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(138, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(153, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(124, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("d");
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Search equals fails.");
        }

        public void Base_ErrorsLookupSortByDeveloperType_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.DeveloperType);
            lookupData.OnColumnClick(p => p.DeveloperType);  //Sets to descending.
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(231, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(229, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(210, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(269, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(270, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("d");
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Search equals fails.");
        }

        public void Base_ErrorsLookupSortByHoursSpent_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.HoursSpent);
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(138, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(139, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(154, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(125, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(124, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(198, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("0");
            Assert.AreEqual(123, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");
        }

        public void Base_ErrorsLookupSortByHoursSpent_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 15;
            var lookupData = new LookupData<ErrorLookup>(LookupContext.DevLogixConfiguration.ErrorsLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.HoursSpent);
            lookupData.OnColumnClick(p => p.HoursSpent);  //Sets to descending.
            Assert.AreEqual(15, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(231, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(206, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(221, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(196, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(198, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(124, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition, "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("0");
            Assert.AreEqual(206, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition, "Set scroll position to middle fails.");
        }

        //Issues Lookup---------------------------------------------------------------------------------------------------------------------------

        public void Base_IssuesLookupSortByTask_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 13;
            var lookupData = new LookupData<IssueLookup>(LookupContext.DevLogixConfiguration.IssuesLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");
            Assert.AreEqual(13, lookupData.LookupResultsList.Count, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Task);
            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(402, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(405, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(376, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(417, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(419, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(432, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("f");
            Assert.AreEqual(436, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }

        public void Base_IssuesLookupSortByTask_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 13;
            var lookupData = new LookupData<IssueLookup>(LookupContext.DevLogixConfiguration.IssuesLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Task);
            lookupData.OnColumnClick(p => p.Task); //Sets to descending.
            Assert.AreEqual(13, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(396, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(398, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(427, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(429, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(432, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(419, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("f");
            Assert.AreEqual(415, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }

        public void Base_IssuesLookupSortByResolved_Ascending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 13;
            var lookupData = new LookupData<IssueLookup>(LookupContext.DevLogixConfiguration.IssuesLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");
            Assert.AreEqual(13, lookupData.LookupResultsList.Count, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Resolved);
            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(379, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(436, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(431, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(438, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(434, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(418, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("y");
            Assert.AreEqual(404, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }

        public void Base_IssuesLookupSortByResolved_Descending()
        {
            //Arrange
            var userInterface = new LookupDataUserInterface();
            userInterface.PageSize = 13;
            var lookupData = new LookupData<IssueLookup>(LookupContext.DevLogixConfiguration.IssuesLookup.Clone(), userInterface);

            //Act/Assert
            var getDataResult = lookupData.GetInitData();
            Assert.IsTrue(getDataResult.ResultCode == GetDataResultCodes.Success, "Get initial data fails.");

            lookupData.OnColumnClick(p => p.Resolved);
            lookupData.OnColumnClick(p => p.Resolved); //Sets to descending.
            Assert.AreEqual(13, lookupData.LookupResultsList.Count, "Get initial data fails.");

            var checkRow = lookupData.LookupResultsList[lookupData.LookupResultsList.Count - 1];
            Assert.AreEqual(455, checkRow.Id, "Get initial data fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoNextRecord();
            Assert.AreEqual(413, lookupData.SelectedItem.Id, "Down arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");

            lookupData.GotoNextPage();
            Assert.AreEqual(420, lookupData.SelectedItem.Id, "Page down fails.");

            lookupData.GotoPreviousPage();
            Assert.AreEqual(453, lookupData.SelectedItem.Id, "Page up fails.");

            lookupData.GotoPreviousRecord();
            Assert.AreEqual(418, lookupData.SelectedItem.Id, "Up arrow fails.");
            Assert.AreEqual(LookupScrollPositions.Top, lookupData.ScrollPosition, "Set scroll position to top fails.");

            lookupData.GotoBottom();
            Assert.AreEqual(434, lookupData.SelectedItem.Id, "Goto bottom fails.");
            Assert.AreEqual(LookupScrollPositions.Bottom, lookupData.ScrollPosition,
                "Set scroll position to bottom fails.");

            lookupData.OnSearchForChange("y");
            Assert.AreEqual(433, lookupData.SelectedItem.Id, "Search equals fails.");
            Assert.AreEqual(LookupScrollPositions.Middle, lookupData.ScrollPosition,
                "Set scroll position to middle fails.");
        }
    }
}