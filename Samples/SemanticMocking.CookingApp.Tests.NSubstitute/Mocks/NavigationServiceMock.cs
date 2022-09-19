using NSubstitute;
using SemanticMocking.Abstractions;
using SemanticMocking.NSubstitute;

namespace SemanticMocking.CookingApp.Tests.NSubstitute.Mocks;

public class NavigationServiceMock : NSubstituteMock<INavigationService,
    NavigationServiceMock.Arrangements,
    NavigationServiceMock.Assertions,
    NavigationServiceMock.Raises>
{
    public class Arrangements : BehaviourFor<NavigationServiceMock>
    {
        
    }

    public class Assertions : BehaviourFor<NavigationServiceMock>
    {
        public Assertions DidNavigateBack()
        {
            Parent.Mock.Received().NavigateBack();
            return this;
        }
    }

    public class Raises : BehaviourFor<NavigationServiceMock>
    {
    }
}