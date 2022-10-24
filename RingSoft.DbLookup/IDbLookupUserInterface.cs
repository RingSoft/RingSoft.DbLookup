using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Implement this to so DbLookup classes can interact with the user interface.
    /// </summary>
    public interface IDbLookupUserInterface
    {
        /// <summary>
        /// Shows the data process execution result.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
        void ShowDataProcessResult(DataProcessResult dataProcessResult);

        void ShowAddOnTheFlyWindow(LookupAddViewArgs e);

        void PlaySystemSound(RsMessageBoxIcons icon);
    }

    internal class DefaultUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Console.WriteLine(dataProcessResult.Message);
        }

        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            
        }

        public void PlaySystemSound(RsMessageBoxIcons icon)
        {
            
        }
    }
}
