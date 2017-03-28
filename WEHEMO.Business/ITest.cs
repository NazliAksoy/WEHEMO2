using System;
using System.Collections.Generic;
using WEHEMO.DTO;

namespace WEHEMO.Business
{
    public interface ITest
    {
        void Add(Guid customerId, string url);

        Dictionary<Guid, string> List();

        DTO_TEST[] List(Guid customerId);

        void Delete(Guid testId);

        void AddTestResult(Guid testId, int statusCode);

        DTO_TEST_RESULT[] TestDetail(Guid testId);
    }
}
