using System;
using RingSoft.DbLookup.DataProcessor;

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
    }

    internal class DefaultUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Console.WriteLine(dataProcessResult.Message);
        }
    }
}
