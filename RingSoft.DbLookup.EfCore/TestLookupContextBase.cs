using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.Testing;

namespace RingSoft.DbLookup.EfCore
{
    public abstract class TestLookupContextBase : LookupContext
    {
        public TestDataRepository DataRepository { get; private set; }

        protected override DbContext DbContext => _context;

        private DbContext _context;

        public TestLookupContextBase(DbContext context)
        {
            DataRepository = new TestDataRepository(new DataRepositoryRegistry());
            DataRepository.Initialize();
            _context = context;
            Initialize();
        }

    }
}
