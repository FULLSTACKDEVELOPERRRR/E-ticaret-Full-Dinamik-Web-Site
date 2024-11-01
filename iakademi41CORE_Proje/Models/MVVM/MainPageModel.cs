namespace iakademi41CORE_Proje.Models.MVVM
{
    public class MainPageModel
    {
        public List<Product>? SliderProducts { get; set; } //slider 
        public List<Product>? NewProducts { get; set; } //yeni ürünler
        public List<Product>? SpecialProducts { get; set; } //özel
        public List<Product>? DiscountedProducts { get; set; } //indirimli 
        public List<Product>? HighLightedProducts { get; set; } //öne çıkanlar
        public List<Product>? TopSellerProducts { get; set; } //çoksatanlar
        public List<Product>? StarProducts { get; set; } //yıldız ürünler
        public List<Product>? OpportunityProducts { get; set; } //fırsat ürünler
        public List<Product>? NotableProducts { get; set; } //dikkat çeken
        public Product? Productofday { get; set; } //günün ürünü
		public Product? ProductDetails { get; set; }//ürün detayı

		public string? CategoryName { get; set; }
		public string? BrandName { get; set; }
		public List<Product> RelatedProducts { get; set; }

	}
}
