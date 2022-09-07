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
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind;assembly=RingSoft.DbLookup.Controls.WPF.AdvancedFind"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AdvancedFindRefreshRateWindow/>
    ///
    /// </summary>
    public class AdvancedFindRefreshRateWindow : BaseWindow
    {
        public Border Border { get; set; }

        public Image YellowAlertImage { get; set; }

        public Image RedAlertImage { get; set; }

        public Button OkButton { get; set; }

        public Button CancelButton { get; set; }

        public AdvancedFindRefreshViewModel ViewModel { get; set; }

        static AdvancedFindRefreshRateWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindRefreshRateWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindRefreshRateWindow)));
        }

        public AdvancedFindRefreshRateWindow(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(advancedFind);
                var yellowAlertImage =
                    LookupControlsGlobals.LookupControlContentTemplateFactory.GetImageForAlertLevel(AlertLevels.Yellow);

                var redAlertImage =
                    LookupControlsGlobals.LookupControlContentTemplateFactory.GetImageForAlertLevel(AlertLevels.Red);

                YellowAlertImage.Source = yellowAlertImage.Source;
                RedAlertImage.Source = redAlertImage.Source;

                OkButton.Click += (o, eventArgs) =>
                {
                    ViewModel.RefreshProperties();
                    DialogResult = true;
                    Close();
                };

                CancelButton.Click += (o, eventArgs) => Close();
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            YellowAlertImage = GetTemplateChild(nameof(YellowAlertImage)) as Image;
            RedAlertImage = GetTemplateChild(nameof(RedAlertImage)) as Image;
            OkButton = GetTemplateChild(nameof(OkButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            ViewModel = Border.TryFindResource("AdvancedFindRefreshViewModel") as AdvancedFindRefreshViewModel;

            base.OnApplyTemplate();
        }
    }
}
