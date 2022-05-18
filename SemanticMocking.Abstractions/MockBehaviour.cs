namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for raising events of the mock that activate a reaction in the system under test.
    /// </summary>
    /// <typeparam name="TParent">The parent mock object.</typeparam>
    /// <typeparam name="TMock">The actual mocking object of the given mocking framework.</typeparam>
    public abstract class MockBehaviour<TParent, TMock> : IMockBehaviour
        where TMock: class
    {
        protected TParent Parent { get; init; }
        protected TMock Mock { get; set; }
    }
}