using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }


        [DisplayName("Telefon")]
        public string? Telephone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }


        [DisplayName("Adres")]
        public string? Adress { get; set; }


        [DisplayName("Asıl Hesap")]
        public int MainpageCount { get; set; }


        [DisplayName("Alt Sayfa")]
        public int SubpageCount { get; set; }
    }
}
