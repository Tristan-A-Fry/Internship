using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Purpose of IRepository.cs
    Abstraction of Data Access Logic:

    It abstracts the data access logic from the application logic, promoting a separation of concerns. This makes the code cleaner and more maintainable.
    Reusability:

    It allows the same repository methods to be used for different entities, promoting code reuse.


How IRepository.cs Works
    Generic Type Parameter <T>:

    The interface uses a generic type parameter <T> which allows it to be used with any entity class.
    CRUD Operation Methods:

    The interface defines methods for common CRUD operations: GetAllAsync, GetByIdAsync, UpsertAsync, and DeleteAsync.

*/




namespace exercise1prob2.Services
{
public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int skip, int take);
        Task<T> GetByIdAsync(int id);
        Task<T> UpsertAsync(T Entity);
        Task DeleteAsync(int id);
    }
}