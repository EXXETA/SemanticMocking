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
        public Assertions DidShowErrorAlert(string? message = null)
        {
            if (message == null)
            {
                Parent.Mock
                    .Verify(mock => mock.ShowMessageAsync("Error", It.IsAny<string>()));
            }
            else
            {
                Parent.Mock
                    .Verify(mock => mock.ShowMessageAsync("Error", message));
            }
            
            return this;
        }

        public Assertions DidShowMessage(string message)
        {
            Parent.Mock
                .Verify(mock => mock.ShowMessageAsync(It.IsAny<string>(), message));
            return this;
        }
    }
    
    public class Raises : RaisesFor<DialogServiceMock> { }
}