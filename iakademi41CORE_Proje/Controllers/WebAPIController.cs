using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace iakademi41CORE_Proje.Controllers
{
    public class WebAPIController : Controller
    {
        public IActionResult PharmacyOnDuty()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");

            var pharmacy = JsonConvert.DeserializeObject<List<Models.MVVM.Pharmacy>>(json);

            return View(pharmacy);
        }

        public IActionResult ArtAndCulture()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");

            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);

            return View(activite);
        }
        public IActionResult ExchangeRate()
        {
            string url = "http://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            string dolaralis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            ViewBag.dolaralis = dolaralis.Substring(0, 5);

            string dolarsatis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            ViewBag.dolarsatis = dolarsatis.Substring(0, 5);

            string euroalis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            ViewBag.euroalis = euroalis.Substring(0, 5);

            string eurosatis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            ViewBag.eurosatis = eurosatis.Substring(0, 5);

            string AUDdolaralis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/BanknoteBuying").InnerXml;
            ViewBag.AUDdolaralis = AUDdolaralis.Substring(0, 5);

            string AUDdolarsatis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/BanknoteSelling").InnerXml;
            ViewBag.AUDdolarsatis = AUDdolarsatis.Substring(0, 5);

            return View();
        }
        public IActionResult WeatherForecast()
        {
            string apikey = "52b72dad903d5a0244a91d029fce3686";
            string city = "izmir";

            //https://api.openweathermap.org/data/2.5/weather?q=izmir&mode=xml&lang=tr&units=metric&appid=52b72dad903d5a0244a91d029fce3686

            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + apikey;

            XDocument weather = XDocument.Load(url);

            ViewBag.temperature = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;

            return View();
        }
    }
}
