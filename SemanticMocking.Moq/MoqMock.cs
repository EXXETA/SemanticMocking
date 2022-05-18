using System.Reflection;
using Moq;
using SemanticMocking.Abstractions;

namespace SemanticMocking.Moq;

public abstract class MoqMock<TInterface, TArrange, TAssert, TRaise> : MockBase<TInterface, TArrange, TAssert, TRaise>
    where TInterface : class
    where TArrange : IMockBehaviour, new()
    where TAssert : IMockBehaviour, new()
    where TRaise : IMockBehaviour, new()
{
    protected readonly Mock<TInterface> Mock;

    protected MoqMock()
    {
        Mock = new Mock<TInterface>();

        SetMock(Mock);
    }

    public override TInterface Object => Mock.Object;
}