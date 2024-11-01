using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM



{
	public class Supplier
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Marka Adı")]
        //reuglar expression  //[RegularExpression(@"^[a-zA-Z]*$")]
        public string? BrandName { get; set; }

        //reuglar expression
        [DisplayName("Resim Seç")]
        [Required]
        public string? PhotoPath { get; set; }

		[DisplayName("Aktif/Pasif")]
		public bool Active { get; set; }
	}
}
