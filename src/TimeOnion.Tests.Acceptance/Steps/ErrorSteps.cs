using TechTalk.SpecFlow;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class ErrorSteps
{
    private readonly ErrorDriver _errorDriver;

    public ErrorSteps(ErrorDriver errorDriver) => _errorDriver = errorDriver;

    [Then(@"an error occurred with the message ""(.*)""")]
    public void ThenAnErrorOccurredWithTheMessage(string errorMessage) =>
        _errorDriver.AssertErrorOccurredWithMessage(errorMessage);

    [Then(@"no error occurred")]
    public void ThenNoErrorOccurred() => _errorDriver.AssertNoErrorOccurred();
}