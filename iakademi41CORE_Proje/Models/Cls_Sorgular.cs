using iakademi41CORE_Proje.Models.MVVM;

namespace iakademi41CORE_Proje.Models
{
	public class Cls_Sorgular
	{
		iakademi41Context context= new iakademi41Context();
		public void Sorgular()
		{
			//entityframeworkcore (ado.net)
			//bir kaydın bütün kolonların bilgisi
			//tek bir ürün sorgusu 10 numaral
			Product? product = context.Products.FirstOrDefault(p => p.ProductID ==10);


			//ÜRÜNLER sorugu
			List<Product> products = context.Products.ToList();


			//TEK BİR ÜRÜN SADECE BİR KOLONU (CATEGORYNAME SORGUSU)
			string categoryname = context.Categories.FirstOrDefault(c => c.CategoryID == 5).CategoryName;

			decimal fiyat = context.Products.FirstOrDefault(p => p.ProductID == 5).UnitPrice;





		}
	}
}
