using Meeting.Core.DAO;
using Meeting.Core.Meeting;
using Meeting.Core.Models;
using Meeting.Core.Models.DTO;
using Meeting.Core.Services;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Init Database 
ISqlSugarClient client = new SqlSugarClient(new ConnectionConfig()
{
    ConnectionString = builder.Configuration.GetConnectionString("MySQL")
});
client.DbMaintenance.CreateDatabase();
client.CodeFirst.InitTables(typeof(RoomModel));
ISugarUnitOfWork<MeetingDbContext> dbContext = new SugarUnitOfWork<MeetingDbContext>(client);
builder.Services.AddSingleton(dbContext);

// Init AutoMapper
builder.Services.AddAutoMapper(typeof(RoomDTOMapperProfile));
// DAO
builder.Services.AddScoped<RoomDAO>();

// Services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddSingleton<SessionManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SdpExchangeHub>("/sdp");

app.Run();