using System;
using System.Collections.Generic;
using System.Linq;
using WEHEMO.DTO;

namespace WEHEMO.Business
{
    public class Test : ITest
    {
        public void Add(Guid customerId, string url)
        {
            var test = new TEST();

            test.ID = Guid.NewGuid();
            test.CUSTOMER_ID = customerId;
            test.URL = url;
            test.CREATE_DATE = DateTime.Now;

            using (var dc = new WEHEMODataContext())
            {
                dc.TESTs.InsertOnSubmit(test);

                dc.SubmitChanges();
            }
        }

        public void AddTestResult(Guid testId, int statusCode)
        {
            var testResult = new TEST_RESULT();

            testResult.ID = Guid.NewGuid();
            testResult.TEST_ID = testId;
            testResult.STATUS_CODE = statusCode;
            testResult.CREATE_DATE = DateTime.Now;

            using (var dc = new WEHEMODataContext())
            {
                dc.TEST_RESULTs.InsertOnSubmit(testResult);

                dc.SubmitChanges();
            }
        }

        public void Delete(Guid testId)
        {
            using (var dc = new WEHEMODataContext())
            {
                var test = dc.TESTs.Where(c => c.ID == testId).FirstOrDefault();

                if (test == null)
                {
                    return;
                }

                test.DELETED = true;

                dc.SubmitChanges();
            }
        }

        public Dictionary<Guid, string> List()
        {
            using (var dc = new WEHEMODataContext())
            {
                return dc.TESTs.Where(c => !c.DELETED).Select(c => new { c.ID, c.URL }).ToDictionary(c => c.ID, c => c.URL);
            }
        }

        public DTO_TEST[] List(Guid customerId)
        {

            using (var dc = new WEHEMODataContext())
            {

                var list1 = (from c in dc.TEST_RESULTs
                             group c by c.TEST_ID into g
                             select new
                             {
                                 TEST_ID = g.Key,
                                 CREATE_DATE = g.Max(c => c.CREATE_DATE)
                             });

                var list2 = (from c in dc.TEST_RESULTs
                             join d in list1 on new { c.TEST_ID, c.CREATE_DATE } equals new { d.TEST_ID, d.CREATE_DATE }
                             select c).ToList();

                return (from c in dc.TESTs
                        join tr in dc.TEST_RESULTs on c.ID equals tr.TEST_ID into g
                        from temp in g.DefaultIfEmpty()
                        join s in dc.STATUS_CODEs on temp.STATUS_CODE equals s.CODE
                        where c.CUSTOMER_ID == customerId
                        orderby temp.CREATE_DATE descending
                        select new DTO_TEST
                        {
                            ID = c.ID,
                            URL = c.URL,
                            STATUS = temp.STATUS_CODE == 200 ? true : false,
                            STATUS_DESCRIPTION = s.DESCRIPTION,
                            DATE = temp.CREATE_DATE,
                            CREATE_DATE = c.CREATE_DATE
                        }).ToArray();
            }
        }

        public DTO_TEST_RESULT[] TestDetail(Guid testId)
        {
            using (var dc = new WEHEMODataContext())
            {
                return (from c in dc.TEST_RESULTs
                        join s in dc.STATUS_CODEs on c.STATUS_CODE equals s.CODE
                        where c.TEST_ID == testId
                        orderby c.CREATE_DATE descending
                        select new DTO_TEST_RESULT
                        {
                            STATUS = c.STATUS_CODE == 200 ? true : false,
                            DATE = c.CREATE_DATE,
                            DESCRIPTION = s.DESCRIPTION
                        }).ToArray();
            }
        }
    }
}
