# Disclosure: 
I am in no way familiar running tests built with VS in a linux or mac environment.
My software career has been .net focused, so I built these tests using SpecFlow and NUnit.
This project is targeting .net Core 5.0.

# Pre-Reqs:
1. **dotnet 5.0 SDK**
	Installation instructions for .NET 5.0 on Linux or macOS can be found here: https://dotnet.microsoft.com/downloads

# To Run
In the project will be a Tests folder, this has all the required dlls published. Run "dotnet test PlatformScience.Service.Specs.dll" to start test execution on a linux machine with dotnet sdk installed.
If using Visual Studio, then simply build the project, and run the tests from the Test Explorer window (my preferred method). 

# Test Overview
I utilize SpecFlow hooks to start the API service up before each scenario. Ideally, I would want to do this on a "BeforeTestRun" hook, and not a "BeforeScenario" hook,
however, in testing, there are bugs preventing me from using the ideal method. I wanted to show non-flaky tests for the assesment, so decided to go for the fresh service start before each scenario.
I try to alleviate the amount of service starts and stops by filtering with scenario tags when it is necessary to start or stop, for example in the tests in which I am checking for a Bad Request response anyways, I allow the service to stay running. 

# Findings
1. There is a bug in which when the "hoover" passes over a patch, it is registered as going over it, however the patch is never removed. Subsequent calls to the cleaning session endpoint do not seem to recreate the grid entirely and previously created patches from previous calls are stil present, even after being passed over.
2. I would assume as a user, that each cleaning session would be unique. This does not seem to be the case as mentioned in finding 1, that previous patches are left behind.
3. Another assumption, is that if I start on a cell with a patch, that the service would automatically clean the patch it starts on and count that towards the amount of patches cleaned, it however does not. In a scenario, I used a 5x5 grid, placed a patch on all coordinates, traversed across all coordinates, and only returned with 24 patches when the expectation would be 25. (to include the starting position patch)
4. The service allows a user to start off grid. In this scenario I created a 5x5 grid, and started at [9,0]. I would expect this to return a bad request status code on call. 
5. In the following instances when I use a negative number I do not always get the status code expected:
	- roomSize [-1, 5] Expected: Bad Request Received: OK
	- roomSize [5, -1] Expected: Bad Request Received: OK
	- coords [0, -1]   Expected: Bad Request Received: OK
	- coords [-1, 0]   Expected: Bad Request Received: Bad Request (Works as expected)
	- patches [-1, 0]  Expected: Bad Request Received: OK
	- patches [0, -1]  Expected: Bad Request Received: Bad Request (Works as expected)