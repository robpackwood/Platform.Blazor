using Platform.Model.Entities;

namespace Platform.Model
{
  public class LegalPartyAssociation
  {
    public int LegalPartyId { get; set; }
    public string DisplayName { get; set; }
    public SysType ObjectType { get; set; }
    public int ObjectId { get; set; }
    public bool IsPrimary { get; set; }
    public SysType RoleType { get; set; }
    public SysType OwnershipType { get; set; }
    public decimal PercentInterest { get; set; }
    public SysType PartyType { get; set; }
    public SysType PartySubType { get; set; }
  }
}