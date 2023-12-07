
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RingSoft.DbLookup.App.Library.MegaDb
{

    public class ItemsTableSeederProgressArgs
    {
        public int CurrentRecord { get; }

        public int MaxRecords { get; }

        public string Message { get; }

        public bool AllowCancel { get; }

        public ItemsTableSeederProgressArgs(int currentRecord, int maxRecords, string message, bool allowCancel)
        {
            CurrentRecord = currentRecord;
            MaxRecords = maxRecords;
            Message = message;
            AllowCancel = allowCancel;
        }
    }
    public interface IMegaDbEfDataProcessor
    {
        event EventHandler<ItemsTableSeederProgressArgs> ItemsTableSeederProgress;

        void SetDataContext();

        Item GetItem(int itemId);

        Task<int> SeedItemsTable(int maxRecords, CancellationToken token);

        void OnItemsTableSeederProgress(ItemsTableSeederProgressArgs e);

        bool DoesItemsTableHaveData();
    }
}
