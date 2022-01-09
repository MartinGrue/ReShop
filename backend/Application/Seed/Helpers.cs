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
            context.Reviews.RemoveRange(context.Reviews);
            context.Badges.RemoveRange(context.Badges);
            context.Images.RemoveRange(context.Images);
            context.Categories.RemoveRange(context.Categories);
            context.Product_Category.RemoveRange(context.Product_Category);
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
            if (oldData == null)
                return true;
            return !oldData.Exists((oldItem) => f(oldItem));
        }
        public static bool CheckIfRemoved<T>(List<T> newData, Func<T, bool> f)
        {
            return newData.Exists((newItem) => f(newItem));
        }

        public static Category CreateCategoryFromSeed(InputJSON.Category category)
        {
            // category.id = category.id == Guid.Empty ? Guid.NewGuid() : category.id;
            return new Category
            {
                id = category.id == Guid.Empty ? Guid.NewGuid() : category.id,
                name = category.name,
                slug = category.slug

            };
            // categories.Add(new Category { id = category.id, name = category.name, slug = category.slug });
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
        public static Category CreateCategoryFromSave(OutputJSON.Category category)
        {
            // category.id = category.id == Guid.Empty ? Guid.NewGuid() : category.id;
            return new Category
            {
                id = category.id,
                name = category.name,
                slug = category.slug
            };
            // categories.Add(new Category { id = category.id, name = category.name, slug = category.slug });
        }
        public static Product CreateProductFromSeed(InputJSON.Product product)
        {
            var productid = product.id == Guid.Empty ? Guid.NewGuid() : product.id;
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

            return new Product
            {
                id = productid,
                name = product.name,
                slug = product.slug,
                description = product.description,
                price = product.price,
                qunatityInStock = product.qunatityInStock,
                badges = badges,
                images = product.images,
                reviews = product.reviews
            };
        }

        public static Product CreateProductFromDb(OutputJSON.Product product)
        {
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

            return new Product
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
            };
        }

        public static void SeedEntity<T>(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            Type myType = typeof(InputJSON);
            PropertyInfo myPropInfo = myType.GetProperty("products");
            Console.WriteLine("The {0} property exists in InputJSON.", myPropInfo.Name);
            var mylist = (List<InputJSON.Product>)myPropInfo.GetValue(newJsonData);
            foreach (var item in mylist)
            {
                Console.WriteLine("item: "+ item.name);
            }
            // var entities = new List<T>();
            // var newEntries = newJsonData.[typeof(prop) == T];
            // var oldEntries = newJsonData.[typeof(prop) == T];

            // if (newJsonData != null && entries != null)
            //     entries.ForEach((pc) =>
            //     {
            //         if (oldJsonData != null && CheckIfNew(
            //             oldEntries, oldItem => (oldItem.productid == pc.productid && oldItem.categoryid == pc.categoryid)))
            //             entities.Add(CreateProduct_CategoryFromSeed(pc));

            //         if (oldJsonData == null)
            //             entities.Add(CreateProduct_CategoryFromSeed(pc));
            //     });
        }

        public static async Task SeedProductCategories(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var prodcut_categories = new List<Product_Category>();

            if (newJsonData != null && newJsonData.product_Categories != null)
                newJsonData.product_Categories.ForEach((pc) =>
                {
                    if (oldJsonData != null && CheckIfNew(
                        oldJsonData.product_Categories, oldItem => (oldItem.productid == pc.productid && oldItem.categoryid == pc.categoryid)))
                        prodcut_categories.Add(CreateProduct_CategoryFromSeed(pc));

                    if (oldJsonData == null)
                        prodcut_categories.Add(CreateProduct_CategoryFromSeed(pc));
                });


            if (oldJsonData != null && oldJsonData.product_Categories != null)
                oldJsonData.product_Categories.ForEach((pc) =>
                {
                    if (newJsonData != null && CheckIfRemoved(
                        newJsonData.product_Categories, newItem => (newItem.productid == pc.productid && newItem.categoryid == pc.categoryid)))
                        prodcut_categories.Add(CreateProduct_CategoryFromSave(pc));
                });

            await context.Product_Category.AddRangeAsync(prodcut_categories);
        }

        public static async Task SeedCategories(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var categories = new List<Category>();

            if (newJsonData != null && newJsonData.categories != null)
                newJsonData.categories.ForEach((category) =>
                {
                    if (oldJsonData != null && CheckIfNew(
                        oldJsonData.categories, oldItem => oldItem.name == category.name))
                        categories.Add(CreateCategoryFromSeed(category));

                    if (oldJsonData == null)
                        categories.Add(CreateCategoryFromSeed(category));
                });


            if (oldJsonData != null && oldJsonData.categories != null)
                oldJsonData.categories.ForEach((category) =>
                {
                    if (newJsonData != null && CheckIfRemoved(
                        newJsonData.categories, newItem => newItem.name == category.name))
                        categories.Add(CreateCategoryFromSave(category));
                });

            await context.Categories.AddRangeAsync(categories);
        }
        public static async Task SeedProducts(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var products = new List<Product>();

            if (newJsonData != null && newJsonData.products != null)
                newJsonData.products.ForEach((product) =>
                {
                    if (oldJsonData != null && CheckIfNew(
                        oldJsonData.products, oldItem => oldItem.name == product.name))
                        products.Add(CreateProductFromSeed(product));

                    if (oldJsonData == null)
                        products.Add(CreateProductFromSeed(product));
                });


            if (oldJsonData != null && oldJsonData.products != null)
                oldJsonData.products.ForEach((product) =>
                {
                    if (newJsonData != null && CheckIfRemoved(
                        newJsonData.products, newItem => newItem.name == product.name))
                        products.Add(CreateProductFromDb(product));
                });

            await context.Products.AddRangeAsync(products);
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

            await SeedCategories(context, cancellationToken);
            await SeedProducts(context, cancellationToken);
            await SeedProductCategories(context, cancellationToken);
            // var succes2s = await context.SaveChangesAsync(cancellationToken);
            SeedEntity<Product>(context, cancellationToken);
            var success = await context.SaveChangesAsync(cancellationToken);

            Serialize("Database.json", mapper, context);
            // var product = await context.Products.FirstOrDefaultAsync(p => p.name == "Hammer");
            var tests = await context.Products.FirstAsync();
            Console.WriteLine("HI: " + tests.price);
            return success > 0;
        }

    }
}
