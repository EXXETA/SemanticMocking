using System.Collections.Generic;

namespace SemanticMocking.SampleTests.App;

public interface IDataProvider
{
    DataRecord[] GetRecords();
}