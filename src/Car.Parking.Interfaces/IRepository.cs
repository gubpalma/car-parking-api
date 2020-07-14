using System.Linq;

namespace Car.Parking.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Query();

        T AddOrUpdate(T record);
    }
}
