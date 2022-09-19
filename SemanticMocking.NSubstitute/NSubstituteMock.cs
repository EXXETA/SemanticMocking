using SemanticMocking.Abstractions;
using NSubstitute;

namespace SemanticMocking.NSubstitute;

public abstract class NSubstituteMock<TInterface, TArrange, TAssert, TRaise> : MockBase<TInterface, TArrange, TAssert, TRaise>
    where TInterface : class
    where TArrange : IMockBehaviour, new()
    where TAssert : IMockBehaviour, new()
    where TRaise : IMockBehaviour, new()
{
    protected NSubstituteMock()
    {
        SetMock(Substitute.For<TInterface>());
    }
}