var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok("Deu certo :)");
});


app.MapGet("/estudante/{nome}", (string nome) =>
{
    return Results.Ok($"Oi {nome} seu lindo!");
});

app.MapPost("/", (User user) =>
 {
     return Results.Ok($"O usuario {user.UserName} foi enviado");
 });

app.Run();

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
}

