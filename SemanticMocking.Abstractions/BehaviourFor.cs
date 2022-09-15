namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for implementing specific behaviours of the mock.
    /// </summary>
    /// <typeparam name="TParent">The parent mock object.</typeparam>
    public abstract class BehaviourFor<TParent> : IMockBehaviour
    {
#pragma warning disable CS8618
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // Parent will be set via Reflection by MockBase class.
        protected TParent Parent { get; init; }
#pragma warning restore CS8618
    }
}