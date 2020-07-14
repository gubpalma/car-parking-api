using System;
using System.Collections.Generic;
using System.Linq;

namespace Car.Parking.Infrastructure.Repository
{
    public abstract class BaseRepository<T>
    {
        private readonly ICollection<T> _table;

        private readonly Func<T, IComparable> _key;

        protected BaseRepository(ICollection<T> table, Func<T, IComparable> key)
        {
            _table = table;
            _key = key;
        }

        public IQueryable<T> Query() => _table.AsQueryable();

        public T AddOrUpdate(T record)
        {
            var existing = _table.FirstOrDefault(o => Equals(_key(o), _key(record)));

            if (existing != null)
                _table.Remove(existing);

            _table.Add(record);

            return record;
        }
    }
}
