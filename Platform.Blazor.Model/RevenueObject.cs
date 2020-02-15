using System;
using Platform.Model.Entities;

namespace Platform.Model
{
  public class RevenueObject
  {
    public int Id { get; set; }
    public string EffStatus { get; set; }
    public string Pin { get; set; }
    public string Ain { get; set; }
    public string XCoord { get; set; }
    public string YCoord { get; set; }
    public SysType RevObjType { get; set; }
    public SysType RevObjSubType { get; set; }
  }
}