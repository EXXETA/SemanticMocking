using Moq;
using SemanticMocking.Abstractions;

namespace SemanticMocking.Moq
{
    /// <summary>
    /// Base class for arrangements of a mocked dependency to prepare the expected
    /// interactions with the system under test.
    /// </summary>
    /// /// <typeparam name="TParent">The parent mock wrapper object.</typeparam>
    /// <typeparam name="TInterface">The interface that should be mocked.</typeparam>
    public class ArrangementsFor<TParent, TInterface>: MockBehaviour<TParent, Mock<TInterface>>
        where TParent : class
        where TInterface : class
    {
    }
}