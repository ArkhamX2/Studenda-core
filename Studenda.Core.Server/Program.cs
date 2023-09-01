using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Studenda.Core.Data;
using Studenda.Core.Data.Configuration;
using Studenda.Core.Model.Account;
using Studenda.Core.Server.Data;
using Studenda.Core.Server.Utils;
using Studenda.Core.Server.Utils.Token;


#if DEBUG
const bool isDebugMode = true;
#else
const bool isDebugMode = false;
#endif
var builder = WebApplication.CreateBuilder(args);
//��������� �����������
builder.Services.AddControllers();
//��������� �  builder ������������ ��� ���� ������(���������� ���  sqlite)
builder.Services.AddSingleton<ContextConfiguration>(_ => new SqliteConfiguration("Data Source=000_debug_storage.db", isDebugMode));
//��������� ���� ������
builder.Services.AddDbContext<DataContext>(o => o.UseSqlite());
builder.Services.AddDbContext<ServerDataContext>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddIdentity<Account, IdentityRole<long>>()
                .AddEntityFrameworkStores<ServerDataContext>()
                .AddUserManager<UserManager<Account>>()
                .AddSignInManager<SignInManager<Account>>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(2),
        };
    });


var app = builder.Build();
app.MapControllers();
app.Run();