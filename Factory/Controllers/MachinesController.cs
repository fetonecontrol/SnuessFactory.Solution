using Microsoft.AspNetCore.Mvc;
using EngineerOffice.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EngineerOffice.Controllers
{
  public class MachinesController : Controller
  {
    private readonly EngineerOfficeContext _db;

    public MachinesController(EngineerOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Machines.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machines machine, int EngineerId)
    {
      _db.Machines.Add(machine);
      if (EngineerId != 0)
      {
        _db.MachineEngineer.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisMachine = _db.Machines // machine table
      .Include(machine => machine.Engineers) // engineers hashset
      .ThenInclude(join => join.Engineer) // machine-engineer relationship
      .FirstOrDefault(machine => machine.MachineId == id); // specific machine info
      return View(thisMachine);
    }

    public ActionResult Edit(int id, int MachineId)
    {
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Type");
    
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machines machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.MachineEngineer.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.Entry(machine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddEngineer(int id)
    {
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(thisMachine);
    }
    [HttpPost]
    public ActionResult AddEngineer(Machines machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.MachineEngineer.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult DeleteCateogry(int joinId)
    {
      var joinEntry = _db.MachineEngineer.FirstOrDefault(entry => entry.MachineEngineerId == joinId);
      _db.MachineEngineer.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
        public ActionResult Delete(int id)
        {
          var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
          return View(thisMachine);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
          var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
          _db.Machines.Remove(thisMachine);
          _db.SaveChanges();
          return RedirectToAction("Index");
        }
  }
}