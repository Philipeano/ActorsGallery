# ActorsGallery
A simple REST API that enables users to fetch information about motion picture episodes and characters, and post comments.


## Overview
This is a back-end service which can be consumed by any web client that supports sending and receiving data in JSON format. 
This application is a work in progress. Consequently, certain features, particularly 'authentication', 'update' and 'delete' operations, will be available in the next version. 

- View the API documentation [here](http://actorsgallery.herokuapp.com/index.html)  
- Access the hosted API [here](http://actorsgallery.herokuapp.com/api).
- Follow the project's progress via the [issue tracker](https://github.com/Philipeano/ActorsGallery/projects/1).



## Main features
- Register motion picture characters.
- Fetch list of characters, optionally filtered and/or sorted.
- Create motion picture episodes.
- Fetch list of episodes, optionally filtered with a featured character.
- Assign episode roles to characters. 
- Post comments on episodes, with each commenter's public IP address automatically retrieved and saved.
- Create locations, defined with longitude and latitude values.
- Fetch list of locations.



## Endpoints

| Endpoint URL       | Method    | Functionality   | Available |
| :----------------- | :------------- | :-------------- | :-------------- |
|**_CHARACTERS_**           |        |                               |		|
| /characters      | ```POST```   | Register a new character    |  	Yes	|
| /characters               | ```GET```    | Fetch all characters    |	Yes	|  
| /characters/:id       | ```PUT```    | Update a specified character    |	No	|
| /characters/:id       | ```DELETE``` | Delete specified character    |	No	|
|**_EPISODES_**          |        |                               |
| /episodes              | ```POST```    | Create a new episode |  	Yes	|
| /episodes/:id/characters              | ```POST```    | Assign an episode role to a character |  	Yes	|
| /episodes/:id/comments              | ```POST```    | Post a comment on an episode |  	Yes	|
| /episodes              | ```GET```    | Fetch all episodes |  	Yes	|
| /episodes/:id     | ```PUT```    | Update a specified episode |	No	|
| /episodes/:id     | ```DELETE``` | Delete a specified episode |	No	|
|**_COMMENTS_**                              |        |                               |
| /comments                                  | ```GET```    | Fetch all comments |  	Yes	|
| /comments/:id     | ```PUT```    | Update a specified comment |	No	|
| /comments/:id     | ```DELETE``` | Delete a specified comment |	No	|
|**_LOCATIONS_**                              |        |                               |
| /locations/  | ```POST```    | Create a new location |	Yes	|
| /locations                  | ```GET```    | Fetch all locations |  	Yes	|
| /locations/:id  | ```PUT```    | Update a specific location |	No	|
| /locations/:id  | ```DELETE``` | Delete a specific location |	No	|



## Built with

- Primary language: ```C#``` 
- Server technology: ```ASP.Net Core```
- Target runtime: ```.Net Core 3.1.1```
- Database system: ```MySQL``` powered by ```ClearDB```
- ORM: ```Entity Framework Core```
- API documentation: ```Swashbuckle.AspNetCore```
- Containerization: ```Docker``` 



## License
[MIT � Philip Newman.](https://opensource.org/licenses/MIT)
