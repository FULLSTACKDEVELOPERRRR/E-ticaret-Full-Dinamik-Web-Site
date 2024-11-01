using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace iakademi41CORE_Proje.Models
{
    public class Cls_Order
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public string? MyCart { get; set; } //Sepet bilgisi
        public decimal UnitPrice { get; set; }
        public string?  ProductName { get; set; }
        public int Kdv { get; set; }
        public string? PhotoPath { get; set; }

        iakademi41Context context = new iakademi41Context();

        public static string tckimlik_vergi_no = "";
        public bool AddToMyCart(string id)
        {
            bool exists = false;    //bu metod bittiginde hala false ise ürün sepete eklendi
            if (MyCart == "")
            {
                MyCart = id + "=" + Quantity; //10=1
            }
            else
            {
                //sepette daha önceden eklenmemiş ürünler var 
                //10=1 & 20=1 & 30=1
                string[] MyCartArray = MyCart.Split('&');
                //10=1 MyCartArray [0]
                //20=1 MyCartArray [1]
                //30=1 MyCartArray [2]

                //eklenecek olan ürün daha önceden eklenmişmi kontrolü yapılıyor forda!
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');

                    if (MyCartArrayLoop[0] ==id )             
                    {
                        //buraya girerse ürün daha önceden eklenmiş
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1";
                }


            }
            return exists;
        }

        public List<Cls_Order> SelectMyCart()
        {
            
            //List<Cls_Order> , sepet  bilgilerini propertylere koyacagım , Property üzerinden dönüş yapılacak
            List<Cls_Order> list = new List<Cls_Order>();

            string[] MyCartArray = MyCart?.Split("&");
            if (MyCart != "")//sepette ürün varken for u yapsın 
            {
                for (int i = 0;i < MyCartArray.Length;i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int ProductID = Convert.ToInt32(MyCartArrayLoop[0]);
                    Product? prd = context.Products.FirstOrDefault(p=> p.ProductID == ProductID);
                    //prd içinde veritabanındaki verileri, propertylere yazdırıyorum.
                    Cls_Order ord = new Cls_Order();
                    ord.ProductID = prd.ProductID;
                    ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    ord.UnitPrice = prd.UnitPrice;
                    ord.ProductName = prd.ProductName;
                    ord.PhotoPath = prd.PhotoPath;  
                    ord.Kdv = prd.Kdv;
                    list.Add(ord);
                }
            }
            return list;
        }
        public void DeleteFromMyCart(string id)
        {
            string[] MyCartArray = MyCart.Split('&'); //ürünler birbirinden ayrıldı
            string NewMyCart = "";
            int count = 1;

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                //ProductID ile adet ayrıldı
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                //MyCartArrayLoop[0] = ProductID var
                //MyCartArrayLoop[1] = Quantity var
                //for her döndüğünde dizinin sıfırıncı alanındaki degeri (10,20,30,40) ProductID ye atadım
                string ProductID = MyCartArrayLoop[0];
                if (ProductID != id)
                {
                    if (count == 1)
                    {
                        //yeni sepetin icine ilk ürünü ekliyorum , & ampersand yok
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        //yeni sepetin icindedaha önce silinmeyecek olan ürün(ler) var , & ampersand ProductID nin önüne ekliyorum
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }
                }
            }
            MyCart = NewMyCart;
        }

        public void EfaturaCreate()
        {
            //...xml dosyası 
        }

        public string OrderCreate(string Email)
        {
            List<Cls_Order> sipList = SelectMyCart();
            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
            DateTime OrderDate = DateTime.Now;  

            foreach (var item in sipList)
            {
                Order order = new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity; 
                context.Orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }

        public List<Vw_MyOrder> SelectMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;

            List<Vw_MyOrder> myOrders = context.Vw_MyOrders.Where(o => o.UserID == UserID).ToList();

            return myOrders;
        }
    }
}

