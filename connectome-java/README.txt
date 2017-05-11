[Purpose]

This document walks through how to run modified emokit project starting from installing java's development kit, Eclipse, importing project, and lastly running it and getting readings raw data from emotiv. 

*Disclaimer* 
This document assumes every step will be completed without any errors. If any error happens, please seekout a solution elsewhere. This walk through is not responsible of any damage that might occur during and after the process. 

I personally used Eclipse.nano with built-in Maven integration and java 1.8. 

[Downloading Java Development Kit (jdk)]
The project requires (1.5 or above). 

To download jdk, go to http://www.oracle.com/technetwork/java/javase/downloads/ (or any updated location)
Next, In the table 'Java SE Development Kit XuXXX' (where X are numbers) check-out 'Accept License Agreement' 
And select download based on your computer OS. 

Notes: when installing java, be sure not to accidently install toolbars. These are really pain in the butt.

[Verifying JDK]

Open command-line (or terminal) type in 'java -version' and it should be reported the java version you have installed. 
If not, then adding java to build path is needed. 

[Adding Java to Build Path]
For not Windows, google it. 

For Windows 10 (or similar), first we need to find where jdk was installed. 
Typically, it's under 'C:\Program Files\Java\jdk1.X.X_XX\bin' (where X are numbers) 

Copy that path (or memorize it if you are smart enough like me). 

Next step, is adding java to build path,

We want to go to 'Evironment Varibale' to add the copied path to build path. There are many way to go to there. 

One long method is: 
Locate to PC, Right-click > Properties. 
On the riht side, Advanced system settings > Environment Variables... 

Another is: 
In SearchBar is start menu, type 'Environment Variables' then select 'Edit the system environment variable' 

After you find that window, on the second list named 'System variables', locate and select 'Path' variable.

Double click 'Path' variable or select then click 'Edit...' button. 

On the 'Edit Environment Variable' popup, click 'New' button and paste the memorized java path (or paste if you're lazy)

*Important*: You might already know this, restart commandline so the changes take effect, otherwise you'll think nothing works. Leanred that first hand. 

Go back and verify. Also verify 'javac -version' (different from 'java -version').

[Download\Install Eclipse]

Install eclipse: https://www.eclipse.org/downloads/  (or any updated location)
After running installtion exe, there will be several options. 
I used is the first option: 'Eclipse IDE for Java Developers' since it comes with build it Maven integration. 
Any version of eclipse should be fine as long you know how to download support for Maven. 

[Importing Project into Eclipse]
Open Eclipse. 

Navigate to File > Import > Maven > Existing Maven Projects
Set Root Directectory to the one containingg this README file which should ends with '\connectome-java'. 
In 'Projects' there should be displayed /emokit/pom.xml and checked. Click Finish. 
Eclipse should start importing project and downloading dependecies *this might take couple minutes* 

[Running emokit in Eclipse]
After importing the project into Eclipse, expand the project folder in 'Package Exploere' 
Expand 'src/main/java' then '(default package)' then open BasicReadings.java
Feels Free to chanage both variables: recordDutarion and fileDestenation. 
Run the program by hitting the play icon in top bar in Eclipse. 
Exported file should appear after refreshing project. In Package Explorer, right-click on emokit > Refresh. 

[Maven Suport]
If importing maven project isn't in eclipse, we need to install an eclipse plugin.  
In Eclipse top bar, navigate to Help > Eclipse Market Place. 
Type in 'm2e' and download the first thing that come which should be : 'Java 8 support for m2e for Eclipse Kepler SR2'

If this doesn't help, good luck. 


Auther(s)
- Khaled Alhendi 

Last edited: 
May 3rd: Added JDK related instructions. 
May 1st: Created this README.

