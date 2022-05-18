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
    public class Arrangements : ArrangementsFor<DialogServiceMock, IDialogService> { }
    public class Assertions : AssertionsFor<DialogServiceMock, IDialogService>
    {
        public Assertions DidShowErrorAlert()
        {
            Mock.Verify(mock => mock.ShowAlertAsync("Error", It.IsAny<string>()));
            return this;
        }
    }
    public class Raises : RaisesFor<DialogServiceMock,IDialogService> { }
}