# PassionProject-Movie

<div id="top"></div>
<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Xuanwen1101/PassionProject-Movie">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Find New Movies</h3>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
    </li>
    <li><a href="#example-api-commands">Example API Commands</a></li>
    <li><a href="#entities-relationship">Entities Relationship</a></li>
    <li><a href="#project-scope">Project Scope</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Find New Movies is a Movie Information System Web Application that stores and shares information about the movie, director, and cinemas. The users can get all the different director profiles in this website. They are also allowed to find the release information of each movie and which cinema is showing this movie. The system admins will update the newest information about movies, directors, and cinemas every Friday.

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

* [.NET](https://docs.microsoft.com/en-us/dotnet/)
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
* [Bootstrap](https://getbootstrap.com)
* [JQuery](https://jquery.com)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

- Clone the repository in Visual Studio
- Locate the project folder on the computer
- Create an <App_Data> folder in the project folder
- Open Package Manage Console in VS (Tools > Nuget Package Manager > Package Manage Console) and build the database
```
update-database
```
- Check that the database (View > SQL Server Object Explorer > MSSQLLocalDb > ..)
- Run the example API commands through CURL
- The project has been set up


<p align="right">(<a href="#top">back to top</a>)</p>



<!-- API EXAMPLES -->
## Example API Commands

Update 44349 to match the current port number.
Make sure to utilize Jsondata/movie.json to formulate data you wish to send as part of the POST requests. 
{id} should be replaced with the movie's primary key ID. 

Get a List of Movies
curl https://localhost:44349/api/MovieData/ListMovies

Find the selecsted Movie
curl https://localhost:44349/api/MovieData/FindMovie/{id}

Add a new Movie (new movie info is in movie.json)
curl -H "Content-Type:application/json" -d @movie.json https://localhost:44349/api/MovieData/AddMovie

Delete the selecsted Movie
curl -d "" https://localhost:44349/api/MovieData/DeleteMovie/{id}

Update a Movie (add the selecsted movie id into movie.json)
curl -H "Content-Type:application/json" -d @movie.json  https://localhost:44349/api/MovieData/UpdateMovie/{id}

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- Entities Relationship -->
## Entities Relationship


![Entyties Relationship](images/er.png)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- Project Scope -->
## Project Scope

- Manage Movie (CRUD)
- Manage Director (CRUD)
- Manage Cinema (CRUD)
- Manage relationship between Movie and Cinema

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Xuanwen Zheng: xuanwen1101@outlook.com

Project Link: [https://github.com/Xuanwen1101/PassionProject-Movie](https://github.com/Xuanwen1101/PassionProject-Movie)

<p align="right">(<a href="#top">back to top</a>)</p>

