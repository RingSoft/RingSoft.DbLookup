// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="DbMaintenanceWindowProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// DbMaintenanceWindowProcessor base class that defines the behavior of a database entity maintenance window.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.IDbMaintenanceProcessor" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.IDbMaintenanceProcessor" />
    public abstract class DbMaintenanceWindowProcessor : IDbMaintenanceProcessor
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DbMaintenanceViewModelBase ViewModel { get; private set; }

        /// <summary>
        /// Gets the key automatic fill control.
        /// </summary>
        /// <value>The key automatic fill control.</value>
        public AutoFillControl KeyAutoFillControl { get; private set; }

        /// <summary>
        /// Gets the save button.
        /// </summary>
        /// <value>The save button.</value>
        public abstract Button SaveButton { get; }
        /// <summary>
        /// Gets the save button UI control.
        /// </summary>
        /// <value>The save button UI control.</value>
        public VmUiControl SaveButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the select button.
        /// </summary>
        /// <value>The select button.</value>
        public abstract Button SelectButton { get; }
        /// <summary>
        /// Gets the select button UI control.
        /// </summary>
        /// <value>The select button UI control.</value>
        public VmUiControl SelectButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the delete button.
        /// </summary>
        /// <value>The delete button.</value>
        public abstract Button DeleteButton { get; }
        /// <summary>
        /// Gets the delete button UI control.
        /// </summary>
        /// <value>The delete button UI control.</value>
        public VmUiControl DeleteButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the find button.
        /// </summary>
        /// <value>The find button.</value>
        public abstract Button FindButton { get; }
        /// <summary>
        /// Gets the find button UI control.
        /// </summary>
        /// <value>The find button UI control.</value>
        public VmUiControl FindButtonUiControl { get; private set; }
        /// <summary>
        /// Creates new button.
        /// </summary>
        /// <value>The new button.</value>
        public abstract Button NewButton { get; }
        /// <summary>
        /// Creates new buttonuicontrol.
        /// </summary>
        /// <value>The new button UI control.</value>
        public VmUiControl NewButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the close button.
        /// </summary>
        /// <value>The close button.</value>
        public abstract Button CloseButton { get; }
        /// <summary>
        /// Gets the next button.
        /// </summary>
        /// <value>The next button.</value>
        public abstract Button NextButton { get; }
        /// <summary>
        /// Gets the next button UI control.
        /// </summary>
        /// <value>The next button UI control.</value>
        public VmUiControl NextButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the previous button.
        /// </summary>
        /// <value>The previous button.</value>
        public abstract Button PreviousButton { get; }
        /// <summary>
        /// Gets the previous button UI control.
        /// </summary>
        /// <value>The previous button UI control.</value>
        public VmUiControl PreviousButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the print button.
        /// </summary>
        /// <value>The print button.</value>
        public abstract Button PrintButton { get; }
        /// <summary>
        /// Gets the print button UI control.
        /// </summary>
        /// <value>The print button UI control.</value>
        public VmUiControl PrintButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the maintenance window.
        /// </summary>
        /// <value>The maintenance window.</value>
        public ContentControl MaintenanceWindow { get; private set; }
        /// <summary>
        /// Gets the maintenance buttons control.
        /// </summary>
        /// <value>The maintenance buttons control.</value>
        public Control MaintenanceButtonsControl { get; private set; }
        /// <summary>
        /// Gets the maintenance buttons UI control.
        /// </summary>
        /// <value>The maintenance buttons UI control.</value>
        public VmUiControl MaintenanceButtonsUiControl { get; private set; }
        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IDbMaintenanceView LibView { get; private set; }
        /// <summary>
        /// Gets the status bar.
        /// </summary>
        /// <value>The status bar.</value>
        public DbMaintenanceStatusBar StatusBar { get; private set; }
        /// <summary>
        /// Gets the status bar UI control.
        /// </summary>
        /// <value>The status bar UI control.</value>
        public VmUiControl StatusBarUiControl { get; private set; }

        public IDbMaintenanceVisualView VisualView { get; private set; }

        /// <summary>
        /// The key automatic fill control UI control
        /// </summary>
        private VmUiControl _keyAutoFillControlUiControl;
        /// <summary>
        /// The register key control
        /// </summary>
        private bool _registerKeyControl;
        /// <summary>
        /// The lookup add view arguments
        /// </summary>
        private LookupAddViewArgs _lookupAddViewArgs;

        /// <summary>
        /// Setups the control.
        /// </summary>
        protected virtual void SetupControl()
        {
            if (PreviousButton != null)
            {
                PreviousButton.Command = ViewModel.PreviousCommand;
                PreviousButtonUiControl = new VmUiControl(PreviousButton, ViewModel.PreviousUiCommand);
            }

            if (NewButton != null)
            {
                NewButton.Command = ViewModel.NewCommand;
                NewButtonUiControl = new VmUiControl(NewButton, ViewModel.NewUiCommand);
            }

            if (SaveButton != null)
            {
                SaveButton.Command = ViewModel.SaveCommand;
                SaveButtonUiControl = new VmUiControl(SaveButton, ViewModel.SaveUiCommand);
            }

            if (DeleteButton != null)
            {
                DeleteButton.Command = ViewModel.DeleteCommand;
                DeleteButtonUiControl = new VmUiControl(DeleteButton, ViewModel.DeleteUiCommand);
            }

            if (FindButton != null)
            {
                FindButton.Command = ViewModel.FindCommand;
                FindButtonUiControl = new VmUiControl(FindButton, ViewModel.FindUiCommand);
            }

            if (SelectButton != null)
            {
                SelectButton.Command = ViewModel.SelectCommand;
                SelectButtonUiControl = new VmUiControl(SelectButton, ViewModel.SelectUiCommand);
            }

            if (NextButton != null)
            {
                NextButton.Command = ViewModel.NextCommand;
                NextButtonUiControl = new VmUiControl(NextButton, ViewModel.NextUiCommand);
            }

            if (PrintButton != null)
            {
                PrintButton.Command = ViewModel.PrintCommand;
                PrintButtonUiControl = new VmUiControl(PrintButton, ViewModel.PrintUiCommand);
            }

            if (CloseButton != null)
            {
                CloseButton.Click += (_, _) => CloseWindow();
            }

            VisualView.ShowInTaskbar = false;
            VisualView.EnterToTab = true;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceWindow.Loaded += (sender, args) =>
            {
                ViewModel.OnViewLoaded(LibView);
            };
            MaintenanceWindow.PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;
            //MaintenanceWindow.Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public virtual void CloseWindow()
        {
            VisualView.Close();
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the DbMaintenanceWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void DbMaintenanceWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        ViewModel.OnGotoPreviousButton();
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ViewModel.OnGotoNextButton();
                        e.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="buttonsControl">The buttons control.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="libView">The view.</param>
        /// <param name="statusBar">The status bar.</param>
        public virtual void Initialize(BaseWindow window, Control buttonsControl, DbMaintenanceViewModelBase viewModel,
            IDbMaintenanceView view, DbMaintenanceStatusBar statusBar = null)
        {
            if (visualView == null)
            {
                throw new ArgumentNullException(nameof(visualView));
            }
            VisualView = visualView;
            MaintenanceWindow = visualView.MaintenanceWindow;

            if (buttonsControl == null)
            {
                throw new ArgumentNullException(nameof(buttonsControl));
            }
            MaintenanceButtonsControl = buttonsControl;

            if (viewModel == null)
            {
                throw new ArgumentException(nameof(viewModel));
            }
            ViewModel = viewModel;

            ViewModel.Processor  = this;

            if (libView == null)
            {
                throw new ArgumentException(nameof(libView));
            }
            LibView = libView;
            SetupControl();

            if (statusBar != null)
            {
                SetupStatusBar(viewModel, statusBar);
            }
            MaintenanceButtonsUiControl =
                new VmUiControl(MaintenanceButtonsControl, ViewModel.MaintenanceButtonsUiCommand);
            if (statusBar != null)
            {
                StatusBarUiControl = new VmUiControl(statusBar, ViewModel.StatusBarUiCommand);
            }

            if (_registerKeyControl && KeyAutoFillControl != null)
            {
                RegisterFormKeyControl(KeyAutoFillControl);
            }

            if (_lookupAddViewArgs != null)
            {
                InitializeFromLookupData(_lookupAddViewArgs);
                _lookupAddViewArgs = null;
            }
        }

        /// <summary>
        /// Setups the status bar.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="statusBar">The status bar.</param>
        public void SetupStatusBar(DbMaintenanceViewModelBase viewModel, DbMaintenanceStatusBar statusBar)
        {
            if (statusBar == null)
            {
                return;
            }
            StatusBar = statusBar;
            BindingOperations.SetBinding(statusBar, DbMaintenanceStatusBar.LastSavedDateProperty, new Binding
            {
                Source = viewModel,
                Path = new PropertyPath(nameof(ViewModel.LastSavedDate)),
                Mode = BindingMode.TwoWay
            });

        }

        /// <summary>
        /// Sets the save status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="alertLevel">The alert level.</param>
        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            if (StatusBar != null)
            {
                StatusBar.SetSaveStatus(message, alertLevel);
            }
        }

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public List<DbAutoFillMap> GetAutoFills()
        {
            if (LibView is Window window)
            {
                return LookupControlsGlobals.GetAutoFills(window);
            }
            return null;
        }

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            if (LibView is Window window)
            {
                LookupControlsGlobals.HandleValFail(window, autoFillMap);
            }
        }

        /// <summary>
        /// Registers the form key control.
        /// </summary>
        /// <param name="keyAutoFillControl">The key automatic fill control.</param>
        public virtual void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            KeyAutoFillControl = keyAutoFillControl;
            if (ViewModel == null)
            {
                _registerKeyControl = true;
                return;
            }
            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.IsDirtyProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyValueDirty)),
                Mode = BindingMode.TwoWay
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.SetupProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillSetup))
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.ValueProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillValue)),
                Mode = BindingMode.TwoWay
            });

            _keyAutoFillControlUiControl = new VmUiControl(keyAutoFillControl, ViewModel.KeyAutoFillUiCommand);
            keyAutoFillControl.UiCommand = _keyAutoFillControlUiControl.Command;

            keyAutoFillControl.AutoFillLostFocus += (sender, args) => ViewModel.OnKeyControlLeave();
            KeyAutoFillControl.SetReadOnlyMode(false);
            KeyControlRegistered = true;
        }

        public bool PreDeleteResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [key control registered].
        /// </summary>
        /// <value><c>true</c> if [key control registered]; otherwise, <c>false</c>.</value>
        public bool KeyControlRegistered { get; set; }
        /// <summary>
        /// Occurs when [lookup add view].
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        /// <summary>
        /// Initializes from lookup data.
        /// </summary>
        /// <param name="e">The e.</param>
        public virtual void InitializeFromLookupData(LookupAddViewArgs e)
        {
            if (ViewModel == null)
            {
                _lookupAddViewArgs = e;
                return;
            }
            ViewModel.InitializeFromLookupData(e);
            LookupAddView?.Invoke(this, e);
            if (e.LookupReadOnlyMode)
            {
                SelectButton.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        public virtual void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(MaintenanceWindow) is TextBox textBox)
            {
                var lookupControl = textBox.GetParentOfType<LookupControl>();
                if (lookupControl == null)
                    textBox.SelectAll();
            }
        }

        /// <summary>
        /// Occurs when [debug show find].
        /// </summary>
        public event EventHandler DebugShowFind;

        /// <summary>
        /// Shows the find lookup window.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <param name="initialSearchForPrimaryKey">The initial search for primary key.</param>
        public virtual void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(lookupDefinition,
                allowAdd, allowView, initialSearchFor, initialSearchForPrimaryKey);

            //lookupWindow.InitialSearchForPrimaryKeyValue = initialSearchForPrimaryKey;

            lookupWindow.LookupSelect += (sender, args) =>
            {
                ViewModel.OnRecordSelected(args);
            };
            lookupWindow.Owner = Window.GetWindow(MaintenanceWindow);
            lookupWindow.AddViewParameter = ViewModel?.InputParameter;
            lookupWindow.ApplyNewLookup += (sender, args) =>
                ViewModel.FindButtonLookupDefinition = lookupWindow.LookupDefinition;

            DebugShowFind?.Invoke(this, EventArgs.Empty);
            bool isAltDown = IsMaintenanceKeyDown(MaintenanceKey.Alt);
            lookupWindow.Closed += (sender, args) =>
            {
                MaintenanceWindow.Focus();
                if (!isAltDown)
                {
                    LibView.ResetViewForNewRecord();
                }
            };
            lookupWindow.Show();
        }

        /// <summary>
        /// Shows the yes no cancel message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageButtons.</returns>
        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            var result = MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return MessageButtons.Yes;
                case MessageBoxResult.No:
                    return MessageButtons.No;
            }

            return MessageButtons.Cancel;
        }

        /// <summary>
        /// Shows the yes no message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            if (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                return true;

            return false;
        }

        /// <summary>
        /// Shows the record saved message.
        /// </summary>
        public virtual void ShowRecordSavedMessage()
        {
            MessageBox.Show("Record Saved!", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public virtual void SetReadOnlyMode(bool readOnlyValue)
        {
            
        }

        /// <summary>
        /// Called when [record select].
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual void OnRecordSelect(LookupSelectArgs args)
        {
            OnRecordSelected();
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (MaintenanceWindow != null && MaintenanceButtonsControl != null)
            {
                if (readOnlyValue)
                {
                    var focusedElement = FocusManager.GetFocusedElement(MaintenanceWindow);
                    if (focusedElement == null || !focusedElement.IsEnabled)
                        NextButton.Focus();
                }
                else if (MaintenanceButtonsControl.IsKeyboardFocusWithin)
                {
                    WPFControlsGlobals.SendKey(Key.Tab);
                }
            }
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            if (MaintenanceWindow != null)
            {
                var window = Window.GetWindow(MaintenanceWindow);
                window.Activate();
                MaintenanceWindow.Focus();
                WPFControlsGlobals.SendKey(Key.Tab);
            }
        }

        /// <summary>
        /// Sets the control read only mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == KeyAutoFillControl)
            {
                return false;
            }
            else if (control == StatusBar)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the add on fly mode.
        /// </summary>
        public void CheckAddOnFlyMode()
        {
            
        }

        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            var delProcedure = new DeleteProcedure(Window.GetWindow(MaintenanceWindow)
                , "Deleting Table Data"
                , ViewModel
                , deleteTables);
            delProcedure.Start();
            return delProcedure;
        }

        public void GetPreDeleteProcedure(
            List<FieldDefinition> fields
            , DeleteTables deleteTables)
        {
            var delProcedure = new PreDeleteProcedure(Window.GetWindow(MaintenanceWindow)
                , "Gathering Tables To Delete"
                , ViewModel
                , deleteTables
                , fields
                , this);

            delProcedure.Start();
        }

        public void SetWindowReadOnlyMode(bool readOnlyMode)
        {
            VisualView.SetReadOnlyMode(readOnlyMode);
        }

        public bool DeleteChildrenResult { get; set; }

        /// <summary>
        /// Checks the delete tables.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            var deleteWindow = new DeleteRecordWindow(deleteTables);
            deleteWindow.Owner = Window.GetWindow(MaintenanceWindow);
            deleteWindow.ShowInTaskbar = false;
            deleteWindow.ShowDialog();
            if (deleteWindow.DialogResult.HasValue)
            {
                return deleteWindow.DialogResult.Value;
            }
            return false;
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            var filterWindow = new GenericReportFilterWindow(printerSetupArgs);
            filterWindow.Owner = Window.GetWindow(MaintenanceWindow);
            filterWindow.ShowInTaskbar = false;
            filterWindow.ShowDialog();
        }

        /// <summary>
        /// Sets the window read only mode.
        /// </summary>
        public virtual void SetWindowReadOnlyMode()
        {
            SelectButton.IsEnabled = false;
            ViewModel.SaveButtonEnabled = false;
            ViewModel.DeleteButtonEnabled = false;
            ViewModel.NewButtonEnabled = false;
        }

        /// <summary>
        /// Shows the record lock window.
        /// </summary>
        /// <param name="lockKey">The lock key.</param>
        /// <param name="message">The message.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            var recordLockInputParameter = new RecordLockingInputParameter
            {
                InputParameter = inputParameter,
                RecordLockMessage = message
            };

            var lookupDefinition = SystemGlobals.AdvancedFindLookupContext.RecordLockingLookup.Clone();

            lookupDefinition.ShowAddOnTheFlyWindow(string.Empty, MaintenanceWindow, null, recordLockInputParameter,
                lockKey);
            
            return recordLockInputParameter.ContinueSave;
        }

        /// <summary>
        /// Determines whether [is maintenance key down] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is maintenance key down] [the specified key]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">key - null</exception>
        public bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            switch (key)
            {
                case MaintenanceKey.Alt:
                    return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
                case MaintenanceKey.Ctrl:
                    return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }

        private void PreSetButton(Button button)
        {

        }
    }
}
