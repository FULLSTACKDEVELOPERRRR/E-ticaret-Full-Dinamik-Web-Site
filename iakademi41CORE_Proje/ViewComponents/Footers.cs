using iakademi41CORE_Proje.Models;
using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace iakademi41CORE_Proje.ViewComponents
{
	public class Footers : ViewComponent
	{
		iakademi41Context context = new iakademi41Context();

		public IViewComponentResult Invoke()
		{
			List<Supplier> suppliers = context.Suppliers.Where(c=> c.Active == true).ToList();	
			return View(suppliers);
		}
		
	}
}
