using Microsoft.AspNetCore.Mvc;

namespace Sangiaodich.Controllers
{
    public class BodyHomeTBodyController(BackgroundServicesGetPrice getPrice) : Controller
    {
        private readonly BackgroundServicesGetPrice _getPrice = getPrice;

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetPrice()
        {
            var data = _getPrice.LstMCKHTML;
            return Json(data);
        }
    }
}
