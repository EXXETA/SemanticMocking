using Moq;
using SemanticMocking.Abstractions;

namespace SemanticMocking.Moq
{
    /// <summary>
    /// Base class for raising events of the mock that activate a reaction in the system under test.
    /// </summary>
    /// <typeparam name="TParent">The parent mock wrapper object.</typeparam>
    /// <typeparam name="TInterface">The interface that should be mocked.</typeparam>
    public class RaisesFor<TParent, TInterface> : MockBehaviour<TParent, Mock<TInterface>>
        where TParent : class
        where TInterface : class
    {
    }
}