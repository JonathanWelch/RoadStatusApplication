# Road Status Application
Example C# solution for retrieving and displaying the status of major roads managed by TfL (Transport for London).  Below are the key details about the solution:

### Solution Overview
Developed using Microsoft Visual Studio 2022, specifically version 17.5.1 of the 64-bit Community edition. It requires the SpecFlow extension for Visual Studio 2022, with version 2022.1.91.26832 used during development. The solution consists of three projects targeting the .NET 7.0 framework and can be easily built using Visual Studio.

#### Configuring the TfL API 
The application includes two important configuration settings: Application ID and API key for the TfL API. These settings are stored in the appsettings.json file located in the root folder of the RoadStatus project.

    "AppSettings": {
    	"ApplicationId": "",
    	"ApiKey": "",
		...
	}

### How to Run the Application
You can run the Console Application using the following methods:

#### Visual Studio
The RoadStatus project is set as the Startup project. Specify the desired road (e.g., "a2") as command line arguments in the project's debug properties. This allows you to easily retrieve and display the status of the specified road when running the application.

#### Windows Powershell
Execute the application in the command line using the following command: 

	.\RoadStatus.exe a2

To check the application's exit code, use the command: 

	echo $lastexitcode

### How to Run the Tests
During development, the ReSharper test runner was used to execute the NUnit tests in the RoadStatusTests and RoadStatusSpecs projects.  Alternatively, you can use Visual Studio's Test Explorer to run these tests. 

The unit tests are located in the RoadStatusTests project, while BDD-style acceptance tests, implemented with SpecFlow, are found in the RoadStatusSpecs project.

### Assumptions and Future Enhancements
In the acceptance tests, the RoadStatusService is executed directly, not the Console application. Consequently, there's no assertion to validate the specific exit code of the Console application. A potential future enhancement could include this coverage. 

Another future enhancement would be to have a more detailed message written to the console when an exception has occurred retrieving the road status from the TfL API.  Currently, the application displays a generic invalid road message.