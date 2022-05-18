using System;
using NUnit.Framework;
using SemanticMocking.SampleTests.App;
using SemanticMocking.SampleTests.Mocks;

namespace SemanticMocking.SampleTests.Tests;

public class Tests
{
    private FancyViewModel _sut = null!;

    private DataProviderMock _dataProvider = null!;
    private DialogServiceMock _dialogService = null!;
    
    [SetUp]
    public void Setup()
    {
        _dataProvider = new DataProviderMock();
        _dialogService = new DialogServiceMock();
        
        _sut = new FancyViewModel(
            _dataProvider.Mock.Object,
            _dialogService.Mock.Object);
    }

    [Test]
    public void DoSomeStuffCommand_Fails_ShowsAlert()
    {
        _dataProvider.Arrange.GetRecordsFailsWith(new NullReferenceException("foo bar"));
        
        _sut.DoSomeStuffCommand.Execute(null);

        _dialogService.Assert.DidShowErrorAlert("foo bar");
    }
    
    [Test]
    public void DoSomeStuffCommand_NoRecordsAvailable_ShowsDataNotFoundInfo()
    {
        _dataProvider.Arrange.HasNoRecordsYet();
        
        _sut.DoSomeStuffCommand.Execute(null);

        _dialogService.Assert.DidShowMessage("No records found!");
    }
}