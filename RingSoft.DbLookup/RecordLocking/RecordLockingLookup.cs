using System;

namespace RingSoft.DbLookup.RecordLocking
{
    public class RecordLockingLookup
    {
        public string Table { get; set; }

        public DateTime LockDate { get; set; }

        public string User { get; set; }
    }
}
