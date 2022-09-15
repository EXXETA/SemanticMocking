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
        /// <summary>
        /// Base constructor that initializes the mock and wires up the behaviour implementations.
        /// </summary>
        protected MockBase()
        {
            Arrange = new TArrange();
            Assert = new TAssert();
            Raise = new TRaise();
            
            SetParentProperty(Arrange, this);
            SetParentProperty(Assert, this);
            SetParentProperty(Raise, this);
        }

        /// <summary>
        /// Gets the mocked interface implementation.
        /// </summary>
        public TMock Mock { get; private set; } = null!;
        
        /// <summary>
        /// Gets all behaviours that can be arranged.
        /// </summary>
        public TArrange Arrange { get; }
        
        /// <summary>
        /// Gets all behaviours that can be verified.
        /// </summary>
        public TAssert Assert { get; }
        
        /// <summary>
        /// Gets all events that can be raised.
        /// </summary>
        public TRaise Raise { get; set; }
        
        /// <summary>
        /// Call this method from the mocking framework dependant implementation of this class. 
        /// </summary>
        /// <param name="mock"></param>
        protected void SetMock(TMock mock)
        {
            Mock = mock;
        }
        
        private void SetParentProperty(object behaviour, MockBase<TMock, TArrange, TAssert, TRaise> parent)
        {
            var propertyInfo = behaviour.GetType().GetProperty("Parent", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo?.SetValue(behaviour, parent);
        }
    }
}