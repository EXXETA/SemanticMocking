using System.Reflection;

namespace SemanticMocking.Abstractions
{
    /// <summary>
    /// Base class for custom mock objects that wrap a specific mocking library
    /// and provide a more semantic interface to the unit test.
    /// </summary>
    /// <typeparam name="TInterface">The interface that should be mocked.</typeparam>
    /// <typeparam name="TArrange">The class implementing methods for mock setups.</typeparam>
    /// <typeparam name="TAssert">The class implementing methods for mock verifies.</typeparam>
    /// <typeparam name="TRaise">The class implementing events raised by the mock.</typeparam>
    public abstract class MockBase<TInterface, TArrange, TAssert, TRaise>
        where TInterface : class
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
        public abstract TInterface Object { get; }
        
        public TArrange Arrange { get; }
        public TAssert Assert { get; }
        public TRaise Raise { get; set; }
        
        protected void SetMock<TMock>(TMock mock)
        {
            SetMockProperty(Arrange, mock);
            SetMockProperty(Assert, mock);
            SetMockProperty(Raise, mock);
        }
        
        private void SetMockProperty<TMock>(object behaviour, TMock mock)
        {
            var propertyInfo = behaviour.GetType().GetProperty("Mock", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo?.SetValue(behaviour, mock);
        }
    }
}