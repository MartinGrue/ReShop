using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using System.Threading;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Seed
{
    public static class SeedHelpers
    {
        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        public static string dataDirectory = Directory.GetParent(projectDirectory).FullName;
        public static SeedData? newJsonData;
        public static SeedData? oldJsonData;
        public static void Serialize(string filename, IMapper mapper, IApplicationDbContext context)
        {
            string datapath = Path.Combine(dataDirectory, @"data/", filename);
            var productsToWrite = new List<ProductJSON>();

            foreach (var product in context.Products)
            {
                var fromMapper = mapper.Map<Product, ProductJSON>(product);
                productsToWrite.Add(fromMapper);
            }

            var jsonData = new JSONData { products = productsToWrite };
            string jsonString = JsonSerializer.Serialize(jsonData);
            File.WriteAllText(datapath, jsonString);
        }
        public static SeedData? Deserialize(string filename)
        {
            string datapath = Path.Combine(projectDirectory, @"data/", filename);

            if (File.Exists(datapath))
            {
                string jsonString = File.ReadAllText(datapath);
                return JsonSerializer.Deserialize<SeedData>(jsonString);
            }
            return null;
        }
        public static async Task<bool> PurgeDb(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            context.Products.RemoveRange(context.Products);

            var saveContext = await context.SaveChangesAsync(cancellationToken);

            return (saveContext > 0);
        }
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
        public static void CheckIfNew<T>(T item, List<T> buffer, List<T> jsondata)
        {
            if (typeof(T) == typeof(Product))
            {
                Product product = (Product)(object)item;
                List<Product> oldproducts = (List<Product>)(object)jsondata;
                if (oldJsonData != null && !oldproducts.Exists((oldproduct) => oldproduct.name == product.name))
                    buffer.Add(item);
            }
        }
        public static void CheckIfRemoved<T>(T item, List<T> buffer, List<T> jsondata)
        {
            if (typeof(T) == typeof(Product))
            {
                Product product = (Product)(object)item;
                List<Product> newproducts = (List<Product>)(object)jsondata;
                if (newJsonData != null && newproducts.Exists((newproduct) => newproduct.name == product.name))
                    buffer.Add(item);
            }
        }

        public static List<T> Create<T>(IMapper mapper)
        {

            var buffer = new List<T>();
            var newdatabuffer = new List<T>();
            var olddatabuffer = new List<T>();

            foreach (PropertyInfo propertyInfo in typeof(SeedData).GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(List<T>))
                {
                    newdatabuffer = (List<T>)propertyInfo.GetValue(newJsonData);
                }
            }

            foreach (PropertyInfo propertyInfo in typeof(JSONData).GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(List<T>))
                {
                    olddatabuffer = (List<T>)propertyInfo.GetValue(oldJsonData);
                }
            }


            if (newJsonData != null && newdatabuffer.Any())
                newdatabuffer.ForEach((item) =>
                {
                    if (oldJsonData == null)
                        buffer.Add(item);
                    CheckIfNew(item, buffer, olddatabuffer);
                });


            if (oldJsonData != null && olddatabuffer.Any())
                olddatabuffer.ForEach((item) => CheckIfRemoved(item, buffer, olddatabuffer));

            return buffer;
        }
        public static async Task SeedEntity<T>(IApplicationDbContext context, IMapper mapper, CancellationToken cancellationToken) where T : class
        {
            var buffer = Create<T>(mapper);
            DbSet<T> Set = context.Set<T>();

            await Set.AddRangeAsync(buffer);
            // await context.SaveChangesAsync();


        }
        public static async Task<bool> ReSeedData(IApplicationDbContext context, IMapper mapper, CancellationToken cancellationToken)
        {
            await PurgeDb(context, cancellationToken);

            string seedFile = Path.Combine(dataDirectory, @"data/", "seedData.json");
            newJsonData = Deserialize(seedFile);

            string databaseFile = Path.Combine(dataDirectory, @"data/", "Database.json");
            oldJsonData = Deserialize(databaseFile);


            await SeedEntity<Product>(context, mapper, cancellationToken);
            // var succes2s = await context.SaveChangesAsync(cancellationToken);
            var success = await context.SaveChangesAsync(cancellationToken);
            Console.WriteLine("sadsasadd" + success);


            Serialize("Database.json", mapper, context);
            var product = await context.Products.FirstOrDefaultAsync(p => p.name == "Hammer");
            Console.WriteLine("HI: " + product.price);
            return success > 0;
        }

    }
}
