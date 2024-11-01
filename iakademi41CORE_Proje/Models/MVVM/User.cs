using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi41CORE_Proje.Models.MVVM
{
	public class User
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "AD SOYAD Zorunlu Alan")]
        public string? NameSurname { get; set; }

        
        [StringLength(100)]
        [Required(ErrorMessage = "Email Zorunlu Alan")]
        [EmailAddress(ErrorMessage = "Doğru Email Adresi Girmediniz")]
        public string? Email { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Şifre Zorunlu Alan")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? Telephone { get; set; }
        public string? InvoicesAdress { get; set; }
        public bool IsAdmin { get; set; }
        public bool Active { get; set; }



    }
}
