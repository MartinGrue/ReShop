using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;
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
        public static SeedData? newdata;
        public static SeedData? database;
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
                Console.WriteLine("found" + datapath);
                string jsonString = File.ReadAllText(datapath);
                return JsonSerializer.Deserialize<SeedData>(jsonString);
                // Console.WriteLine("target" + target.products[1].name);
            }
            return null;
        }
        public static async Task<bool> PurgeDb(DataContext context)
        {
            context.Products.RemoveRange(context.Products);

            var saveContext = await context.SaveChangesAsync();

            return (saveContext > 0);
        }
        public static async Task<bool> ReSeedData(DataContext context, IMapper mapper)
        {
            await PurgeDb(context);

            string seedFile = Path.Combine(projectDirectory, @"data/", "seedData.json");
            newdata = Deserialize(seedFile);

            string databaseFile = Path.Combine(projectDirectory, @"data/", "Database.json");
            database = Deserialize(databaseFile);


            await SeedProducts(context);

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

        // public static async Task SeedFollowerFollowings(DataContext context,
        //     UserManager<AppUser> userManager, IPhotoAccessor photoAccessor)
        // {
        //     var FollowerFollowings = new List<FollowerFollowings>();
        //     database.followerFollowings.ForEach((ff) =>
        //      FollowerFollowings.Add(new FollowerFollowings { UserAId = ff.UserAId, UserBId = ff.UserBId }));
        //     await context.FollowerFollowings.AddRangeAsync(FollowerFollowings);
        // }
        public static void CreateProduct(Product product, List<Product> products)
        {
            product.id = product.id == Guid.Empty ? Guid.NewGuid() : product.id;

            products.Add(new Product
            {
                id = product.id,
                name = product.name,
                description = product.description,
                price = product.price,
                qunatityInStock = product.qunatityInStock,
            });
        }
        public static async Task SeedProducts(DataContext context)
        {
            var products = new List<Product>();
 
            if (newdata != null && newdata.products.Any())
            {
                newdata.products.ForEach((newproduct) =>
                {
                    if(database == null){
                     CreateProduct(newproduct, products);
                    }
                    if (database != null && !database.products.Exists((oldproduct) => oldproduct.name == newproduct.name))
                        CreateProduct(newproduct, products);
                });
            }


            if (database != null && database.products.Any())
            {
                database.products.ForEach((product) => 
                {
                    if(newdata.products.Exists((newproduct) => newproduct.name == product.name))
                    CreateProduct(product, products);
                });
            }


            await context.Products.AddRangeAsync(products);
        }
    }
}