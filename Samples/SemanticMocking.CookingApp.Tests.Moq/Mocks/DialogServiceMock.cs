using Moq;
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;

namespace SemanticMocking.CookingApp.Tests.Moq.Mocks;

public class DialogServiceMock : MoqMock<
    IDialogService, 
    DialogServiceMock.Arrangements, 
    DialogServiceMock.Assertions, 
    NoBehaviour>
{
    public class Arrangements : BehaviourFor<DialogServiceMock> { }
   
    public class Assertions : BehaviourFor<DialogServiceMock>
    {
        public Assertions DidShowError(string? message = null)
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
        
        public Assertions DidShowInfo(string message)
        {
            Parent.Mock
                .Verify(mock => mock.ShowMessageAsync("Info", message));
            return this;
        }
    }
}