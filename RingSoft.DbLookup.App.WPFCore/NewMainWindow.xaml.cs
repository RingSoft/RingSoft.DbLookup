﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow : ILookupAddViewDestination
    {
        private VmUiControl _lookupUiControl;
        public NewMainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize();
                //var uControl = new OrdersGridUserControl();
                //tabItem = new DbMaintenanceTabItem(
                //    uControl, TabControl);
                //TabControl.Items.Add(tabItem);
                //tabItem.IsSelected = true;

                LocalViewModel.OrderLookupDefinition.Destination = this;
                _lookupUiControl = new VmUiControl(OrderLookup, LocalViewModel.LookupUiCommand);

                _lookupUiControl.Command.SetFocus();
                //uControl.Loaded += (sender, args) =>
                //{
                //    _lookupUiControl.Command.SetFocus();
                //};
            };
        }

        public void ShowAddView(LookupAddViewArgs addViewArgs, object inputParameter = null)
        {
            if (LookupControlsGlobals.WindowRegistry.IsTableRegistered(addViewArgs.LookupData.LookupDefinition.TableDefinition))
            {
                var ucControl = LookupControlsGlobals.WindowRegistry.GetUserControl(
                    addViewArgs.LookupData.LookupDefinition.TableDefinition);
                if (ucControl != null)
                {
                    ucControl.LookupAddViewArgs = addViewArgs;
                    ucControl.AddViewParameter = inputParameter;
                    var tabItem = new DbMaintenanceTabItem(
                        ucControl, TabControl);
                    TabControl.Items.Insert(0, tabItem);
                    tabItem.IsSelected = true;
                    ucControl.Focus();
                    ucControl.SetInitialFocus();
                }
            }
        }
    }
}
