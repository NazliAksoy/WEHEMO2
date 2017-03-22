using System;

namespace WEHEMO.Business
{
    public interface ICustomer
    {
        Guid Add(string name, string email, string password);

        void Update(Guid cutomerId, string name, string email, string password);

        Guid? Login(string email, string password);

        void Delete(Guid customerId);
    }
}
