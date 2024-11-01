

using iakademi41CORE_Proje.Models;
using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using X.PagedList;

namespace iakademi41CORE_Proje.Controllers
{
	public class HomeController : Controller
	{
        Cls_Supplier cls_Supplier = new Cls_Supplier();
		Cls_Product cls_Product= new Cls_Product();
        Cls_User cls_User= new Cls_User();
        Cls_Category cls_Category= new Cls_Category();
        Cls_Order cls_Order= new Cls_Order();
		MainPageModel mpm = new MainPageModel();
        iakademi41Context context = new iakademi41Context();
        public HomeController()
        {
            cls_Product.mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainpageCount;
            cls_Product.subpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;
        }
        public async Task<IActionResult> Index()
		{
			mpm.SliderProducts = cls_Product.ProductSelect("Slider",-1);
			mpm.NewProducts = cls_Product.ProductSelect("New" ,- 1);
			mpm.SpecialProducts = cls_Product.ProductSelect("Special", -1);
			mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", -1);
			mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", -1);
			mpm.TopSellerProducts = cls_Product.ProductSelect("Topseller", -1);
			mpm.StarProducts = cls_Product.ProductSelect("Star", -1);
			mpm.OpportunityProducts = cls_Product.ProductSelect("Opportunity", -1);
			mpm.NotableProducts = cls_Product.ProductSelect("Notable", -1);
			mpm.Productofday = await cls_Product.ProductDetails(0);
            return View(mpm);
		}

		//alt sayfa ilk defa tıklanınca
		public IActionResult NewProducts()
		{
			mpm.NewProducts = cls_Product.ProductSelect("New",0);//yeni
			return View(mpm);
		}

		//alt sayfa ajax yaparken yeni ürünler
		public PartialViewResult _PartialNewProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.NewProducts =cls_Product.ProductSelect("New",pagenumber);
			return PartialView(mpm);
		}

        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cls_Product.ProductSelect("Special", 0);//yeni
            return View(mpm);
        }
        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = cls_Product.ProductSelect("Special", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", 0);//yeni
            return View(mpm);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialDiscountedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", 0);//yeni
            return View(mpm);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.HighLightedProducts = cls_Product.ProductSelect("HighLighted", pagenumber);
            return PartialView(mpm);
        }
        public IActionResult TopsellerProducts(int page = 1)
        {
            //manage nuget pacgages install
            //  X.pagedlist (10.0.3)
            // X.Web.PagedList (10.1.2)
            //  İPTAL -> X.pagedlist.Mvc.Core (8.4.7)

            // eger page = null ise , int page = 1 yaptık
            //using X.PagedList;
            cls_Product.page = page;
            PagedList<Product> model = cls_Product.TopsellerProductsList();

            return View("TopsellerProducts", model);
        }

        public async Task<IActionResult> CategoryPage(int id) //Menüden tıklanınca kategori kısmında açılacak olan sayfa
        {
            List<Product> products = cls_Product.ProductSelect(id, "Category");
            Category category = await cls_Category.CategoryDetails(id);
            ViewBag.Header = category.CategoryName;

            return View(products);
        }
        public async Task<IActionResult> SupplierPage(int id)
        {
            List<Product> products = cls_Product.ProductSelect(id, "Supplier");
            Supplier supplier = await cls_Supplier.SupplierDetails(id);
            ViewBag.Header = supplier.BrandName;

            return View(products);

        }
        public IActionResult CartProcess(int id)
		{
            //hangi sayfadan sepete ekle yapıldı ise ürün sepete eklendikten sonra yine aynı sayfada kalacak.
            string refererUrl = Request.Headers["Referer"].ToString();
            string url = "";

            if (id > 0)
            {
                Cls_Product.Highlighted_Increase(id);//öne çıkanlar kolonunun değerini artırdım.

                cls_Order.ProductID = id;
                cls_Order.Quantity = 1;

                var cookieOptions = new CookieOptions();
                var cookie = Request.Cookies["Sepetim"];//tarayıcıda sepetim isminde ,daha önceden  yaratılmış sepet varmı 

                if (cookie == null)
                {
                    //sepetim diye birşey yok, kullanıcı sepetine henüz birşey eklememiş, dolayısıyla sepetim  diye birsey olusturmamısım
                    cookieOptions.Expires = DateTime.Now.AddDays(1);
                    cookieOptions.Path = "/";
                    cls_Order.MyCart = "";
                    //sepete ekle metodu cagıralacak
                    cls_Order.AddToMyCart(id.ToString());   
                    //propertydeki sepet bilgisini tarayıcıya gönder 
                    Response.Cookies.Append("sepetim",cls_Order.MyCart,cookieOptions);
                    TempData["Message"] = "Ürün sepetinize eklendi";

                }
                else
                {
                    //daha önceden sepete eklenen ürünler var 
                    cls_Order.MyCart = cookie;//tarayıcıdaki sepetim içerisindeki ürünleri property'e gönderdim 

                    if (cls_Order.AddToMyCart(id.ToString()) == false) 
                    {
                        //aynı ürün sepete eklenmediyse , burada sepete ekleme işlemi yapacagım 
                        HttpContext.Response.Cookies.Append("sepetim",cls_Order.MyCart, cookieOptions);
                        TempData["Message"] = "Ürün sepetinize eklendi";

                    }
                    else
                    {
                        //aynı ürün sepete eklenmişse 
                        TempData["Message"] = "Bu ürün zaten sepetinizde var";
                    }
                }
                Uri refererUri = new Uri(refererUrl,UriKind.Absolute);
                url = refererUri.AbsolutePath;

                //Check the path for specific  criteria and redirect accordingly 
                if (url.Contains("DpProducts") || refererUrl.Contains("http:localhost:7210"))   
                {
                    return RedirectToAction("Index");

                }
                return Redirect(url);
            }
            else
            {
                //Handle cases where id is not greater  than 0
                Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
                url = refererUri.AbsolutePath;

                if (url.Contains("DpProducts"))
                {
                    return RedirectToAction("Index");

                }
                return Redirect(url);
            }
		}
		public IActionResult Cart() 
		{
            if (HttpContext.Request.Query["ProductID"]=="")
            {
                var cookie = Request.Cookies["sepetim"];//tarayıcıdan sepet bilgilerini al gel
                if (cookie == null)//sepet boşsa
                {
                    //sağ üst köşede sepet tıklanınca sepet boş..
                    cls_Order.MyCart = "";
                    ViewBag.Sepetim = cls_Order.SelectMyCart(); //icinde kayıt yok count =  0
                }
                else
                {
                    //sağ üst köşede sepet tıklanınca sepette en az bir ürün varsa doldurur.

                    cls_Order.MyCart = Request.Cookies["sepetim"];
                    ViewBag.Sepetim = cls_Order.SelectMyCart(); //sepeti listele
                }
            }
            else
            {
                string? ProductID = HttpContext.Request.Query["ProductID"]; //cshtml sayfasından parametre (metod parametresi olarak yakalanmayacaksa , bu sekilde yakalarız)
                cls_Order.MyCart = Request.Cookies["sepetim"];  //çerezi yakalamanın yolu
                cls_Order.DeleteFromMyCart(ProductID);
                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                TempData["Message"] = "Ürün Sepetten Silindi";
                ViewBag.Sepetim = cls_Order.SelectMyCart();
            }
            return View();
		}

        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                //kullanıcı Login.cshtml den giriş yapıp , Session alıp gelmiştir,Modelle kullanıcının bilgilerini gösterecegim
                User? usr = Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));
                if (usr != null)
                {
                    return View(usr);
                }
                return RedirectToAction("Login");
            }
            else
            {
                //kullanıcı Login.cshtml ye gitmemiş , Session alıp gelmemiş
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            // string kredikartno = Request.Form["kredikartno"]; //1. yol
            // string kredikartay = frm["kredikartno"]; // 2. yol

            string txt_individual = Request.Form["txt_individual"]; //bireysel
            string txt_corporate = Request.Form["txt_corporate"]; //kurumsal

            if (txt_individual != null)
            {
                //bireysel fatura
                //digital planet
                //WebServiceController.tckimlik_vergi_no = txt_individual;
                Cls_Order.tckimlik_vergi_no = txt_individual;
                cls_Order.EfaturaCreate();
            }
            else
            {
                //kurumsal fatura
                WebServiceController.tckimlik_vergi_no = txt_corporate;
                Cls_Order.tckimlik_vergi_no = txt_corporate;
                cls_Order.EfaturaCreate();
            }

            string kredikartno = Request.Form["kredikartno"];
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvs = frm["kredikartcvs"];

            return RedirectToAction("backref");

            //buradan sonraki kodlar , payu , iyzico

            //payu dan gelen örnek kodlar

            /* AŞAGIDAKİ KODLAR GERÇEK HAYATTA AÇILALAK 
            NameValueCollection data = new NameValueCollection();
            string url = "https://www.sedattefci.com/backref";
 
            data.Add("BACK_REF", url);
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", "20" + kredikartyil);
 
            var deger = "";
 
            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }
 
            var signatureKey = "size verilen SECRET_KEY buraya yazılacak";
 
            var hash = HashWithSignature(deger, signatureKey);
 
            data.Add("ORDER_HASH", hash);
 
            var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);
 
            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart (debit kart) ile alış veriş yaptı , bankadan onay aldı
            }
            else
            {
                //gerçek kart ile alış veriş yaptı , bankadan onay aldı
            }
            */
        }

        //Detaylı arama metodu
        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers =  context.Suppliers.ToList();
            return View();
        }

        //Detaylı ARA butonun arkası
        public IActionResult DProducts(int CategoryID,string [] SupplierID, string price,string IsInStock)
        {
            price = price.Replace(" ", "").Replace("$", "");
            string [] PriceArray = price.Split('-');
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];
            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }
            string suppliervalue = ""; //1,2,4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (i == 0)
                {
                    //ilk marka
                    suppliervalue = "SupplierID=" + SupplierID[i];
                }
                else
                {
                    //ikinci ve sonrası markaları getir.
                    suppliervalue += "or SupplierID=" + SupplierID[i];
                }
            }
            string query = "Select from Products where CategoryID=" + CategoryID + "and("+ suppliervalue +")and (UnitPrice >= " + startprice + "and UnitPrice <= " + endprice + ")and Stock " + sign + " 0 order by UnitPrice";
            ViewBag.Products = cls_Product.SelectProductsByDetails(query);
            return View();
        }

        public static int staticid = 0;
        [HttpGet]
		public IActionResult Details(int id)
		{
            if (id >= 0)
            {
                staticid = 0;
            }
            if (id == 0)
            {
                id = staticid;
            }
			Cls_Product.Highlighted_Increase(id);
			mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();
			mpm.CategoryName = (from p in context.Products
								join c in context.Categories on p.CategoryID equals c.CategoryID
								where p.ProductID == id
								select c.CategoryName).FirstOrDefault();
			mpm.BrandName = (from p in context.Products
							 join s in context.Suppliers on p.SupplierID equals s.SupplierID
							 where p.ProductID == id
							 select s.BrandName).FirstOrDefault();
			mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();
			return View(mpm);
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View();	
		}

       
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (Cls_User.loginEmailControl(user) == false)
                {
                    bool answer = Cls_User.AddUser(user);

                    if (answer)
                    {
                        TempData["Message"] = "Kaydedildi.";
                        return RedirectToAction("Login");
                    }
                    TempData["Message"] = "Hata.Tekrar deneyiniz.";
                }
                else
                {
                    TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz.";
                }
            }
            return View();
        }

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(User user)
		{
			string answer = Cls_User.MemberControl(user);
			if (answer == "error")
			{
				HttpContext.Session.Remove("Email");
				HttpContext.Session.Remove("Admin");
				TempData["Message"] = "Email/Şifre yanlış girildi";
				return View();
			}

			else if (answer == "admin")
			{
				HttpContext.Session.SetString("Email", answer);
				HttpContext.Session.SetString("Admin", answer);
				return RedirectToAction("Login", "Admin");
			}
			else
			{
				HttpContext.Session.SetString("Email", answer);
				return RedirectToAction("Index");
			}
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Remove("Email");
			HttpContext.Session.Remove("Admin");
			return RedirectToAction("Index");
		}

        public static string OrderGroupGUID = "";
        public IActionResult backref ()
        {
            //sipariş tablosuna kaydet
            //sepetim cookisinden sepeti temizle
            //e-fatura oluştur metodunu çağır
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {
                cls_Order.MyCart = cookie;
                OrderGroupGUID = cls_Order.OrderCreate(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim");//tarayıcıdan sepeti sil
                                                   //cls_User.Send_Sms(OrderGroupGUID)
                                                   //cls_User.Send_Email(OrderGroupGUID)
            }
            return RedirectToAction("ConfirmPage");
        }
        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }


        //menüden siparişlerim tıklanınca metodu
        public IActionResult MyOrders()
        {
            //daha önceden giriş yapmış
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<Vw_MyOrder> orders = cls_Order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            { //yapmamış
                return RedirectToAction("Login");
            }
        }
        public IActionResult AboutUs(string id)
        {
            // Null kontrolü yap
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.Message = $"You are viewing the About Us section for {id}.";
            }
            else
            {
                ViewBag.Message = "Welcome to the About Us page.";
            }

            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
		}

        public PartialViewResult gettingProducts(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<Sp_Search> ulist = Cls_Product.gettingSearchProducts(id);
            string json = JsonConvert.SerializeObject(ulist);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);
            return PartialView(response);
        }
    }
}

