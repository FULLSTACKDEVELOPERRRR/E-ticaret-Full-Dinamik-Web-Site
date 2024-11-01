using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
	public class Product
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Ürün Adı")]
        public string? ProductName { get; set; }

        [DisplayName("Fiyat")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Kategori")]
        public int CategoryID { get; set; }

        [DisplayName("Marka")]

        public int SupplierID { get; set; }

        [DisplayName("Stok")]
        public int Stock { get; set; }

        public int Discount { get; set; }

        [DisplayName("Statü")]
        public int StatusID { get; set; }

        public DateTime AddDate { get; set; }

        public string? Keywords { get; set; }

        //ENCAPSULATION (Kapsülleme)
        private int _Kdv { get; set; }

        public int Kdv
        {
            get { return _Kdv; }
            set
            {
                _Kdv = Math.Abs(value);
            }
        }

        public int HighLighted { get; set; }

        public int TopSeller { get; set; }
        [DisplayName("BunaBakan")]
        public int Related { get; set; }
        public string? Notes { get; set; }

		[DisplayName("Resim Seç")]
		[Required]
		public string? PhotoPath { get; set; }
        

        [DisplayName("Aktif")]
        public bool Active { get; set; } 













    }
}
