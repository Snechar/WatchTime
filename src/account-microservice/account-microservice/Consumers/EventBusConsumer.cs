using account_microservice.Authentication;
using account_microservice.DTO;
using account_microservice.JWTOperations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NATS.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace account_microservice.Consumers
{
    public class EventBusConsumer : BackgroundService
    {
        static UserManager<ApplicationUser> userManager;
        static RoleManager<IdentityRole> roleManager;
        static IConfiguration _configuration;
        static  IConnection _connection;
        static JTWOperations jWTOperations;

        public EventBusConsumer(IConnection connection, IServiceScopeFactory factory, IConfiguration configuration)
        {
            userManager = factory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 
            _connection = connection;
            _configuration = configuration;
            jWTOperations = new JTWOperations(_configuration, factory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());
        }

    
    static EventHandler<MsgHandlerEventArgs> hCreateAccount = (sender, args) =>
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(args.Message.Data);
            var deserializedMessage = (JObject)JsonConvert.DeserializeObject(receivedMessage);
            var decodedMessage = deserializedMessage.SelectToken("Message").ToString();
            Console.WriteLine($"Got message: {decodedMessage}");
            try
            {
                AccountRegistration account = System.Text.Json.JsonSerializer.Deserialize<AccountRegistration>(decodedMessage);
                Response response = Register(account).Result;
                _connection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(response)));




            }
            catch (Exception)
            {
                Response response = new Response()
                {
                    Status = "Failed",
                    Message = "Json object incorrect ",
                };
                _connection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(response)));
                
            }
         
            

        };
        static EventHandler<MsgHandlerEventArgs> hLogin = (sender, args) =>
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(args.Message.Data);
            var deserializedMessage = (JObject)JsonConvert.DeserializeObject(receivedMessage);
            var decodedMessage = deserializedMessage.SelectToken("Message").ToString();
            Console.WriteLine($"Got message: {decodedMessage}");
            try
            {
                AccountLogin account = System.Text.Json.JsonSerializer.Deserialize<AccountLogin>(decodedMessage);
                Response response = Login(account).Result;
                _connection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(response)));
            }
            catch (Exception)
            {
                Response response = new Response()
                {
                    Status = "Failed",
                    Message = "Json object incorrect",
                };
                _connection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(response)));

            }



        };
        static EventHandler<MsgHandlerEventArgs> hCheck = (sender, args) =>
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(args.Message.Data);
            var deserializedMessage = (JObject)JsonConvert.DeserializeObject(receivedMessage);
            var decodedMessage = deserializedMessage.SelectToken("Message").ToString();
            Console.WriteLine($"Got message: {decodedMessage}");
            ApplicationUser user = userManager.FindByNameAsync(decodedMessage).Result;
            var token = jWTOperations.GenerateTokenAsync(user);
            var result = jWTOperations.ValidateToken(token, UserRoles.User);
            Console.WriteLine(result);
            _connection.Publish(args.Message.Reply, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize("hello")));


        };

        static async Task<Response> Register(AccountRegistration account)
        {
            var userExists = await userManager.FindByNameAsync(account.Username);
            if (userExists != null)
                return new Response { Status = "Error", Message = "Username already exists!" };
            var emailExists = await userManager.FindByNameAsync(account.Email);
            if (emailExists != null)
                return new Response { Status = "Error", Message = "Email already exists" };

            ApplicationUser user = new ApplicationUser()
            {
                Email = account.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = account.Username,
            };
            var result = await userManager.CreateAsync(user, account.Password);
            userManager.AddToRoleAsync(user, UserRoles.User).Wait();
            if (!result.Succeeded)
                return new Response { Status = "Error", Message = result.ToString() };

            return new Response { Status = "Success", Message = "User created successfully!" };
        }

        static async Task<Response> Login(AccountLogin model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(24),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new Response { Status = "Success", Message = new JwtSecurityTokenHandler().WriteToken(token) };
            }
            return new Response { Status = "Error", Message = "LogIn Failed" };
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IAsyncSubscription sCreateAcccount = _connection.SubscribeAsync("account.create", "load-balancing-queue-account", hCreateAccount);
            IAsyncSubscription sLogin = _connection.SubscribeAsync("account.login", "load-balancing-queue-account", hLogin);
            IAsyncSubscription sCheckRoles = _connection.SubscribeAsync("account.check", hCheck);
            Console.WriteLine("Event Bus Hosted Service has been started");
            return Task.CompletedTask;
        }

    }
}
