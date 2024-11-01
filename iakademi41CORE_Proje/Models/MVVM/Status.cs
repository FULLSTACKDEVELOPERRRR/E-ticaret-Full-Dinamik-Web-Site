using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi41CORE_Proje.Models.MVVM
{
	public class Status
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("ID")]
		public int StatusID { get; set; }


		[DisplayName("Statü Adı")]
		[StringLength(100)]
        [Required]
        public string? StatusName { get; set; }


		[DisplayName("Aktif/Pasif")]
		public bool Active { get; set; }
    }
}
