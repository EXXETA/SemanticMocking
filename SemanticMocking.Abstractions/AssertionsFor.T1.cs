namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for arrangements of a mocked dependency to prepare the expected
    /// interactions with the system under test.
    /// </summary>
    /// <typeparam name="TParent">The parent mock wrapper object.</typeparam>
    public abstract class AssertionsFor<TParent>: MockBehaviour<TParent>
        where TParent : class
    {
    }
}