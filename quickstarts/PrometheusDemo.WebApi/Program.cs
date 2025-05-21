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

// ��� CORS ���ԣ�������������������Դ��
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ����������Դ����������Ӧ�滻Ϊ WithOrigins("��������")��
              .AllowAnyMethod()  // �������� HTTP ����
              .AllowAnyHeader();// ������������ͷ
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
