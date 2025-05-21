var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.WriteIndented = false;
    option.JsonSerializerOptions.DefaultBufferSize = 128;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.UseHttpClientMetrics();

// 添加 CORS 策略（开发环境允许所有来源）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // 允许所有来源（生产环境应替换为 WithOrigins("具体域名")）
              .AllowAnyMethod()  // 允许所有 HTTP 方法
              .AllowAnyHeader();// 允许所有请求头
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMetricServer();
//app.UseHttpMetrics();
app.UseCors("AllowAll");

app.Run();
