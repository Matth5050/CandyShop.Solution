
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Dynamic;
using CandyShop.Models;

namespace CandyShop.Controllers
{
  public class TreatFlavorController : Controller
  {
    private readonly CandyShopContext _db;
    public TreatFlavorController(CandyShopContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      dynamic mymodel = new ExpandoObject();
      mymodel.Flavors = _db.Flavors.ToList();
      mymodel.Treats = _db.Treats.ToList();
      mymodel.TreatFlavor = _db.TreatFlavors.ToList();
      return View(mymodel);
    }
  }
}