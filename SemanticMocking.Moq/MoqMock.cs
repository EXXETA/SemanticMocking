using Moq;
using SemanticMocking.Abstractions;

namespace SemanticMocking.Moq;

public abstract class MoqMock<TInterface, TArrange, TAssert, TRaise> : MockBase<Mock<TInterface>, TArrange, TAssert, TRaise>
    where TInterface : class
    where TArrange : IMockBehaviour, new()
    where TAssert : IMockBehaviour, new()
    where TRaise : IMockBehaviour, new()
{
    protected MoqMock()
    {
        SetMock(new Mock<TInterface>());
    }
}