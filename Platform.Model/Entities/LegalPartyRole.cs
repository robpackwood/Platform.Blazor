using System;
using Platform.Common;

namespace Platform.Model.Entities
{
  public class LegalPartyRole : IBegEffDateEntity
  {
    public int LegalPartyId { get; set; }
    public string EffStatus { get; set; }
    public int ObjectType { get; set; }
    public int ObjectId { get; set; }
    public int LpRoleType { get; set; }
    public short PrimeLegalParty { get; set; }
    public int OwnershipType { get; set; }
    public decimal PercentInt { get; set; }
    public int Id { get; set; }
    public DateTime BegEffDate { get; set; }
  }
}