namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for implementing specific behaviours of the mock.
    /// </summary>
    /// <typeparam name="TParent">The parent mock object.</typeparam>
    public abstract class MockBehaviour<TParent> : IMockBehaviour
    {
        protected TParent Parent { get; init; }
    }
}