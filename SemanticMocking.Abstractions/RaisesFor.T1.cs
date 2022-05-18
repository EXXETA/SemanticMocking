namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for raising events of the mock that activate a reaction in the system under test.
    /// </summary>
    /// <typeparam name="TParent">The parent mock wrapper object.</typeparam>
    public class RaisesFor<TParent> : MockBehaviour<TParent>
    {
    }
}