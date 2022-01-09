
// using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Badge> Badges { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<FilterAttribute> Attributes { get; set; }
        DbSet<Product_FilterAttribute> Product_FilterAttribute { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Product_Category> Product_Category { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<T> Set<T>() where T : class;
    }
}
