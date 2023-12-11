// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="AddOnTheFlyProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class NewAddOnTheFlyResult.
    /// </summary>
    public class NewAddOnTheFlyResult
    {
        /// <summary>
        /// Creates new primarykeyvalue.
        /// </summary>
        /// <value>The new primary key value.</value>
        public PrimaryKeyValue NewPrimaryKeyValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewAddOnTheFlyResult"/> class.
        /// </summary>
        /// <param name="newPrimaryKeyValue">The new primary key value.</param>
        internal NewAddOnTheFlyResult(PrimaryKeyValue newPrimaryKeyValue)
        {
            NewPrimaryKeyValue = newPrimaryKeyValue;
        }
    }

    /// <summary>
    /// Class AddOnTheFlyProcessor.
    /// Implements the <see cref="ILookupControl" />
    /// </summary>
    /// <seealso cref="ILookupControl" />
    internal class AddOnTheFlyProcessor : ILookupControl
    {
        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>The number of rows on the page.</value>
        public int PageSize => 1;
        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText { get; set; }
        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex => 0;
        /// <summary>
        /// Sets the index of the lookup.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetLookupIndex(int index)
        {
            
        }

        /// <summary>
        /// Gets or sets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; set; }

        /// <summary>
        /// Gets or sets the owner window.
        /// </summary>
        /// <value>The owner window.</value>
        public object OwnerWindow { get; set; }

        /// <summary>
        /// Creates new primarykeyvalue.
        /// </summary>
        /// <value>The new primary key value.</value>
        public PrimaryKeyValue NewPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets or sets the selected primary key value.
        /// </summary>
        /// <value>The selected primary key value.</value>
        public PrimaryKeyValue SelectedPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOnTheFlyProcessor"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public AddOnTheFlyProcessor(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        /// <summary>
        /// Setups the processor.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <returns>LookupAddViewArgs.</returns>
        public LookupAddViewArgs SetupProcessor(LookupDataMauiBase lookupData)
        {
            LookupAddViewArgs args = null;
            if (SelectedPrimaryKeyValue == null)
            {
                args = new LookupAddViewArgs(lookupData, false, LookupFormModes.Add,
                    SearchText, OwnerWindow)
                {
                    NewRecordPrimaryKeyValue = NewPrimaryKeyValue,
                    InputParameter = AddViewParameter
                };
            }
            else
            {
                args = new LookupAddViewArgs(lookupData, false, LookupFormModes.View,
                    string.Empty, OwnerWindow)
                {
                    SelectedPrimaryKeyValue = SelectedPrimaryKeyValue,
                    InputParameter = AddViewParameter
                };
                //args.LookupData.SetNewPrimaryKeyValue(SelectedPrimaryKeyValue);
                args.LookupData.SelectedPrimaryKeyValue = SelectedPrimaryKeyValue;

            }

            args.CallBackToken.RefreshData += (sender, eventArgs) =>
            {
                LookupDefinition.FireCloseEvent(lookupData);
                if (lookupData.SelectedPrimaryKeyValue == null || !lookupData.SelectedPrimaryKeyValue.IsValid())
                {
                    lookupData.RefreshData();
                }
            };
            return args;
        }

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        public void ShowAddOnTheFlyWindow()
        {
            //var lookupData = new LookupDataBase(LookupDefinition, this);
            var lookupData = LookupDefinition.TableDefinition.LookupDefinition
                .GetLookupDataMaui(LookupDefinition, true);
            //lookupData.LookupView += (sender, viewArgs) =>
            //{
            //    viewArgs.Handled = true;
            //};
            lookupData.SetParentControls(this);
            var args = SetupProcessor(lookupData);
            LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }
    }

    /// <summary>
    /// Class AddOnTheFlyProcessor.
    /// Implements the <see cref="RingSoft.DbLookup.AddOnTheFlyProcessor" />
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the t lookup entity.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.AddOnTheFlyProcessor" />
    internal class AddOnTheFlyProcessor<TLookupEntity, TEntity> : AddOnTheFlyProcessor
        where TLookupEntity : new() where TEntity : class, new()
    {
        /// <summary>
        /// The lookup definition
        /// </summary>
        private LookupDefinition<TLookupEntity, TEntity> _lookupDefinition;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddOnTheFlyProcessor{TLookupEntity, TEntity}"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="newText">The new text.</param>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="newRecordPrimaryKeyValue">The new record primary key value.</param>
        /// <param name="selectedPrimaryKeyValue">The selected primary key value.</param>
        public AddOnTheFlyProcessor(LookupDefinition<TLookupEntity, TEntity> lookupDefinition, string newText,
            object ownerWindow, PrimaryKeyValue newRecordPrimaryKeyValue = null,
            PrimaryKeyValue selectedPrimaryKeyValue = null) : base(lookupDefinition)
        {
            SearchText = newText;
            OwnerWindow = ownerWindow;
            NewPrimaryKeyValue = newRecordPrimaryKeyValue;
            SelectedPrimaryKeyValue = selectedPrimaryKeyValue;
            _lookupDefinition = lookupDefinition;
        }

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <returns>NewAddOnTheFlyResult.</returns>
        public NewAddOnTheFlyResult ShowAddOnTheFlyWindow()
        {
            var lookupData =
                _lookupDefinition
                    .TableDefinition
                    .LookupDefinition
                    .GetLookupDataMaui(_lookupDefinition, true);
            lookupData.SetParentControls(this);
            
            //var lookupData = new LookupData<TLookupEntity, TEntity>(_lookupDefinition, this);
            lookupData.LookupView += (sender, viewArgs) => { viewArgs.Handled = true; };
            var args = SetupProcessor(lookupData);
            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);

            var result = new NewAddOnTheFlyResult(lookupData.SelectedPrimaryKeyValue);
            return result;
        }
    }
}
