using Moq;
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;
using SemanticMocking.SampleTests.App;

namespace SemanticMocking.SampleTests.Mocks;

public class DialogServiceMock : MoqMock<
    IDialogService, 
    DialogServiceMock.Arrangements, 
    DialogServiceMock.Assertions, 
    DialogServiceMock.Raises>
{
    public class Arrangements : ArrangementsFor<DialogServiceMock> { }
   
    public class Assertions : AssertionsFor<DialogServiceMock>
    {
        public Assertions DidShowErrorAlert()
        {
            Parent.Mock
                .Verify(mock => mock.ShowAlertAsync("Error", It.IsAny<string>()));
            return this;
        }
    }
    
    public class Raises : RaisesFor<DialogServiceMock> { }
}