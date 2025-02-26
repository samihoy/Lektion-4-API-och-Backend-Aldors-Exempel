
using System.Reflection;

namespace Lektion_4_Backend_och_API_Lektions_antekningar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

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

            app.MapGet("/async", async() => 
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                Task t1 = Task.Run(async ()=>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("hämtar databas...");
                    Task.Delay(5000).Wait();
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("datahämtning är klar!");
                
                });

                await t1;
                Console.WriteLine("data hämtning är klar!");

                return Results.Ok("202 succses");
            });

            app.MapGet("task", ()=>
            {
                Task t1 = Task.Run (()=>
                    {
                        Console.WriteLine("console writeline console operation påbörjad");
                        Console.WriteLine("hämtar data från databas....");
                        Thread.Sleep(5000);
                        Console.WriteLine("data hämtnng klar!");
                    });
            });

            app.MapGet("/syncron", () =>
            {
                Console.WriteLine("");

                Thread t1 = new Thread(()=>
                {
                    Console.WriteLine("console writeline console operation påbörjad");
                    Console.WriteLine("hämtar data från databas....");
                    Thread.Sleep(5000);
                    Console.WriteLine("data hämtnng klar!");
                });

                Thread t2 = new Thread(()=>
                {
                    Console.WriteLine("console writeline console operation påbörjad");
                    Console.WriteLine("hämtar data från databas....");
                    Thread.Sleep(5000);
                    Console.WriteLine("data hämtnng klar!");

                });

                Console.WriteLine("tråd 1 startas");
                t1.Start();
                Console.WriteLine("tråd 2 startas");
                t2.Start();

                t1.Join(); //gör så att programet stanar och väntar in "tråden" innan den går vidare i programet
                t1.Join(); // alltså den pausar programet tills tasken är klar

                Console.WriteLine("alla operationer är färdig");
                return Results.Ok("sucses");
            });

            app.Run();
        }
    }
}
