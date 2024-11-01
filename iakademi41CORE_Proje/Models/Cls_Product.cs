using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using XAct;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Product
    {
        public int page { get; set; }
        public int subpageCount { get; set; }
        public int mainpageCount { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }
        public string Notes { get; set; }

        iakademi41Context context = new iakademi41Context();
        //metod overload(Aynı isimli metodu farklı parametre sırası ile yazmak)
        //homeindex için productselect
        public List<Product> ProductSelect(string mainPageName, int pagenumber)
        {
            List<Product> products;
            if (mainPageName == "Slider")
            {
                products = context.Products.Where(p => p.StatusID == 1 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();//EFCORE
            }
            else if (mainPageName == "New")
            {
                if (pagenumber == -1)
                {
                    //Home/Index
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(subpageCount).ToList();
                }
                else
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }
            }

            else if (mainPageName == "Special")
            {
                if (pagenumber == -1)
                {
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p => p.ProductName).Take(subpageCount).ToList();
                }
                else
                {
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p => p.ProductName).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }              
            }
            else if (mainPageName == "Discounted")
            {
                if (pagenumber == -1)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(subpageCount).ToList();
                }
                else
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }

                
            }
            else if (mainPageName == "HighLighted")
            {
                if (pagenumber == -1)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
                }
                else if (pagenumber == 0)
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
                }
                else
                {
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                }               
            }
            else if (mainPageName == "TopSeller")
            {
                products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Star")
            {
                products = context.Products.Where(p => p.StatusID == 3 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Opportunity")
            {
                products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Notable")
            {
                products = context.Products.Where(p => p.StatusID == 5 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
            }
            else
            {
                products = context.Products.Take(mainpageCount).ToList();
            }
            return products;
        }
        
        public List<Product>  ProductSelect (int id,string TableName) //Metod overload Product select aynı metod farklı parametreler ile yazıldı.
        {
            List<Product> products;
            if (TableName == "Category")
            {
				products = context.Products.Where(p => p.CategoryID == id).ToList();
			}
            else if (TableName  =="Supplier")
            {
				products = context.Products.Where(p => p.SupplierID == id).ToList();
			}
            else
            {
                products = context.Products.ToList();
            }
            return products;
        }
        public static int NewProductCount()
        {
            using(iakademi41Context context = new iakademi41Context())
            {
                int count = context.Products.Count(p => p.AddDate == DateTime.Now);
                return count;
            }
            
        }

        public static int ProductCount(int? id)
        {
            using iakademi41Context context = new iakademi41Context();
            int count = context.Products.Where(p=> p.SupplierID == id).Count();
            return count;
        }
        public async Task<List<Product>>ProductSelect()
        {
            List<Product> products = await context.Products.OrderBy(p=> p.StatusID).ToListAsync();
            return products;
        }
        public static bool ProductInsert(Product product)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    product.AddDate = DateTime.Now;
                    context.Add(product);
                    context.SaveChanges();
                    return true;
                }

			}
            catch (Exception)
            {
                return false;
			}
        }

        public List<Cls_Product> SelectProductsByDetails(string query)
        {
            List<Cls_Product> products = new List<Cls_Product>();
            SqlConnection sqlConnection = Connection.ServerConnect;
            SqlCommand sqlCommand =  new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Cls_Product product = new Cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                product.Notes = sqlDataReader["Notes"].ToString();
                products.Add(product);
            }
            return products;
        }
        public async Task<Product> ProductDetails(int? id)
        {
            Product? product;
            if (id == 0)//Home/Index=günün ürünü için id=0 parametresi gönderildi.
            {
                product =await context.Products.Where(p=> p.StatusID == 6).FirstOrDefaultAsync();
            }
            else
            {
                //Admin tarafta güncelleme silme detay
                //Home ürünün detay bilgilerini getir
                product = await context.Products.FindAsync(id);
            }
            return product;
        }
        public static bool ProductUpdate(Product product)
        {
            try
            {
                using (iakademi41Context context = new iakademi41Context())
                {
                    context.Update(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool ProductDelete(int? id)
        {
            try
            {
                using (iakademi41Context context =new iakademi41Context())
                {
                    Product? product = context.Products.FirstOrDefault(c => c.ProductID == id);
                    product.Active = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;   
            }
        }

        public PagedList<Product> TopsellerProductsList()
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, subpageCount);

            return model;
        }


        //ürün detayına veya sepete ekle butonuna basınca 1 artırıyoruz 
        public static void Highlighted_Increase(int id)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                //eski kayıtları buluyoruz
                Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
                //eski kaydın Highlighted kolon değerini 1 artırıyorum
                product.HighLighted += 1;
                //veritabanına tekrar geri yazıyorum, sadece Highligted kolonunun  1 artırarak
                context.Update(product);
                context.SaveChanges();
            }
        }
        public static List<Sp_Search> gettingSearchProducts(string id)
        {
            using (iakademi41Context context = new iakademi41Context())
            {
                var products = context.Sp_Searches.FromSqlRaw($"sp_arama {id}").ToList();
                return products;
            }
        }
    }
}
