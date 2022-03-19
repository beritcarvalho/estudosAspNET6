using ToDo.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();//Adicionando os controllers
builder.Services.AddDbContext<AppDbContext>();//Adicionando o contexto aos servi�os do AspNet

var app = builder.Build();
app.MapControllers();
app.Run();
