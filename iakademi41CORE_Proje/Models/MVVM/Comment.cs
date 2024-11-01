using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi41CORE_Proje.Models.MVVM
{
	public class Comment
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }

        //StringLength(150,MinimumLength=10)
        [Range(10, 150)]
        [DisplayName("Yorum")]
        public string? Review { get; set; }
    }
}
