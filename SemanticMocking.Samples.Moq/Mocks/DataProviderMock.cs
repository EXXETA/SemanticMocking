using System;
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;
using SemanticMocking.SampleTests.App;

namespace SemanticMocking.SampleTests.Mocks;

public class DataProviderMock : MoqMock<
    IDataProvider, 
    DataProviderMock.Arrangements, 
    DataProviderMock.Assertions, 
    DataProviderMock.Raises>
{
    public class Arrangements : ArrangementsFor<DataProviderMock>
    {
        public Arrangements GetRecordsFailsWith(Exception exception)
        {
            Parent.Mock
                .Setup(mock => mock.GetRecords())
                .Throws(exception);
            return this;
        }

        public Arrangements GetRecordsReturns(DataRecord[] records)
        {
            Parent.Mock
                .Setup(mock => mock.GetRecords())
                .Returns(records);
            return this;
        }


        public Arrangements HasNoRecordsYet()
        {
            return GetRecordsReturns(Array.Empty<DataRecord>());
        }
    }
   
    public class Assertions : AssertionsFor<DataProviderMock>
    {
    }

    public class Raises : RaisesFor<DataProviderMock>
    {
    }
}