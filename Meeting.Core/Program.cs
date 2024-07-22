using Meeting.Core.DAO;
using Meeting.Core.DAO.Cache;
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
client.CodeFirst.InitTables(typeof(RoomModel), typeof(UserModel), typeof(RoomMemberModel));
ISugarUnitOfWork<MeetingDbContext> dbContext = new SugarUnitOfWork<MeetingDbContext>(client);
builder.Services.AddSingleton(dbContext);

// Init AutoMapper
builder.Services.AddAutoMapper(typeof(RoomDTOMapperProfile));

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// DAO
builder.Services.AddScoped<ICacheHelper, CacheHelper>();
builder.Services.AddScoped<RoomDAO>();
builder.Services.AddScoped<RoomMemberDAO>();

// Services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddSingleton<SessionManager>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
        });
});
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
