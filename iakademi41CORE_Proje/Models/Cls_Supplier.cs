using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XAct;



namespace iakademi41CORE_Proje.Models
{
	public class Cls_Supplier
	{
		iakademi41Context context = new iakademi41Context();//contexti sadece static olmayan metotlar çağırabilir.Kullanmak için başa using;
															//aşağıdaki şekilde admincontroller dan çağırıyoruz..
															//List<Category> categories = cls_Category.CategorySelect("all");

		public List<Supplier> SupplierSelect()
		{
			List<Supplier> suppliers = context.Suppliers.ToList();
			return suppliers;

		}

		public static bool SupplierInsert(Supplier supplier)
		{
			try
			{
				using (iakademi41Context context = new iakademi41Context())
				{
				
					context.Add(supplier);
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
		public async Task<Supplier> SupplierDetails(int? id)
		{
			//Supplier supplier = await context.Suppliers.FirstOrDefaultAsync(p => p.SupplierID == id);
			Supplier? supplier =await context.Suppliers.FindAsync(id);
			return supplier;
		}

		public static bool SupplierUpdate(Supplier supplier)
		{
			try
			{
				//metod static olduğu için contexti burada tanımlamak zorundayım
				using (iakademi41Context context = new iakademi41Context())
				{
				

					

					context.Update(supplier);
					context.SaveChanges();
					return true;

				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool SupplierDelete(int id)
		{
			
			try
			{
				//gelen id ile  databaseden eski kaydını alıyorum , Active kolonuna false basıyorum
				Supplier supplier= context.Suppliers.FirstOrDefault(s=> s.SupplierID == id);
				supplier.Active = false;
				context.SaveChanges();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
