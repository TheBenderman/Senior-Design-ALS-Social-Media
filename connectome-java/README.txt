To Open emokit, we need an IDE that opens the Maven project in java. 

I used Eclipse with built-in Maven integration. 

[Download\Install Eclipse]

Install eclipse: https://www.eclipse.org/downloads/ 

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
Expand 'src/main/java' then '(default package)' then open BasicReadings

Feels Free to chanage both variables: recordDutarion and fileDestenation. 

Run the program by hitting the play icon in top bar in Eclipse. 

Exported file should appear after refreshing project. In Package Explorer, right-click on emokit > Refresh. 


[Maven Suport]
If importing maven project isn't in eclipse, we need to install an eclipse plugin.  

In Eclipse top bar, navigate to Help > Eclipse Market Place. 
Type in 'm2e' and download the first thing that come which should be : 'Java 8 support for m2e for Eclipse Kepler SR2'

If this doesn't help. good luck. 


