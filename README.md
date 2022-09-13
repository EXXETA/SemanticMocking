# What is it about?

SemanticMocking helps you to abstract the mocking framework from the unit test. It enables you to provide 
an API for your dependencies that describe their behaviour in a more semantic way.

# Why do I need this?
Unit tests should be simple not only to write but also to read. Using mocking frameworks helps us to 
abstract dependencies and arrange a predefined behaviour for the dependencies of the system under test (SUT). 
Unfortunately the setup of these dependencies can become quite complex in some scenarios. This bloats the 
unit tests and they become harder to read and to understand. Also some dependencies are used by many 
different SUTs and therefore the same dependency setup and verification code has to be written over and over again. 

Additionally, after you have written a few thousands unit tests you are practically not able to switch 
the mocking framework with a reasonable cost-usage-relation anymore. 


# Ok, tell me more!
Writing your unit test you will most likely follow the arrange, act and assert pattern, short AAA. 
This means your test starts with arranging the state of your SUT and the expected 
behavior of its dependency. After that you execute a method of your SUT that you want to test. 
At the end you assert the return value of this act method or verify an interaction that took place 
with one of its dependencies.

The arrange part is the one that can become a little complex in some scenarios and the setup of 
the dependencies may fill the most lines of your test. It is best practice to inspect only one aspect 
in every unit test, but in those complex setup scenarios we tend to do multiple assertions trying to 
avoid copy and pasting the arrange code into multiple tests.

SemanticMocking will help you to reduce the code that is needed for your arrangements and will provide 
you a growing reusable mocking api. 

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

Here we have a simple unit test for a view model class that has a dependencies to this two services. 
In our tests we use [moq](https://github.com/moq/moq) as our mocking framework of choice. 

*Please note that I added the three stages of 
the AAA pattern as comments just for clarification. I would use these kind of comments
only when the test is complex and the three stages are not clearly identifiable.*

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

These tests are quite short and easy to understand. But nevertheless there is duplicated code.
What happens, when the Product Owner decides the 'no' option should be changed to 'cancel'?

Also the title of the confirm dialog seems to be irrelevant for the test cases but it has to be handled.

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
As we can see the tests got even shorter and even more readable. There is no code duplication 
and no irrelevant aspects anymore. Also there is no code specific to moq in these tests. 
Therefore changing the mocking framework late in the project becomes easier.
A side-effect is also that your unit tests are easier to grasp in a code review. 
From my experience the bigger the pull request is the less likely it is that someone will
have a deep look at the unit tests when they are hard to read/understand.

You may wonder if it would improve the semantic meaning use single mock methods in this example 
instead of using boolean parameters:

```c#
_storageService.Assert.UserWasDeleted();
_storageService.Assert.UserWasNotDeleted();
```

You can do this for sure. I for myself want to keep the list of mock methods short and want to 
be able to reuse existing methods by parameterizing them. When you provide good xml documentation and 
parameter names, intellisense will give you a good explanation how to use these parameters.
Remember that I said we should understand these mock methods as an API for your dependencies?
Every API should be well documented (except everything that is self explaining). 
The mock methods are no exception.

# Ok, now you have convinced me. How can I start using it?
Great. The easiest way to start is to add the SemanticMocking nuget packages to your test project. 
At the moment only moq is supported, but this might change in the future. 

**todo: add link**

Starting with SemanticMocking in a new project is much easier than in an existing project where there are already 
a lot of existing unit tests. Therefore 




## Implementing a mock class
```c#
..
```


