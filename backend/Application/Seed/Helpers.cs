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
            context.Reviews.RemoveRange(context.Reviews);
            context.Badges.RemoveRange(context.Badges);
            context.Images.RemoveRange(context.Images);
            // context.Attributes.RemoveRange(context.Attributes);
            // context.Product_FilterAttribute.RemoveRange(context.Product_FilterAttribute);
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


        public static bool CheckIfNew<T>(List<T> oldData, Func<T, bool> f)
        {
            return !oldData.Exists((oldItem) => f(oldItem));
        }
        public static bool CheckIfRemoved<T>(List<T> newData, Func<T, bool> f)
        {
            return newData.Exists((newItem) => f(newItem));
        }

        public static void CreateProduct(Product product, List<Product> products)
        {

            product.id = product.id == Guid.Empty ? Guid.NewGuid() : product.id;
            var badges = new List<Badge>();
            foreach (var badge in product.badges)
            {
                badges.Add(new Badge
                {
                    id = badge.id == Guid.Empty ? Guid.NewGuid() : badge.id,
                    type = "iiuiuiuiu"
                });
            }
            var reviews = new List<Review>();
            foreach (var review in product.reviews)
            {
                reviews.Add(new Review
                {
                    id = review.id == Guid.Empty ? Guid.NewGuid() : review.id,
                    rating = review.rating,
                    createdAt = review.createdAt,
                    text = review.text
                });
            }

            products.Add(new Product
            {
                id = product.id,
                name = product.name,
                slug = product.slug,
                description = product.description,
                price = product.price,
                qunatityInStock = product.qunatityInStock,
                badges = badges,
                images = product.images,
                reviews = product.reviews
            });
        }
        public static async Task SeedProducts(IApplicationDbContext context,
         IMapper mapper,
         CancellationToken cancellationToken)
        {
            Console.WriteLine("new file: " + newJsonData);
            Console.WriteLine("old file: " + oldJsonData);

            var products = new List<Product>();

            if (newJsonData != null && newJsonData.products.Any())
                newJsonData.products.ForEach((product) =>
                {
                    if (oldJsonData != null && CheckIfNew(
                        oldJsonData.products, oldItem => oldItem.name == product.name))
                        CreateProduct(product, products);

                    if (oldJsonData == null)
                        CreateProduct(product, products);
                });


            if (oldJsonData != null && oldJsonData.products.Any())
                oldJsonData.products.ForEach((product) =>
                {
                    if (newJsonData != null && CheckIfRemoved(
                        newJsonData.products, newItem => newItem.name == product.name))
                        CreateProduct(product, products);
                });

            await context.Products.AddRangeAsync(products);
        }
        public static async Task<bool> ReSeedData(IApplicationDbContext context,
         IMapper mapper,
         CancellationToken cancellationToken)
        {
            await PurgeDb(context, cancellationToken);

            string seedFile = Path.Combine(dataDirectory, @"data/", "seedData.json");
            newJsonData = Deserialize(seedFile);

            string databaseFile = Path.Combine(dataDirectory, @"data/", "Database.json");
            oldJsonData = Deserialize(databaseFile);

            await SeedProducts(context, mapper, cancellationToken);
            // var succes2s = await context.SaveChangesAsync(cancellationToken);
            var success = await context.SaveChangesAsync(cancellationToken);

            Serialize("Database.json", mapper, context);
            // var product = await context.Products.FirstOrDefaultAsync(p => p.name == "Hammer");
            var tests = await context.Products.FirstAsync();
            Console.WriteLine("HI: " + tests.price);
            return success > 0;
        }

    }
}
