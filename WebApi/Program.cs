var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Load available words
var availableWords = app.Configuration
	.GetValue<string>("AvailableWords")
	.Split(';');


Random random = new();
var wordIndex = random.Next(0, availableWords.Count() - 1);
var dailyWord = availableWords[wordIndex];

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/words", () =>
{
	return availableWords;
})
.WithName("Get all available words");

app.MapGet("/words/daily", () =>
{
	return dailyWord;
})
.WithName("Get the daily word to be guessed");

app.MapGet("/words/{word}/assert", (string word) =>
{
	return word == dailyWord
		? Results.Ok("Success!")
		: Results.NotFound("Fail");
}) 
.WithName("Check if the provided word is correct.");

app.Run();

