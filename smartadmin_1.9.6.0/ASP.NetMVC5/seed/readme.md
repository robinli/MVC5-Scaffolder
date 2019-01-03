## SmartAdmin 1.8 for MVC5 "SeedProject"

+ Issue Tracker: [This project references NuGet package(s) that might seem "missing" when opening the project inside Visual Studio. These packages should automatically get restored upon your first build; additionally you can invoke the menu item **Restore NuGet Packages** when right-clicking on your solution inside the Solution Explorer]
+ When we commit the updates for SmartAdmin 1.8, the Packages folder will not be included into Source Control. By doing so, it not only saves us repository size it also prevents security issues.

**Note:** If you get the error stated above on the first time you run the Solution, by simply enabling Nuget package manager to download those packages, you can run the project smoothly. 

#### Remedy:

1. In Visual Studio, right-click the solution and click "Restore NuGet Packages"; this will download the necessary packages needed to run the solution.

2. Build the project and Run.


**Note:** The packages used by the project are the default package files when you create a new MVC template within Visual Studio.