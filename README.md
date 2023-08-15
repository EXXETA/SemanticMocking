# What is it about?

SemanticMocking helps you to abstract the mocking framework from the unit tests and to provide 
an API for your dependencies that describes their behaviour in a more semantic way.

SemanticMocking is not really a framework it is just a few classes that help you to implement 
your mocks in a consistent way and to make them more meaningful.

# Why do I need this?
Unit tests should be simple not only to write but also to read. Using mocking frameworks helps us to 
abstract dependencies and arrange a predefined behaviour for the dependencies of the system under test (SUT). 
Unfortunately the setup of these dependencies can become quite complex in some scenarios. This bloats the 
unit tests and they become harder to read and to understand. Also some dependencies are used by many 
different SUTs and therefore the same dependency setup and verification code has to be written over and over again. 

Additionally, after you have written a few thousands unit tests you are practically not able to switch 
the mocking framework with a reasonable cost-usage-relation anymore. 


# Ok, tell me more!
When writing unit tests you will most likely follow the arrange, act and assert pattern, short AAA. 
This means your test starts with arranging the state of your SUT and the expected 
behavior of its dependencies. After that you execute a method of your SUT that you want to test. 
At the end you assert the return value of this act method or verify an interaction that took place 
with one of its dependencies.

The arrange part is the one that can become a little bit complex in some scenarios and the setup of 
the dependencies may fill the most lines of your test. It is best practice to check only one aspect 
in each unit test, but in complex setup scenarios we tend to perform multiple assertions trying to 
avoid copying and pasting the arrange code into multiple tests, or to use private methods.

SemanticMocking will help you to reduce the code that is needed for your arrangements and will provide you a reusable mocking api. 

# Hmm, can you show me some examples?
Sure. Let's imagine we have a dialog service that is responsible for showing dialogs to the user 
and a storageService that is responsible for accessing the persistence layer.  

```c#
public interface IDialogService {
    Task<string> ConfirmDialog(string title, string message, string confirm, string deny);
}

public interface IStorageService {
    void Delete(User userToDelete);
}

```

Here we have a simple unit test for a view model class that is dependent on this two services. 
In our tests we use [moq](https://github.com/moq/moq) as our mocking framework of choice. 

```c#

public Task DeleteUserAsync_UserDoesNotConfirm_ShouldNotDeleteUser()
{
    // arrange
    _dialogService
        .Setup(mock => mock.ConfirmDialogAsync(It.IsAny<string>(), "Delete user?", "yes", "no"))
        .Returns("no");       

    // act
    _sut.DeleteUserAsync(new User("John", "Doe"));

    // assert
    _storageService.Verify(mock => mock.Delete(It.IsAny<User>()), Times.Never());
}

public Task DeleteUserAsync_UserConfirms_ShouldDeleteUser()
{
    // arrange
    _dialogService
        .Setup(mock => mock.ConfirmDialogAsync(It.IsAny<string>(), "Delete user?", "yes", "no"))
        .Returns("yes");       

    // act
    _sut.DeleteUserAsync(new User("John", "Doe"));

    // assert
    _storageService.Verify(mock => mock.Delete(It.IsAny<User>()), Times.Once());
}
```
*Please note that the commented stages of the AAA pattern are just for clarification. 
One would need these kind of comments only when the test is complex and the three stages 
are not clearly identifiable.*

These tests are quite short and easy to understand. But nevertheless there is duplicated and unrelevant code.

Let's have a look how it could look like when we hide the calls to moq using SemanticMocking:

```c#

public Task DeleteUserAsync_UserDoesNotConfirm_ShouldNotDeleteUser()
{
    // arrange
    _dialogService.Arrange.UserConfirmsUserDeletion(false);

    // act
    _sut.DeleteUserAsync(new User("John", "Doe"));

    // assert
    _storageService.Assert.UserWasDeleted(false);
}

public Task DeleteUserAsync_UserConfirms_ShouldDeleteUser()
{
    // arrange
    _dialogService.Arrange.UserConfirmsUserDeletion(true);

    // act
    _sut.DeleteUserAsync(new User("John", "Doe"));

    // assert
    _storageService.Assert.UserWasDeleted(true);
}
```

Even for these short unit tests we can see some benefits here:
- The tests are even shorter and easier to understand
- There is less code duplication
- No need to adapt the unit test when unrelevant aspects of the test have changed (eg. The 'no' option is renamed to 'cancel')
- changing the mocking framework will be easier (no code specific to moq)

# But now we are hiding the implementation details of the dependencies!
Yes that is true. But does that really matter? We are testing the view model class
and not the dependencies. The implementation details are just one click away.  

Abstracting the details of the dependencies helps to focus on the interaction of the sut
with the dependencies on a semantic level not a technical one.
Also the bigger a pull request is the less likely it is that someone will
have a deep look at the unit tests. Especially when they are hard to read and understand.

# Wouldn't the mock methods look nicer without the boolean parameter?
You mean like this?

```c#
_storageService.Assert.UserWasDeleted();

_storageService.Assert.UserWasNotDeleted();
```

You can do this for sure. However, for a large number of tests it might be better to keep the list of mock methods short and reusing existing methods by parameterization might be a better choice for maintenance reasons.
When you provide good xml documentation and parameter names, intellisense will give you a good 
explanation how to use these parameters.
Remember that we should understand these mock methods as an API for your dependencies?
Every API should be well documented (except everything that is self-explanatory). 
The mock methods are no exception.
For sure this won't help much during a code review you do in Bitbucket or Azure DevOps instead of the IDE. Therefore you should develop a common sense for when and how to use mock methods parameters within your team.

# Ok, now you have convinced me. How can I start using it?
Great. The easiest way to start is to add the SemanticMocking nuget packages to your test project. 
At the moment the supported mocking frameworks are moq and NSubstitute. But adding support for other mocking frameworks is quite easy. 

Just add one of these nuget package to your test project.

```
dotnet add package SemanticMocking.Moq
dotnet add package SemanticMocking.NSubstitute
```


## Implementing a mock class
To implement a mock you have to extend the base class provided in the chosen SemanticMocking library. In this example we use 
*MoqMock<TInterface, TArrangements, TAssertions, TRaises>* as base class to implement a mock for the interface *IMyService* using moq.

```c#
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;

public class MyServiceMock : MoqMock<
    IMyService, 
    MyServiceMock.Arrangements, 
    MyServiceMock.Assertions, 
    MyServiceMock.Raises>
{
    public class Arrangements : BehaviourFor<MyServiceMock>
    {               
    }
   
    public class Assertions : BehaviourFor<MyServiceMock>
    {
    }

    public class Raises : BehaviourFor<MyServiceMock>
    {
    }
}
```

Yes, that is a lot of code for setup only, but it is worth it as soon as you use the same abstractions in different places.
You can copy the Rider file template inside the [SemanticMocking.sln.DotSettings](SemanticMocking.sln.DotSettings) file to create new mocks 
the easy way. If someone creates a template for Visual Studio feel free to contribute!

## Let's get into some details
The behaviours of a mock are distributed over three sub classes: Arrangements, Assertions and Raises. Wich can be accessed by the coresponding properties Arrange, Assert and Raise of your mock. 

As the names indicate you put all methods that do some mock behaviour setup into the Arrangements class and all methods that verify a behaviour into the Assertions class. The Raises class is for methods that raise an event of the mocked dependency.

So if you want to write an assertion method you need to implement it in the Arrangements class:

```c#
...
public void MyTest()
{
    _myService.Arrange.SomeBehaviour();
}
...

public class Arrangements : BehaviourFor<MyServiceMock>
{
    public Arrangements SomeBehaviour()
    {
            // do some setup
            return this;
        }               
    }
}
```
It is recommended to use the instance of the implemented behaviour class as return value for each method. Therefore you are always able to combine existing methods for your test setup. This improves the readabilty of the tests.

```c#
public void MyTest()
{
    _myService.Arrange
        .SomeBehaviour()
        .SomeOtherBehaviour();
}
```

# Q&A

## Why is raising events not included in Arrangements?
That's a good question. In many scenarios we want to bring our SUT into a predefined state. 
Therefore it might have to response to events raised by its dependencies. This is clearly part 
of the arrangement phase. 
But that is not always the case. It can be that you want to test the reaction of your SUT to 
a given event from one of its dependencies. In this case raising the event is the act part 
of the test. 

## What if the mocked dependency provides no events?
You don't need to implement every type of behaviour. You can just use the helper class *NoBehaviour* as
behaviour implementation for your mock.

```c#
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;

public class MyServiceMock : MoqMock<
    IMyService, 
    MyServiceMock.Arrangements, 
    NoBehaviour, 
    NoBehaviour>
{
    public class Arrangements : BehaviourFor<MyServiceMock>
    {               
    }      
}
```
## Do I always have to create a new method in my mock?
No, you don't but it is recommended. You can access the underlying mocking framework mock via the Mock property
of your mock implementation. Then you can use your mocking framework directly in the unit test code.

```c#
_myService.Mock.Setup(mock => mock.DoSomething()).Returns("something");
```

*By the way this is one reasons why the names of the mock instances don't end with the postfix "Mock" (_myService instead of _myServiceMock) in the given examples.* 

## Where can I learn more?
There is a small sample project with some examples in this repository. Just have a look. If you have any further questions you can [create a new issue on Github](https://github.com/EXXETA/SemanticMocking/issues).
