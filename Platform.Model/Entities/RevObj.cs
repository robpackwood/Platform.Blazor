using System;
using Platform.Common;

namespace Platform.Model.Entities
{
  public class RevObj : IBegEffDateEntity
  {
    public int Id { get; set; }
    public DateTime BegEffDate { get; set; }
    public string EffStatus { get; set; }
    public string Pin { get; set; }
    public string Ain { get; set; }
    public string XCoord { get; set; }
    public string YCoord { get; set; }
    public int RevObjType { get; set; }
    public int RevObjSubType { get; set; }
  }
}