S.i. Systems - Client App
==========================

Setting up your environment:

Requirements:
 - Visual Studio 2013 Update 2 (or later)
 - Xamarin Business Account (see below)
 - Xamarin for Windows AND/OR Xamarin for OS X
 - Machine running OS X
 - SQL Server 2008+
 
###Xamarin Business Account
Ensure that you have a Business level license. This level is required for using Xamarin with Visual Studio. Also ensure that the account you have is licensed for the correct platform.

###Additional Note: 
When developing on Windows using Xamarin, you will need access to a OS X machine with Xamarin installed to act as your build machine. While you can develop in Visual Studio on Windows, you cannot build the iOS project(s) without OS X and Xcode.

##Set up Development Database
On your Windows machine, run the PowerShell script at "developmentScripts/refresh_Dev_Environment.ps1". If all goes well, this will create a DEV database named SiSystemsClientApp in your local SQL Server instance.

##Start local SMTP Server
Install SMTP4Dev (http://smtp4dev.codeplex.com/) and run it. SMTP4Dev acts as a local SMTP server and is useful when debugging anything that sends e-mail.
Also, during development, ELMAH logging is configured to send emails on error, so this will intercept those nicely.

##Build and Run
1. Install the software above
2. Download the latest source code. Note that Visual Studio Online will allow you to easily do this directly from Visual Studio 2013
3. When you launch Visual Studio with a Xamarin iOS Project, a pop-up will appear. Follow the onscreen instruction to set connect for building the iOS project. 


##Ensure that the iOS simulator can connect to the Web API running in Visual Studio + IIS Express
1. Navigate to C:\Users\[your username]\Documents\IISExpress\config
2. Edit the applicationhost.config file.
	a. Find the <site> element that exists for the project (you'll need to have run it in VS at least once for the entry to be created).
	b. Change the bindingInformation attributes by removing localhost. This will allow IIS Express to handle incoming connections from external machines.
	   For example, change bindingInformation="*:50021:localhost" to bindingInformation="*:50021:"
	   and bindingInformation="*:44300:localhost" to bindingInformation="*:44300:"
3. On your mac, edit the file @ /private/etc/hosts
	a. sudo nano /private/etc/hosts
	b. add an entry with your windows machine IP Address and set the hostname to clientapi.local
	c. you should be able to access your running IIS Express instance at https://clientapi.local:44300/ or http://clientapi.local:50021



##See Also
Become familiar with:
 - iOS Human Interface Guidelines (https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/index.html)
 