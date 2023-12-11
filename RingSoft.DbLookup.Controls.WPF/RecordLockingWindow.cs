// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 05-26-2023
// ***********************************************************************
// <copyright file="RecordLockingWindow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class RecordLockingWindow.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="IRecordLockingView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="IRecordLockingView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class RecordLockingWindow : BaseWindow, IRecordLockingView
    {
        /// <summary>
        /// Gets or sets the buttons panel.
        /// </summary>
        /// <value>The buttons panel.</value>
        public StackPanel ButtonsPanel { get; set; }
        /// <summary>
        /// Gets or sets the message panel.
        /// </summary>
        /// <value>The message panel.</value>
        public StackPanel MessagePanel { get; set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public RecordLockingViewModel ViewModel { get; set; }
        /// <summary>
        /// Gets or sets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public IDbMaintenanceProcessor Processor { get; set; }
        /// <summary>
        /// Gets or sets the user label.
        /// </summary>
        /// <value>The user label.</value>
        public Label UserLabel { get; set; }
        /// <summary>
        /// Gets or sets the user read only control.
        /// </summary>
        /// <value>The user read only control.</value>
        public StringReadOnlyBox UserReadOnlyControl { get; set; }
        /// <summary>
        /// Gets or sets the user automatic fill control.
        /// </summary>
        /// <value>The user automatic fill control.</value>
        public AutoFillReadOnlyControl UserAutoFillControl { get; set; }

        /// <summary>
        /// The buttons control
        /// </summary>
        private Control _buttonsControl;
        /// <summary>
        /// The add view arguments
        /// </summary>
        private LookupAddViewArgs _addViewArgs;

        /// <summary>
        /// Initializes static members of the <see cref="RecordLockingWindow"/> class.
        /// </summary>
        static RecordLockingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RecordLockingWindow), new FrameworkPropertyMetadata(typeof(RecordLockingWindow)));
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            _buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetRecordLockingButtonsControl(ViewModel);
            if (ButtonsPanel != null)
            {
                ButtonsPanel.Children.Add(_buttonsControl);
                ButtonsPanel.UpdateLayout();
            }
            ViewModel.View = this;
            Processor.Initialize(this, _buttonsControl, ViewModel, this);
            if (_addViewArgs != null)
            {
                Processor.InitializeFromLookupData(_addViewArgs);
            }

        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ButtonsPanel = GetTemplateChild(nameof(ButtonsPanel)) as StackPanel;
            ViewModel = Border.TryFindResource("RecordLockingViewModel") as RecordLockingViewModel;
            UserLabel = GetTemplateChild(nameof(UserLabel)) as Label;
            UserReadOnlyControl = GetTemplateChild(nameof(UserReadOnlyControl)) as StringReadOnlyBox;
            UserAutoFillControl = GetTemplateChild(nameof(UserAutoFillControl)) as AutoFillReadOnlyControl;
            MessagePanel = GetTemplateChild(nameof(MessagePanel)) as StackPanel;

            Initialize();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        /// <summary>
        /// Reset the view for new record.
        /// </summary>
        public void ResetViewForNewRecord()
        {
            
        }

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        /// <summary>
        /// Setups the view.
        /// </summary>
        public void SetupView()
        {
            UserLabel.Visibility = Visibility.Collapsed;
            UserReadOnlyControl.Visibility = Visibility.Collapsed;
            UserAutoFillControl.Visibility = Visibility.Collapsed;
            MessagePanel.Visibility = Visibility.Collapsed;

            if (!ViewModel.Message.IsNullOrEmpty())
            {
                MessagePanel.Visibility = Visibility.Visible;

                ButtonsPanel.Visibility = Visibility.Collapsed;
            }
            if (!ViewModel.UserName.IsNullOrEmpty())
            {
                UserLabel.Visibility = Visibility.Visible;
                UserReadOnlyControl.Visibility = Visibility.Visible;
            }
            else if (ViewModel.UserAutoFillSetup != null)
            {
                UserLabel.Visibility = Visibility.Visible;
                UserAutoFillControl.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            Close();
        }
    }
}
