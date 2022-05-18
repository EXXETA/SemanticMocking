using System.Reflection;

namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for custom mock objects that wrap a specific mocking library
    /// and provide a more semantic interface to the unit test.
    /// </summary>
    /// <typeparam name="TMock">The mock object of the wrapped mocking framework.</typeparam>
    /// <typeparam name="TArrange">The class implementing methods for mock setups.</typeparam>
    /// <typeparam name="TAssert">The class implementing methods for mock verifies.</typeparam>
    /// <typeparam name="TRaise">The class implementing events raised by the mock.</typeparam>
    public abstract class MockBase<TMock, TArrange, TAssert, TRaise>
        where TMock : class
        where TArrange : IMockBehaviour, new()
        where TAssert : IMockBehaviour, new()
        where TRaise : IMockBehaviour, new()
    {
        protected MockBase()
        {
            Arrange = new TArrange();
            Assert = new TAssert();
            Raise = new TRaise();
        }

        /// <summary>
        /// Gets the mocked interface implementation.
        /// </summary>
        public TMock Mock { get; private set; } = null!;
        
        public TArrange Arrange { get; }
        public TAssert Assert { get; }
        public TRaise Raise { get; set; }
        
        protected void SetMock(TMock mock)
        {
            Mock = mock;
            
            SetMockProperty(Arrange, mock);
            SetMockProperty(Assert, mock);
            SetMockProperty(Raise, mock);
        }
        
        private void SetMockProperty(object behaviour, TMock mock)
        {
            var propertyInfo = behaviour.GetType().GetProperty("Mock", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo?.SetValue(behaviour, mock);
        }
    }
}