# Appva code test basic CRUD application
A simple CRUD application for a Todo list application

## Endpoints
```
GET api/todo
Returns all TodoItems in list format
```
```
GET api/todo/{id}
Returns specific TodoItem item at given id
```

```
POST api/todo
Takes a TodoItem resource as request body, JSON is accepted

Returns 201 Created on success, along with created resource
```
```
DELETE api/todo/{id}
Deletes a TodoItem given an id identifier

Returns 204 No Content on success
```
```
PUT api/todo/{id}
Changes a TodoItem resource given a resource body and a valid id
Remember to provide complete resource body since PATCH is not implemented

Returns 204 No Content on success
```

## Resources

### TodoItem
```
{
    "TodoItemId": int(Primary key),
    "Name": string(Required),
    "Desc": string,
    "IsDone": bool
}
```

## Built using
* ASP.NET core - Using MVC
* Entity Framework - Database management
* MSSQL - Database
