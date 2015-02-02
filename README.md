S.i. Systems - Client App
==========================

Setting up your environment:
Requirments:
 - Visual Studio 2013 Update 2 (or later)
 - Xamarin Business Account (see below)
 - Xamarin for Windows AND/OR Xamarin for OS X
 - Machine running OS X

###Xamarin Business Account
Ensure that you have a Business level license. This level is required for using Xamarin with Visual Studio. Also ensure that the account you have is licensed for the correct platform.

###Additional Note: 
When developing on Windows using Xamarin, you will need access to a OS X machine with Xamarin installed to act as your build machine. While you can develop in Visual Studio on Windows, you cannot build the iOS project(s) without OS X and Xcode.


##Build and Run
1. Install the software above
2. Download the latest source code. Note that Visual Studio Online will allow you to easily do this directly from Visual Studio 2013
3. When you launch Visual Studio with a Xamarin iOS Project, a pop-up will appear. Follow the onscreen instruction to set connect for building the iOS project. 


##Set up Development Database
Just run the PowerShell script at "developmentScripts/refresh_Dev_Environment.ps1". If all goes well, this will create a DEV database named SiSystemsClientApp in your local SQL Server instance.


##See Also
Become familiar with:
 - iOS Human Interface Guidelines (https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/index.html)
 