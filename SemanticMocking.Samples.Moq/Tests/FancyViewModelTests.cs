using NUnit.Framework;
using SemanticMocking.SampleTests.App;
using SemanticMocking.SampleTests.Mocks;

namespace SemanticMocking.SampleTests.Tests;

public class Tests
{
    private FancyViewModel _sut = null!;

    private DialogServiceMock _dialogService = null!;
    
    [SetUp]
    public void Setup()
    {
        _dialogService = new DialogServiceMock();
        
        _sut = new FancyViewModel(_dialogService.Object);
    }

    [Test]
    public void DoSomeStuffCommand_ShowsAlert()
    {
        _sut.DoSomeStuffCommand.Execute(null);

        _dialogService.Assert.DidShowErrorAlert();
    }
}