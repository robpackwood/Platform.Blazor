using System;
using Platform.Common;

namespace Platform.Model.Entities
{
  public class SysType : IBegEffDateEntity
  {
    public int Id { get; set; }
    public DateTime BegEffDate { get; set; }
    public string ShortDescr { get; set; }
    public string Descr { get; set; }
  }
}