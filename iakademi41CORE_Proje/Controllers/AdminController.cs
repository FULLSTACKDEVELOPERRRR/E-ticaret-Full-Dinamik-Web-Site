using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using iakademi41CORE_Proje.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace iakademi41CORE_Proje.Controllers
{
	public class AdminController : Controller
	{
		Cls_Supplier cls_Supplier = new Cls_Supplier();
		Cls_User cls_User = new Cls_User();
		Cls_Category cls_Category = new Cls_Category();
		Cls_Status cls_Status = new Cls_Status();
		Cls_Product cls_Product = new Cls_Product();
		Cls_Setting cls_Setting = new Cls_Setting(); 
		iakademi41Context context = new iakademi41Context();
		IHostingEnvironment _environment;

        public AdminController(IHostingEnvironment environment)
        {
            _environment = environment;

        }
        

        [HttpGet]
		public IActionResult Login()
		{
			//Sayfanın tasarımını oluşturcak
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task <IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
		{
			//[Bind("Email,Password")],  .cshtml sayfasından sadece bu bilgiler kabul edilecek
			if (ModelState.IsValid)
			{
				//.cshtml sayfasındaki bütün zorunlu alanlar ok ise buraya girecek
				//User clasındaki [Required] , EmailAdress

				User? usr =await cls_User.loginControl(user);
				if (usr != null)
				{
					return RedirectToAction("Index");//Admin/Index sayfasına gider
				}
				else
				{
					ViewBag.error = "Login ve/Veya şifre yanlış";
				}
				//Sayfada formdan girilen veriler buraya gelicek
				//ben bütün metodlarda zaman kaybetmemek için 
				//Bind,ModelState,IsValid
				//kendin yazcan

			}
			return View();//login sayfasında kalmaya devam edicek.

		}
		[HttpGet]
		public IActionResult Index()
		{

			ViewBag.count = Cls_Product.NewProductCount();
			//Sayfanın tasarımını oluşturcak
			return View();
		}
		[HttpGet]	
		public IActionResult CategoryIndex()
		{
			List<Category> categories = cls_Category.CategorySelect("all");
			return View(categories);
		}

		[HttpGet]
		public IActionResult CategoryCreate()
		{
			//bütün yazılım dillerinde  ortak = casting = tip dönüştürme = Convert.ToInt32
			//örnek convert:
			//int yas= ConvertToInt32(txt_yas);
			//int yas=(int)txt_yas;(IEnumerable<SelectListItem>)ViewData["CategoryList"]
			//int yas= txt_yas as int;ViewData["CategoryList"] as (IEnumerable<SelectListItem>)

			CategoryFill("main");
			return View();
		}
		
		void CategoryFill(string main_or_all)
		{
			List <Category> categories = cls_Category.CategorySelect(main_or_all);
			ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
		}
		[HttpPost]
		public IActionResult CategoryCreate(Category category)
		{
			if (ModelState.IsValid)
			{
				bool answer = Cls_Category.CategoryInsert(category);
				if(answer == true)
				{
					TempData["Message"] = "Eklendi";
				}
				else
				{
					TempData["Message"] = "HATA";
				}
			}
			return RedirectToAction("CategoryCreate");//HtppGet
		}
		[HttpGet]
		public async Task<IActionResult>CategoryEdit(int? id)
		{
			CategoryFill("main");
			if (id == null || context.Categories == null)
			{
				return NotFound();
			}
			var category = await cls_Category.CategoryDetails(id);
			return View(category);
		}
		[HttpPost]
		public IActionResult CategoryEdit(Category category)
		{
			bool answer = Cls_Category.CategoryUpdate(category);
			if (answer == true)
			{
				//Türkçe karakter sorunu için,Program.cs içinde
				//builder.Services.AddWebEncoders(); eklendi
				TempData["Message"] = category.CategoryName + "Kategorisi Güncellendi";
				return RedirectToAction("CategoryIndex");

			}
			else
			{
				TempData["Message"] = "HATA";
				return View();
				return RedirectToAction(nameof(CategoryEdit));//[HttpGet]
			}
		}
		[HttpGet]
		public async Task<IActionResult> CategoryDelete(int? id)
		{
			if(id == null || context.Categories == null)
			{
				return NotFound();	
			}

			var category = await cls_Category.CategoryDetails(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost, ActionName("CategoryDelete")] //Routing..
		public IActionResult CategoryDeleteConfirmed(int id)
		{
			bool answer = Cls_Category.CategoryDelete(id);
			if (answer == true)
			{
				TempData["Message"] = "Silindi";
				return RedirectToAction("CategoryIndex");
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(CategoryDelete));
			}
		}
		public async Task<IActionResult> CategoryDetails(int? id)
		{
			//using Microsoft.EntityFrameWorkCore ; ctrl+
			var category =await cls_Category.CategoryDetails(id);
			ViewBag.CategoryName = category?.CategoryName; //menüde detay tıklanırken problem yaşanmaması için
            return View(category);

		}
		public IActionResult SupplierIndex()
		{
			List<Supplier> suppliers = cls_Supplier.SupplierSelect();
			return View(suppliers);
		}
		[HttpGet]
		public IActionResult SupplierCreate()
		{
			//bütün yazılım dillerinde  ortak = casting = tip dönüştürme = Convert.ToInt32
			//örnek convert:
			//int yas= ConvertToInt32(txt_yas);
			//int yas=(int)txt_yas;(IEnumerable<SelectListItem>)ViewData["CategoryList"]
			//int yas= txt_yas as int;ViewData["CategoryList"] as (IEnumerable<SelectListItem>)

			return View();
		}
		
		[HttpPost]
		public IActionResult SupplierCreate(Supplier supplier)
		
		{
			if (ModelState.IsValid)
			{
				bool answer = Cls_Supplier.SupplierInsert(supplier);
				if (answer == true)
				{
					TempData["Message"] = supplier.BrandName.ToUpper() + "Markası eklendi";
				}
				else
				{
					TempData["Message"] = "HATA";
				}
			}
			return RedirectToAction("SupplierCreate");//HtppGet
		}
		[HttpGet]
		public async Task<IActionResult> SupplierEdit(int? id)
		{
			if (id == null || context.Suppliers == null)
			{
				return NotFound();
			}
			var supplier = await cls_Supplier.SupplierDetails(id);
			return View(supplier);
		}
		[HttpPost]
		public IActionResult SupplierEdit(Supplier supplier)
		{
			if (supplier.PhotoPath == null)
			{
				//eski resim kaydını muhafaza etmek için;
				supplier.PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath.ToString();
			}
			bool answer = Cls_Supplier.SupplierUpdate(supplier);
			if (answer == true)
			{
				//Türkçe karakter sorunu için,Program.cs içinde
				//builder.Services.AddWebEncoders(); eklendi
				TempData["Message"] = "Güncellendi";
				
				return RedirectToAction(nameof(SupplierIndex));

			}
			else
			{
				TempData["Message"] = "HATA";
				
				return RedirectToAction(nameof(SupplierEdit));//[HttpGet]
			}
		}

		[HttpGet]
		public async Task<IActionResult> SupplierDelete(int? id)
		{
			if (id == null || context.Suppliers == null)
			{
				return NotFound();
			}

			var supplier = await cls_Supplier.SupplierDetails(id);
			if (supplier == null)
			{
				return NotFound();
			}
			return View(supplier);
		}

		[HttpPost, ActionName("SupplierDelete")] //Routing..
		public IActionResult SupplierDeleteConfirmed(int id)
		{
			bool answer = cls_Supplier.SupplierDelete(id);
			if (answer == true)

			{
				TempData["Message"] = "Silindi";
				return RedirectToAction("SupplierIndex" , new {status ="1"});
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(SupplierDelete));
			}
		}
		public async Task<IActionResult> SupplierDetails(int? id)
		{
			//using Microsoft.EntityFrameWorkCore ; ctrl+
			var supplier = await cls_Supplier.SupplierDetails(id);
			ViewBag.ProductCount = Cls_Product.ProductCount(id);
			return View(supplier);

		}

		public IActionResult StatusIndex()
		{
			List<Status> statuses = cls_Status.StatusSelect();
			return View(statuses);//html sayfasında modele vericek.
		}

		[HttpGet]
		public IActionResult StatusCreate()

		{
			return View();
		}

		[HttpPost]
		public IActionResult StatusCreate(Status status)
		{
			bool answer=Cls_Status.StatusInsert(status);
			if (answer == true) 
			{
				TempData["Message"] = "Eklendi";
		    }
			else
			{
				TempData["Message"] = "HATA";
			}
			return RedirectToAction(nameof(StatusCreate));
		}
		[HttpGet]
		public async Task<IActionResult> StatusEdit(int? id)
		{
			if (id == null || context.Statuses == null)
			{
				return NotFound();
			}
			var statuses =await cls_Status.StatusDetails(id);
			return View(statuses);
		}
		[HttpPost]
		public IActionResult StatusEdit(Status status)
		{
			bool answer = Cls_Status.StatusUpdate(status);
			if (answer == true)
			{
				TempData["Message"] = "Güncellendi";
				return RedirectToAction(nameof(StatusIndex));
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(StatusEdit));
			}
		}

		public async Task<IActionResult> StatusDetails(int? id)
		{
			var status = await cls_Status.StatusDetails(id);
			ViewBag.statusname = status?.StatusName;

			return View(status);
		}
		[HttpGet]
		public async Task<IActionResult> StatusDelete(int? id)
		{
			bool answer = Cls_Status.StatusDelete(id);
			if (answer == true)
			{
				TempData["Message"] = "Silindi";
				return RedirectToAction(nameof(StatusIndex));
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(StatusIndex));
            }
        }
		
		public async Task <IActionResult> ProductIndex()
		{
			List<Product> products = await cls_Product.ProductSelect();
			return View(products);
		}
		[HttpGet]
		public async Task<IActionResult> ProductCreate()
		{
			CategoryFill("all");
			SupplierFill();
			StatusFill();

			return View();
		}
		void SupplierFill()
		{
			List<Supplier> suppliers = cls_Supplier.SupplierSelect();
			ViewData["SupplierList"] = suppliers.Select(c => new SelectListItem { Text =c.BrandName , Value = c.SupplierID.ToString() });
		}

		void StatusFill()
		{
			List<Status> statuses = cls_Status.StatusSelect();
			ViewData["StatusesList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
		}
		[HttpPost]
		public IActionResult ProductCreate([FromForm]Product product)
		{
			var file = Request.Form.Files.FirstOrDefault();  //first or defaultla kontrol ettik
			if (file == null)
			{
				TempData["Message"] = "HATA";
			}
			else
			{
				string _FileName = Path.GetFileName(file.FileName);  
				string _path = Path.Combine(_environment.WebRootPath + "/resimler", _FileName);
				file.CopyTo(new FileStream(_path, FileMode.CreateNew));
				product.PhotoPath = _FileName;
				bool createResult = Cls_Product.ProductInsert(product);
				if (createResult == true)
				{
					TempData["Message"] = "Eklendi";
				}
				else
				{
					TempData["Message"] = "HATA";
				}
			}			
			return RedirectToAction(nameof(ProductCreate));
		}

		public async Task<IActionResult> ProductEdit(int? id)
		{
			CategoryFill("all");
			SupplierFill();
			StatusFill();

			if(id == null || context.Products == null)
			{
				return NotFound();
			}
			var product = await cls_Product.ProductDetails(id);
			return View(product);
		}
		[HttpPost]
		public IActionResult ProductEdit(Product product)
		{
			Product prd = context.Products.FirstOrDefault(s=> s.ProductID == product.ProductID);

			product.AddDate = prd.AddDate;
			product.HighLighted = prd.HighLighted;
			product.TopSeller = prd.TopSeller;

			if(product.PhotoPath == null)
			{
				string? PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
				product.PhotoPath = PhotoPath;
			}
			
			bool answer = Cls_Product.ProductUpdate(product);
			if (answer == true) 
			{
				TempData["Message"] = "Güncellendi";
				return RedirectToAction("ProductIndex");
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(ProductEdit));
			}
		}

		public async Task<IActionResult>ProductDetails(int? id)
		{
			var product=await cls_Product.ProductDetails(id);
			return View(product);
		}
		[HttpGet]
		public async Task<IActionResult> ProductDelete(int? id)
		{
			if (id == null || context.Products == null)
			{
				return NotFound();
			}
			var product = await cls_Product.ProductDetails(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
		[HttpPost,ActionName("ProductDelete")]//routing ...
		public async Task<IActionResult> ProductDeleteConfirmed(int? id)
		{
			bool answer = Cls_Product.ProductDelete(id);
			if (answer == true)
			{
				TempData["Message"] = "Silindi";
				return RedirectToAction("ProductIndex");
            }
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(ProductDelete));
			}
		}

		[HttpGet]
		public async Task<IActionResult> SettingEdit()
		{
			var setting = await cls_Setting.SettingDetails();

			return View(setting);
		}
		[HttpPost]
		public IActionResult SettingEdit(Setting setting)
		{
			bool answer=Cls_Setting.SettingUpdate(setting);
			if (answer == true)
			{
				TempData["Message"] = "Güncellendi";
			}
			else
			{
				TempData["Message"] = "HATA";
			}
			return RedirectToAction(nameof(SettingEdit));
		}


	}
}






