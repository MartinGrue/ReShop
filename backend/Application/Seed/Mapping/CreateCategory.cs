using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Seed.Mapping
{
    public static class CreateCategory
    {
        public static Category FromDb(OutputJSON.Category category)
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

        public static Category FromSeed(InputJSON.Category category)
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
    }
}