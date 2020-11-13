# hacker-news
This is a simple coding test API for consuming data from https://github.com/HackerNews/API


# run

Open the solution in Visual Studio 2019 <br/>
Set News.API as default project <br/>
Ctrl + F5 to run the project on IIS Express <br/>

# changes

Add exception handling + custom exception <br/>
Add logging (built-in or Serilog) <br/>
Add authentication using an API_KEY <br/>
Add output cache mechanism if the data we are reading don't have real time changes <br/>
Add a HttpClient helper to execute external APIs requests <br/>
Have request URI details like API_Version in appsettings instead of hardcoded (e.g. /v0/) <br/>

Note: Depending on context or complexity some of the above points may not be needed


