using iakademi41CORE_Proje.Models;
using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace iakademi41CORE_Proje.ViewComponents
{
    public class Headers : ViewComponent
    {
        iakademi41Context context = new iakademi41Context();

        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.Where(c=> c.Active == true).ToList();
            return View(categories);
        }
    }
}
