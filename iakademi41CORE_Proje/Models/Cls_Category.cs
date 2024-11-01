using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iakademi41CORE_Proje.Models
{
	public class Cls_Category
	{
		iakademi41Context context= new iakademi41Context();//contexti sadece static olmayan metotlar çağırabilir.Kullanmak için başa using;
		//aşağıdaki şekilde admincontroller dan çağırıyoruz..
		//List<Category> categories = cls_Category.CategorySelect("all");

		public List<Category> CategorySelect(string value)
		{
			List<Category> categories;
			if (value == "all")
			{
				 categories = context.Categories.ToList();
			}
			else 
			{
				 categories = context.Categories.Where(c => c.ParentID == 0).ToList();
			}
			return categories;  

		}

		public static bool CategoryInsert(Category category)
		{
			try
			{
				using (iakademi41Context context = new iakademi41Context())
				{
					if(category.ParentID == null)
					{
						category.ParentID = 0;
					}
					context.Add(category);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
			
		}
		//Category category =await cls_Category.CategoryDetails(id);
		public async Task<Category> CategoryDetails(int? id)
		{
			Category category =await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
			return category;
		}

		public static bool CategoryUpdate(Category category)
		{
			try
			{
				//metod static olduğu için contexti burada tanımlamak zorundayım
				using (iakademi41Context context = new iakademi41Context())
				{
					if (category.ParentID  == null)
					{
						category.ParentID = 0;
					}

					//category.ParentID= category.ParentID == null ? 0: 1;   (turner if)

					context.Update(category);
					context.SaveChanges();
					return true;

				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		public static bool CategoryDelete(int id)
		{
			try
			{
				using (iakademi41Context context = new iakademi41Context())
				{
					//eski kaydını veritabanında çekiyoruz
					Category? category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
					category.Active = false;

					//eğer silinen ana kategori ise , alt kategoriye bakıp siliyoruz.
					List<Category> categoryList = context.Categories.Where(c => c.ParentID == id).ToList();
					foreach(var item in categoryList)
					{
						item.Active = false;
					}
					context.SaveChanges();
					return true;

				}
			}
			catch(Exception)
			{ return false; }
		}



	}
}
