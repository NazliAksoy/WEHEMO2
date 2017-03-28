using System;
using System.Linq;

namespace WEHEMO.Business
{
    public class Customer : ICustomer
    {
        public Guid Add(string name, string email, string password)
        {
            var customer = new CUSTOMER();

            customer.ID = Guid.NewGuid();
            customer.NAME = name;
            customer.EMAIL = email;
            customer.PASSWORD = password;
            customer.CREATE_DATE = DateTime.Now;
            customer.DELETED = false;

            using (var dc = new WEHEMODataContext())
            {
                dc.CUSTOMERs.InsertOnSubmit(customer);

                dc.SubmitChanges();
            }

            return customer.ID;
        }

        public void Update(Guid customerId, string name, string email, string password)
        {
            using (var dc = new WEHEMODataContext())
            {
                var customer = dc.CUSTOMERs.Where(c => c.ID == customerId).FirstOrDefault();

                if (customer == null)
                {
                    return;
                }

                customer.NAME = name;
                customer.EMAIL = email;
                customer.PASSWORD = password;

                dc.SubmitChanges();
            }
        }

        public void Delete(Guid customerId)
        {
            using (var dc = new WEHEMODataContext())
            {
                var customer = dc.CUSTOMERs.Where(c => c.ID == customerId).FirstOrDefault();

                if (customer == null)
                {
                    return;
                }

                customer.DELETED = true;

                dc.SubmitChanges();
            }
        }

        public Guid? Login(string email, string password)
        {
            using (var dc = new WEHEMODataContext())
            {
                var customer = dc.CUSTOMERs.Where(c => c.EMAIL == email && c.PASSWORD == password).FirstOrDefault();

                if (customer == null)
                {
                    return null;
                }

                return customer.ID;
            }
        }
    }
}
