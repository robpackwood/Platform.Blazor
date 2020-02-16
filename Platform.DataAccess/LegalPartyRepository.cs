using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Model;
using Platform.Model.Entities;

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
      List<LegalPartyAssociation> legalPartyAssociations = await (
        from legalPartyRole in _dc.LegalPartyRoles.WithEffDate( effectiveDate )
        join legalParty in _dc.LegalParties
          on legalPartyRole.LegalPartyId equals legalParty.Id
        where legalPartyRole.ObjectType == objectType &&
              legalPartyRole.ObjectId == objectId &&
              legalPartyRole.EffStatus == "A"
        select new LegalPartyAssociation
        {
          DisplayName = legalParty.DisplayName,
          IsPrimary = legalPartyRole.PrimeLegalParty == 1,
          LegalPartyId = legalParty.Id,
          ObjectId = legalPartyRole.ObjectId,
          ObjectType = new SysType {Id = legalPartyRole.ObjectType},
          OwnershipType = new SysType {Id = legalPartyRole.OwnershipType},
          PartySubType = new SysType {Id = legalParty.LpSubType},
          PartyType = new SysType {Id = legalParty.LegalPartyType},
          PercentInterest = legalPartyRole.PercentInt,
          RoleType = new SysType {Id = legalPartyRole.LpRoleType}
        } ).ToListAsync();

      await PopulateSysTypes( legalPartyAssociations );

      return legalPartyAssociations;
    }

    /// <summary>
    ///   NOTE: This would be replaced with a reasonable SysTypeCache mechanism loaded on Startup
    /// </summary>
    private async Task PopulateSysTypes( List<LegalPartyAssociation> legalPartyAssociations )
    {
      var sysTypeIds = new HashSet<int>();

      foreach ( LegalPartyAssociation legalPartyAssociation in legalPartyAssociations )
      {
        sysTypeIds.Add( legalPartyAssociation.ObjectType.Id );
        sysTypeIds.Add( legalPartyAssociation.OwnershipType.Id );
        sysTypeIds.Add( legalPartyAssociation.PartySubType.Id );
        sysTypeIds.Add( legalPartyAssociation.PartyType.Id );
        sysTypeIds.Add( legalPartyAssociation.RoleType.Id );
      }

      var sysTypeLookup = await
        ( from sysType in _dc.SysTypes.WithMaxEffDate()
          where sysTypeIds.Contains( sysType.Id )
          select new
          {
            sysType.Id,
            sysType.ShortDescr,
            sysType.Descr
          } ).ToDictionaryAsync( key => key.Id, value => new {value.ShortDescr, value.Descr} );

      foreach ( LegalPartyAssociation legalPartyAssociation in legalPartyAssociations )
      {
        if ( sysTypeLookup.TryGetValue( legalPartyAssociation.ObjectType.Id, out var objectType ) )
        {
          legalPartyAssociation.ObjectType.ShortDescr = objectType.ShortDescr;
          legalPartyAssociation.ObjectType.Descr = objectType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( legalPartyAssociation.OwnershipType.Id, out var ownershipType ) )
        {
          legalPartyAssociation.OwnershipType.ShortDescr = ownershipType.ShortDescr;
          legalPartyAssociation.OwnershipType.Descr = ownershipType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( legalPartyAssociation.PartySubType.Id, out var partySubType ) )
        {
          legalPartyAssociation.PartySubType.ShortDescr = partySubType.ShortDescr;
          legalPartyAssociation.PartySubType.Descr = partySubType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( legalPartyAssociation.PartyType.Id, out var partyType ) )
        {
          legalPartyAssociation.PartyType.ShortDescr = partyType.ShortDescr;
          legalPartyAssociation.PartyType.Descr = partyType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( legalPartyAssociation.RoleType.Id, out var rolType ) )
        {
          legalPartyAssociation.RoleType.ShortDescr = rolType.ShortDescr;
          legalPartyAssociation.RoleType.Descr = rolType.Descr;
        }
      }
    }
  }
}