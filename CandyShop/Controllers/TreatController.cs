using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CandyShop.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CandyShop.Controllers
{
  [Authorize]
  public class TreatController : Controller
  {
    private readonly CandyShopContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatController(UserManager<ApplicationUser> userManager, CandyShopContext db)
    {
       _userManager = userManager;
      _db = db;
    }
    
    public ActionResult Index()
    {
      return View(_db.Treats.ToList());
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat treat, int FlavorId)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      if (FlavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

   [Authorize(Roles = "Admin")]
   public ActionResult Edit(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat, int FlavorId)
    {
      if (FlavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
      }
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
     public ActionResult Delete(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

     [HttpGet]
    public ActionResult ShowSearch()
    {
      return View();
    }

    [HttpPost]
    public ActionResult ShowSearch(string searchPhrase)
    {
      List<Treat> model = _db.Treats.Where(p => p.Name.ToLower().Contains(searchPhrase.ToLower())).ToList();
      return View("Index", model);
    } 
  }
}