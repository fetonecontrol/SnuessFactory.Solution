using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
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
    public ActionResult Create(Machine machine, int EngineerId)
    {
      _db.Machines.Add(machine);
      if (EngineerId != 0)
      {
        _db.MachineEngineers.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
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

    public ActionResult Edit(int id)
    {
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
    
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.MachineEngineers.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.Entry(machine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddEngineer(int id)
    {
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Type");
      return View(thisMachine);
    }
    [HttpPost]
    public ActionResult AddEngineer(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.MachineEngineers.Add(new MachineEngineer() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult DeleteMachine(int joinId)
    {
      var joinEntry = _db.MachineEngineers.FirstOrDefault(entry => entry.MachineEngineerId == joinId);
      _db.MachineEngineers.Remove(joinEntry);
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