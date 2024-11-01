using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
	public class Category
	{
		//sqldeki primary key , identity =Yes
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("ID")] 
        public int CategoryID { get; set; }


		[DisplayName("Kategori Adı")] //Formda görünecek kısım
		[Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
		[StringLength(150, ErrorMessage = "En fazla 50 karakter")]

		public string? CategoryName { get; set; }

        [DisplayName("Üst Kategori")]
        public int ParentID { get; set; } //ÜSt ID

        [DisplayName("Aktif/Pasif")]
        public bool Active  { get; set; }

		
    }

}
