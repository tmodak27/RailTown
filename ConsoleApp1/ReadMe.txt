
INTRO
I have decided to implement this project as a simple console app. The project takes 3 external arguments- City name, city latitude, and city longitude. The console app prints some of the output fields to console, and generated a csv file Visitors_<CITYNAME>.csv that has all the required fields. The input url as well as the output file path are hardcoded into the project. this could further be improved by passing them as arguments as well, but I wanted to keep it simple. 

WORKFLOW:
The project consists of the following functions in temporal order"
1. Get(): this function makes a simple get request to the url and returns the json object serialized as a string.
2. GetDistance(): The GetDistance function takes in the City object and then uses the latitude and longitude of both the city and the visitor to calculate the distance in miles
3. GetNEarestVisitors(): This function iterates over a json array of visitor data, and calculates the distance for each visitor from the city. The output is a sorted list of visitors in ascending order of distance to the city.
4. WriteToFile(): this function takes in the output of Function #3 and writes it to a csv.  The fields in the file are id, name, address, phone, latitude, longitude, distance.

UNIT TESTING
Kindly refer to Unit Tests.txt.

INSTRUCTIONS:
1. Once the project is cloned to a local repo, right click on the project name in the Solution Explorer and click on Properties-> Debug
In the arguments list, type the arguments as <Latitude> <Longitude> <CityName>. For example, for vancouver the arguments list will be <49.2827 123.1207 Vancouver>. After running from Visual Studio you will get a console output as well as a file Visitors_Vancouver.csv in the project folder itself.

2. Alternatively, this can be run through command prompt as well. 
Open command prompt, and go to the \ConsoleApp1. Type the below command below
dotnet publish -c Debug -r win10-x64

This will create a file ConsoleApp1.exe in the folder ConsoleApp1\bin\Debug\netcoreapp2.1\win10-x64\publish\. It might take approx 2 minutes for this to complete. 
Once this is done, go to ConsoleApp1\bin\Debug\netcoreapp2.1\win10-x64\publish\ in the command prompt, and type the command below
ConsoleApp1.exe 49.2827 123.1207 Vancouver

This will print the output in the command prompt, and then generate the output csv as well. 

PUBLISHING TO AZURE: 
Publishing to azure would require generating the EXE as discussed above, and then using Service Fabric to schedule it as a periodic task.Alternatively, we can also use a virtual machine or a local machine and use Task Manager to run the EXE on schedule. 

CONSIDERATIONS
There were several ways to implement this- Console app, MVC Web app, Web form, etc. I decided to go with a console app primarily because of its simplicity. With console app It was easy to write the logic and then supply the arguments using the command line.


