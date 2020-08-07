using System.Collections.Generic;

namespace Factory.Models
{
  public class Machine
  {
    public Machine()
    {
      this.Engineers = new HashSet<MachineEngineer>(); // contextual list
    }

    public int MachineId { get; set; }
    public string Type { get; set; }


    public ICollection<MachineEngineer> Engineers { get; }
  }
}