using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Model;

namespace Platform.DataAccess
{
  public interface ILegalPartyRepository
  {
    Task<List<LegalPartyAssociation>> GetLegalPartyAssociations(
      int objectType, int objectId, DateTime effectiveDate );
  }

  public class LegalPartyRepository : ILegalPartyRepository
  {
    private readonly DataContext _dc;

    public LegalPartyRepository( DataContext dc )
    {
      _dc = dc;
    }

    public async Task<List<LegalPartyAssociation>> GetLegalPartyAssociations(
      int objectType, int objectId, DateTime effectiveDate )
    {
      return await (
        from legalPartyRole in _dc.LegalPartyRoles.WithEffDate( effectiveDate )
        join legalParty in _dc.LegalParties
          on legalPartyRole.LegalPartyId equals legalParty.Id
        where legalPartyRole.ObjectType == objectType &&
              legalPartyRole.ObjectId == objectId
        select new LegalPartyAssociation
        {
          DisplayName = legalParty.DisplayName,
          IsPrimary = legalPartyRole.PrimeLegalParty == 1,
          LegalPartyId = legalParty.Id,
          ObjectId = legalPartyRole.ObjectId,
          ObjectType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == legalPartyRole.ObjectType ),
          OwnershipType =_dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == legalPartyRole.OwnershipType ),
          PartySubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == legalParty.LpSubType ),
          PartyType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == legalParty.LegalPartyType ),
          PercentInterest = legalPartyRole.PercentInt,
          RoleType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == legalPartyRole.LpRoleType )
        } ).ToListAsync();
    }
  }
}