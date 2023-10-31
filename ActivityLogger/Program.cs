using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// the connection string would reside in a config file and not hard coded, this is due to time limitations
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient("mongodb+srv://tomer265:webbingtest123@cluster0.db25y.mongodb.net/?retryWrites=true&w=majority"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
