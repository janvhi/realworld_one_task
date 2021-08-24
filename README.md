
# RealWorld_One Rotate cat API.

This API provide the following functionality:

This repository contains two controller
1. CatController: It displayed rotated cat coming from cataas.com with additional manipulation provided in querystring.
 2. AuthenticateController: It deal with Registering and login functionality using JWT Token.

It also performs API documentation, Unit testing and E2E testing for the API implemented

## Software/Packages used
For this project I have used below softwares/libraries

- InMemoryDatabase
- ASP.NET Core 5.0
- Entity Framework 5.0
- NLog for logging
- MSTest for Unit Testing
- Xunit for Integration Testing
- Swagger

## Usage
#### GET registered users
```http://localhost:5000/api/authenticate/retrieveUser```
 
#### GET rotated cat
```http://localhost:5000/api/v1/cat```

#### GET rotated cat with filter(you can try passing blur, mono, sepia, negative, paint, pixel) passed as a querystring
```http://localhost:5000/api/v1/cat?filter=sepia```

#### GET rotated cat based on width or/and height passed in querystring
```http://localhost:5000/api/v1/cat?width=600&height=500```

#### GET rotated cat based on type(small or sm, medium or md, square or sq, original or or) passed as a querystring
```http://localhost:5000/api/v1/cat?type=:type```

#### GET rotated cat with mixed query string value(type,filter,width,height) passed as a querystring
```http://localhost:5000/api/v1/cat?filter=sepia&width=700&height=500```

#### To Register New User
```http://localhost:5000/api/authenticate/register```

```json
{
   "username": "test@integration.com",
   "password":  "SomePass1234!"
}
```

#### To Login
```http://localhost:5000/api/authenticate/login```
```json
   {
      "username": "test@integration.com",
      "password":  "SomePass1234!"
   }
```

you will receive JWT Token on login, which can be used to authenticate 
get cat and get registered user api

