using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API
{

    public class JSONData
    {
        public List<ProductJSON> products { get; set; }
    }

    public class SeedData
    {
        public List<Product> products { get; set; }

    }
    public static class SmallSeed
    {
        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        public static SeedData? newJsonData;
        public static SeedData? oldJsonData;
        private static void Serialize(string filename, IMapper mapper, DataContext context)
        {
            string datapath = Path.Combine(projectDirectory, @"data/", filename);
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
        private static SeedData? Deserialize(string datapath)
        {
            if (File.Exists(datapath))
            {
                string jsonString = File.ReadAllText(datapath);
                return JsonSerializer.Deserialize<SeedData>(jsonString);
            }
            return null;
        }
        public static async Task<bool> PurgeDb(DataContext context)
        {
            context.Products.RemoveRange(context.Products);

            var saveContext = await context.SaveChangesAsync();

            return (saveContext > 0);
        }
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
        public static async Task<bool> ReSeedData(DataContext context, IMapper mapper)
        {
            await PurgeDb(context);

            string seedFile = Path.Combine(projectDirectory, @"data/", "seedData.json");
            newJsonData = Deserialize(seedFile);

            string databaseFile = Path.Combine(projectDirectory, @"data/", "Database.json");
            oldJsonData = Deserialize(databaseFile);


            await Seed<Product>(context, mapper);
            var success = await context.SaveChangesAsync();
            Serialize("Database.json", mapper, context);

            return success > 0;
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

        public static List<T> Create<T>(DataContext context, IMapper mapper)
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

        // public static async Task SeedFollowerFollowings(DataContext context,
        //     UserManager<AppUser> userManager, IPhotoAccessor photoAccessor)
        // {
        //     var FollowerFollowings = new List<FollowerFollowings>();
        //     database.followerFollowings.ForEach((ff) =>
        //      FollowerFollowings.Add(new FollowerFollowings { UserAId = ff.UserAId, UserBId = ff.UserBId }));
        //     await context.FollowerFollowings.AddRangeAsync(FollowerFollowings);
        // }
        // public static void CreateProduct(Product product, List<Product> products)
        // {
        //     product.id = product.id == Guid.Empty ? Guid.NewGuid() : product.id;

        //     products.Add(new Product
        //     {
        //         id = product.id,
        //         name = product.name,
        //         description = product.description,
        //         price = product.price,
        //         qunatityInStock = product.qunatityInStock,
        //     });
        // }
        static async Task Seed<T>(DataContext context, IMapper mapper) where T : class
        {
            var buffer = Create<T>(context, mapper);
            DbSet<T> Set = context.Set<T>();
            await Set.AddRangeAsync(buffer);
        }
    }
}