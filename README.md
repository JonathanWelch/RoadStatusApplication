# Road Status Application
Example solution, written in C#, for retrieving and displaying the status of major roads managed by TfL.

The solution was created using Microsoft Visual Studio 2002 (version **17.5.1** of the 64 bit Community version).  It contains three projects that target the framework **.NET 7.0**.

The extension **SpecFlow for Visual Studio 2022** is required for the **RoadStatusSpecs** project. Version **2022.1.91.26832** was used during development.    

The solution can be built using Visual Studio.

#### Configuring the TfL API 
Two application configuration settings have been included to store the details of the Application ID and API key for the TfL API.  They can be found in the **appsettings.json** file in the root folder of the **RoadStatus** project.  An extract of these settings is shown below:

    "AppSettings": {
    	"ApplicationId": "",
    	"ApiKey": "",
		...
	}

### How to run the application
There are different options available for running the Console Application, which include:

#### Visual Studio
The application can be run inside Visual Studio.  The **RoadStatus** project has been set as the Startup project with `a2` also specified as the command line arguments in the projects debug properties, which results in the application retrieving and displaying the status of the `a2` road.   

#### Windows Powershell
The command `.\RoadStatus.exe a2` can be used to execute the application in the command line to see the status of the `a2` road. 

The exit code of the application can be seen using the command `echo $lastexitcode`

### How to run the tests

During development the ReSharper test runner was used to execute the nunit tests in the two test projects **RoadStatusTests** and **RoadStatusSpecs**.  An alternate approach would be to use Visual Studio's Test Explorer. 


### Assumptions and future enhancements
Changes could be made to have a more specific message written to the console when an exception has occurred retrieving the road status from the TfL API.

