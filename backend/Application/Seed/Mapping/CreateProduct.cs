using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Seed.Mapping
{
    public static class CreateProduct
    {
        public static Product FromDb(OutputJSON.Product product)
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
        public static Product FromSeed(InputJSON.Product product)
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
    }
}