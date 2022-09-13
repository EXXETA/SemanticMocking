using SemanticMocking.Abstractions;
using SemanticMocking.Moq;

namespace SemanticMocking.CookingApp.Tests.Moq.Mocks;

public class NavigationServiceMock : MoqMock<INavigationService,
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
            Parent.Mock.Setup(mock => mock.NavigateBack());
            return this;
        }
    }

    public class Raises : BehaviourFor<NavigationServiceMock>
    {
    }
}