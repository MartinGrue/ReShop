namespace Domain
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public long price { get; set; }
        public string? brand { get; set; }
        public int qunatityInStock { get; set; }
        
        
        // //features
        // public string? slug { get; set; }
        // public virtual ICollection<Image> images { get; set; }
        // public int? rating { get; set; }
        // public int? review { get; set; }

        // //string | string[] | enum?
        // public string? categories { get; set; }
        // public ICollection<Attributes>? Name { get; set; }

        // //more features
        // public long? compareAtPrice { get; set; }
        // public ProductBadges? badges { get; set; }

    }
}