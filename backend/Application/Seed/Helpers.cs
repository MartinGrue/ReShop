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
using Application.Seed.Mapping;

namespace Application.Seed
{
    public static class SeedHelpers
    {
        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        public static string dataDirectory = Directory.GetParent(projectDirectory).FullName;

#nullable enable
        public static InputJSON? newJsonData;
        public static OutputJSON? oldJsonData;
#nullable disable
        public static void Serialize(string filename, IMapper mapper, IApplicationDbContext context)
        {
            string datapath = Path.Combine(dataDirectory, @"data/", filename);
            var categoriesToWrite = new List<OutputJSON.Category>();
            foreach (var category in context.Categories)
            {
                categoriesToWrite.Add(mapper.Map<Category, OutputJSON.Category>(category));
            }

            var productsToWrite = new List<OutputJSON.Product>();
            foreach (var product in context.Products)
            {
                productsToWrite.Add(mapper.Map<Product, OutputJSON.Product>(product));
            }

            var product_categoriesToWrite = new List<OutputJSON.Product_Category>();
            foreach (var pc in context.Product_Category)
            {
                product_categoriesToWrite.Add(mapper.Map<Product_Category, OutputJSON.Product_Category>(pc));
            }


            var jsonData = new OutputJSON { products = productsToWrite, categories = categoriesToWrite, product_Categories = product_categoriesToWrite };
            string jsonString = JsonSerializer.Serialize(jsonData);
            File.WriteAllText(datapath, jsonString);
        }
        public static T Deserialize<T>(string filename)
        {
            string datapath = Path.Combine(projectDirectory, @"data/", filename);

            if (File.Exists(datapath))
            {
                string jsonString = File.ReadAllText(datapath);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            return default(T);
        }
        public static async Task<bool> PurgeDb(IApplicationDbContext context, CancellationToken cancellationToken)
        {



            context.Product_Category.RemoveRange(context.Product_Category);
            context.Reviews.RemoveRange(context.Reviews);
            context.Badges.RemoveRange(context.Badges);
            context.Images.RemoveRange(context.Images);
            context.Categories.RemoveRange(context.Categories);
            context.Products.RemoveRange(context.Products);
            // context.Product_FilterAttribute.RemoveRange(context.Product_FilterAttribute);

            var saveContext = await context.SaveChangesAsync(cancellationToken);

            return (saveContext > 0);
        }



        public static Product_Category CreateProduct_CategoryFromSave(OutputJSON.Product_Category product_category)
        {
            // category.id = category.id == Guid.Empty ? Guid.NewGuid() : category.id;
            return new Product_Category
            {
                productid = product_category.productid,
                categoryid = product_category.categoryid,
            };
            // categories.Add(new Category { id = category.id, name = category.name, slug = category.slug });
        }

        public static Product_Category CreateProduct_CategoryFromSeed(InputJSON.Product_Category product_category)
        {
            // category.id = category.id == Guid.Empty ? Guid.NewGuid() : category.id;
            return new Product_Category
            {
                productid = product_category.productid,
                categoryid = product_category.categoryid,

            };
            // categories.Add(new Category { id = category.id, name = category.name, slug = category.slug });
        }




        public static List<T> GetPropertyValue<T>(object? obj, string propName)
        {
            var list = new List<T>();
            if (obj == null)
                return list;
            var prop = (List<T>)obj.GetType().GetProperty(propName).GetValue(obj, null);
            if (prop.Any())
            {
                list.AddRange(prop);
                return list;
            }
            return list;
        }
        public static bool CheckIfNew2<G>(IEnumerable<G> oldData, G newItem, Func<G, G, bool> f)
        {
            if (oldData == null)
                return true;
            return !oldData.ToList().Exists((oldItem) => f(oldItem, newItem));
        }
        public static bool CheckIfRemoved2<G>(IEnumerable<G> newData, G oldItem, Func<G, G, bool> f)
        {
            return newData.ToList().Exists((newItem) => f(newItem, oldItem));
        }
        public static List<T> SeedEntity<T, G, E, F>(
            string propname,
            Func<F, F, bool> predicate,
            Func<G, T> createFromSeed,
            Func<E, T> createFromSave) where F : class where T : class
        {
            var newEntries = GetPropertyValue<G>(newJsonData, propname);
            var oldEntries = GetPropertyValue<E>(oldJsonData, propname);

            var entities = new List<T>();
        


            if (newJsonData != null && newEntries != null)
                newEntries.ToList().ForEach((newItem) =>
                {
                    if (oldJsonData == null)
                        entities.Add(createFromSeed(newItem));
                    if (oldJsonData != null && !oldEntries.Any() && CheckIfNew2<F>(oldEntries as IEnumerable<F>, newItem as F, predicate))
                        entities.Add(createFromSeed(newItem));
                });

            if (oldJsonData != null && oldEntries != null)
                oldEntries.ToList().ForEach((oldItem) =>
                {
                    if (newJsonData != null && CheckIfRemoved2(newEntries as IEnumerable<F>, oldItem as F, predicate))
                        entities.Add(createFromSave(oldItem));
                });
            return entities;
        }

        public static async Task<bool> ReSeedData(IApplicationDbContext context,
         IMapper mapper,
         CancellationToken cancellationToken)
        {
            await PurgeDb(context, cancellationToken);

            string seedFile = Path.Combine(dataDirectory, @"data/", "seedData.json");
            newJsonData = Deserialize<InputJSON>(seedFile);

            string databaseFile = Path.Combine(dataDirectory, @"data/", "Database.json");
            oldJsonData = Deserialize<OutputJSON>(databaseFile);

            // await SeedProducts(context, cancellationToken);
            var products = SeedEntity<Product, InputJSON.Product, OutputJSON.Product, BaseJSON.Product>(
                "products", (oldItem, newItem) => oldItem.name == newItem.name, CreateProduct.FromSeed, CreateProduct.FromDb);
            await context.Set<Product>().AddRangeAsync(products);

            // await SeedProductCategories(context, cancellationToken);
            var categories = SeedEntity<Category, InputJSON.Category, OutputJSON.Category, BaseJSON.Category>(
                "categories", (oldItem, newItem) => oldItem.name == newItem.name, CreateCategory.FromSeed, CreateCategory.FromDb);
            await context.Set<Category>().AddRangeAsync(categories);

            var success = await context.SaveChangesAsync(cancellationToken);

            Serialize("Database.json", mapper, context);
            // var product = await context.Products.FirstOrDefaultAsync(p => p.name == "Hammer");
            // var tests = await context.Products.FirstAsync();
            // Console.WriteLine("HI: " + tests.price);
            return success > 0;
        }

    }
}
