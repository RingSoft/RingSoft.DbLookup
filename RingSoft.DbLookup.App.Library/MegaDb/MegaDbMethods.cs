using System;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.MegaDb
{
    public class MegaDbMethods
    {
        public static async Task<int> SeedItemsTable(IMegaDbDbContext context, IMegaDbEfDataProcessor processor,
            int maxRecords, CancellationToken token)
        {
            var result = 0;
            var insertResult = true;
            
            await Task.Run(async () =>
            {
                var numFormat = GblMethods.GetNumFormat(0, false);

                var locationId = 1;
                var manufacturerId = 1;
                var base36CharArray = new[] { '0','1','2','3','4','5','6','7','8','9',
                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
                for (int i = 0; i < maxRecords; i++)
                {
                    var item = ItemTableDbSeederCreateItem(base36CharArray, i, ref locationId, ref manufacturerId);

                    context.AddItem(item);

                    if (i % 100 == 0)
                    {
                        var message = $@"Inserting {i.ToString(numFormat)} of {maxRecords.ToString(numFormat)} Records";
                        processor.OnItemsTableSeederProgress(
                            new ItemsTableSeederProgressArgs(i, maxRecords, message, true));
                        result += context.SaveBatch();
                    }

                    if (token.IsCancellationRequested)
                    {
                        insertResult = false;
                        break;
                    }
                }

                if (insertResult)
                {
                    processor.OnItemsTableSeederProgress(new ItemsTableSeederProgressArgs(maxRecords, maxRecords,
                        "Saving Changes", true));
                    result += await context.SaveChangesAsync(token);
                }
            }, token);

            return result;
        }

        public static Item ItemTableDbSeederCreateItem(char[] base36CharArray, int index, ref int locationId, ref int manufacturerId)
        {
            var prefix = IntToString(index, base36CharArray);
            var itemName = $"{prefix}ITEM{index}";
            var item = new Item
            {
                Name = itemName,
                LocationId = locationId,
                ManufacturerId = manufacturerId
            };

            locationId++;
            if (locationId > 11)
                locationId = 1;

            manufacturerId++;
            if (manufacturerId > 9)
                manufacturerId = 1;

            return item;
        }

        /// <summary>
        /// An optimized method using an array as buffer instead of 
        /// string concatenation. This is faster for return values having 
        /// a length > 1.
        /// </summary>
        public static string IntToString(int value, char[] baseChars)
        {
            // 32 is the worst cast buffer size for base 2 and int.MaxValue
            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[value % targetBase];
                value = value / targetBase;
            }
            while (value > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            return new string(result);
        }
    }
}
