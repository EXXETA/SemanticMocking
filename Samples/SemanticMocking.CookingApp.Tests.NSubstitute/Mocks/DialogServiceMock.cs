using NSubstitute;
using SemanticMocking.Abstractions;
using SemanticMocking.NSubstitute;

namespace SemanticMocking.CookingApp.Tests.NSubstitute.Mocks;

public class DialogServiceMock : NSubstituteMock<
    IDialogService, 
    DialogServiceMock.Arrangements, 
    DialogServiceMock.Assertions, 
    DialogServiceMock.Raises>
{
    public class Arrangements : BehaviourFor<DialogServiceMock> { }
   
    public class Assertions : BehaviourFor<DialogServiceMock>
    {
        public Assertions DidShowError(string? message = null)
        {
            if (message == null)
            {
                Parent.Mock.Received().ShowMessageAsync("Error", Arg.Any<string>());
            }
            else
            {
                Parent.Mock.Received().ShowMessageAsync("Error", message);
            }
            
            return this;
        }

        public Assertions DidShowMessage(string message)
        {
            Parent.Mock.Received().ShowMessageAsync(Arg.Any<string>(), message);
            return this;
        }
        
        public Assertions DidShowInfo(string message)
        {
            Parent.Mock.Received().ShowMessageAsync("Info", message);
            return this;
        }
    }
    
    public class Raises : BehaviourFor<DialogServiceMock> { }
}