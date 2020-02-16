using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Model;
using Platform.Model.Entities;

namespace Platform.DataAccess
{
  public interface IRevenueObjectRepository
  {
    Task<RevenueObject> GetRevenueObjectByPin(
      string pin, DateTime effectiveDate, bool includeInactive = true );

    Task<List<RevenueObject>> GetRevenueObjects(
      DateTime effectiveDate, int maxCount = 100, bool includeInactive = true );

    Task<RevenueObject> GetRevenueObjectById( int id, DateTime effectiveDate );
  }

  public class RevenueObjectRepository : IRevenueObjectRepository
  {
    private readonly DataContext _dc;

    public RevenueObjectRepository( DataContext dc )
    {
      _dc = dc;
    }

    public async Task<List<RevenueObject>> GetRevenueObjects(
      DateTime effectiveDate, int maxCount = 100, bool includeInactive = true )
    {
      IQueryable<RevenueObject> revenueObjectQuery =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where includeInactive || revObj.EffStatus == "A"
        orderby revObj.Id
        select MapRevenueObject(
          revObj,
          new SysType {Id = revObj.RevObjType},
          new SysType {Id = revObj.RevObjSubType} );

      List<RevenueObject> revenueObjects = await revenueObjectQuery.Take( maxCount ).ToListAsync();
      await PopulateSysTypes( revenueObjects );
      return revenueObjects;
    }

    public async Task<RevenueObject> GetRevenueObjectByPin(
      string pin, DateTime effectiveDate, bool includeInactive = true )
    {
      IQueryable<RevenueObject> revenueObjectQuery =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where revObj.Pin == pin &&
              ( includeInactive || revObj.EffStatus == "A" )
        let revObjType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjType )
        let revObjSubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjSubType )
        select MapRevenueObject( revObj, revObjType, revObjSubType );

      return await revenueObjectQuery.FirstOrDefaultAsync();
    }

    public async Task<RevenueObject> GetRevenueObjectById( int id, DateTime effectiveDate )
    {
      IQueryable<RevenueObject> revenueObjectQuery =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where revObj.Id == id
        let revObjType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjType )
        let revObjSubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjSubType )
        select MapRevenueObject( revObj, revObjType, revObjSubType );

      return await revenueObjectQuery.FirstOrDefaultAsync();
    }

    /// <summary>
    ///   NOTE: Some kind of AutoMapper solution would come into play here...
    /// </summary>
    private static RevenueObject MapRevenueObject( RevObj revObj, SysType revObjType, SysType revObjSubType )
    {
      return new RevenueObject
      {
        Id = revObj.Id,
        EffStatus = revObj.EffStatus,
        Pin = revObj.Pin,
        Ain = revObj.Ain,
        XCoord = revObj.XCoord,
        YCoord = revObj.YCoord,
        RevObjType = revObjType,
        RevObjSubType = revObjSubType
      };
    }

    /// <summary>
    ///   NOTE: This would be replaced with a reasonable SysTypeCache mechanism loaded on Startup
    /// </summary>
    private async Task PopulateSysTypes( List<RevenueObject> revenueObjects )
    {
      var sysTypeIds = new HashSet<int>();

      foreach ( RevenueObject revenueObject in revenueObjects )
      {
        sysTypeIds.Add( revenueObject.RevObjType.Id );
        sysTypeIds.Add( revenueObject.RevObjSubType.Id );
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

      foreach ( RevenueObject revenueObject in revenueObjects )
      {
        if ( sysTypeLookup.TryGetValue( revenueObject.RevObjType.Id, out var revObjType ) )
        {
          revenueObject.RevObjType.ShortDescr = revObjType.ShortDescr;
          revenueObject.RevObjType.Descr = revObjType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( revenueObject.RevObjSubType.Id, out var revObjSubType ) )
        {
          revenueObject.RevObjSubType.ShortDescr = revObjSubType.ShortDescr;
          revenueObject.RevObjSubType.Descr = revObjSubType.Descr;
        }
      }
    }
  }
}