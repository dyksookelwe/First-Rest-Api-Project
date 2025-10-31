using System.Text.RegularExpressions;
using RestApiProject.Models;


List<Person> users = new List<Person>
{
    new() {Id = Guid.NewGuid().ToString(), Name = "Martin", Age = 30},
    new() {Id = Guid.NewGuid().ToString(), Name = "Adam", Age = 25},
    new() {Id = Guid.NewGuid().ToString(), Name = "Mariam", Age = 20 }
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

//Processing requests in terminal middleware
app.Run(async (context) =>
{
    //To simplify the work, we will add variables
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    //Get all users data
    if (path == "/api/users" && request.Method == "GET")
    {
        await GetAllPeople(response);
    }
    //Get one user data
    else if (Regex.IsMatch(path.Value ?? "", expressionForGuid) && request.Method == "GET")
    {
        string? id = path.Value?.Split("/")[3];
        await GetPerson(id, response);
    }
    //Create new user
    else if (path == "/api/users" && request.Method == "POST")
    {
        await CreatePerson(response, request);
    }
    //Update user data
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdatePerson(response, request);
    }
    //Delete user data
    else if (Regex.IsMatch(path.Value ?? "", expressionForGuid) && request.Method == "DELETE")
    {
        string? id = path.Value?.Split("/")[3];
        await DeletePerson(id, response);
    }
    else
    {
        response.StatusCode = 404;
    }
});

app.Run();

async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users); //get all users information
}

async Task GetPerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault((u) => u.Id == id); //getting user in list by id
    if (user != null)
    {
        await response.WriteAsJsonAsync(user);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "User is not found" });
    }
}

async Task DeletePerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault((u) => u.Id == id); //we are getting user by id

    if (user != null) //if user exist then delete
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "User is not found" });
    }
}

async Task CreatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        var user = await request.ReadFromJsonAsync<Person>();
        if (user != null)
        {
            user.Id = Guid.NewGuid().ToString(); //Create a new Id to user
            users.Add(user);
            await response.WriteAsJsonAsync(user);
        }
        else
        {
            throw new Exception("Incorrect data");
        }
    }
    catch (Exception) //The information received is incorrect
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync("Incorrect data");
    }
}

async Task UpdatePerson(HttpResponse response, HttpRequest request) //User data update logic
{
    try
    {
        Person? userData = await request.ReadFromJsonAsync<Person>(); //Get user data
        if (userData != null) //
        {
            var user = users.FirstOrDefault((u) => u.Id == userData.Id);
            if (user != null) //If the user exists, we override his data.
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "User is not found" });
            }
        }
        else
        {
            throw new Exception("Incorrect data");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Incorrect Data" });
    }
}





