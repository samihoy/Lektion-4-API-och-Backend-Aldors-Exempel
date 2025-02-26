
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
                    Console.WriteLine("h�mtar databas...");
                    Task.Delay(5000).Wait();
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("datah�mtning �r klar!");
                
                });

                await t1;
                Console.WriteLine("data h�mtning �r klar!");

                return Results.Ok("202 succses");
            });

            app.MapGet("task", ()=>
            {
                Task t1 = Task.Run (()=>
                    {
                        Console.WriteLine("console writeline console operation p�b�rjad");
                        Console.WriteLine("h�mtar data fr�n databas....");
                        Thread.Sleep(5000);
                        Console.WriteLine("data h�mtnng klar!");
                    });
            });

            app.MapGet("/syncron", () =>
            {
                Console.WriteLine("");

                Thread t1 = new Thread(()=>
                {
                    Console.WriteLine("console writeline console operation p�b�rjad");
                    Console.WriteLine("h�mtar data fr�n databas....");
                    Thread.Sleep(5000);
                    Console.WriteLine("data h�mtnng klar!");
                });

                Thread t2 = new Thread(()=>
                {
                    Console.WriteLine("console writeline console operation p�b�rjad");
                    Console.WriteLine("h�mtar data fr�n databas....");
                    Thread.Sleep(5000);
                    Console.WriteLine("data h�mtnng klar!");

                });

                Console.WriteLine("tr�d 1 startas");
                t1.Start();
                Console.WriteLine("tr�d 2 startas");
                t2.Start();

                t1.Join(); //g�r s� att programet stanar och v�ntar in "tr�den" innan den g�r vidare i programet
                t1.Join(); // allts� den pausar programet tills tasken �r klar

                Console.WriteLine("alla operationer �r f�rdig");
                return Results.Ok("sucses");
            });

            app.Run();
        }
    }
}
