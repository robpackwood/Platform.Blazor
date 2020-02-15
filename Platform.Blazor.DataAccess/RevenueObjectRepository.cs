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
      DateTime effectiveDate, int maxCount = 10, bool includeInactive = true );

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
      DateTime effectiveDate, int maxCount = 10, bool includeInactive = true )
    {
      IQueryable<RevenueObject> revenueObjects =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where includeInactive || revObj.EffStatus == "A"
        let revObjType =  _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjType )
        let revObjSubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjSubType )
        orderby revObj.Id
        select MapRevenueObject( revObj, revObjType, revObjSubType );

      return await revenueObjects.Take( maxCount ).ToListAsync();
    }

    public async Task<RevenueObject> GetRevenueObjectByPin(
      string pin, DateTime effectiveDate, bool includeInactive = true )
    {
      IQueryable<RevenueObject> revenueObjects =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where revObj.Pin == pin &&
              ( includeInactive || revObj.EffStatus == "A" )
        let revObjType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjType )
        let revObjSubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjSubType )
        select MapRevenueObject( revObj, revObjType, revObjSubType );

      return await revenueObjects.FirstOrDefaultAsync();
    }

    public async Task<RevenueObject> GetRevenueObjectById( int id, DateTime effectiveDate )
    {
      IQueryable<RevenueObject> revenueObjects =
        from revObj in _dc.RevObjs.WithEffDate( effectiveDate )
        where revObj.Id == id
        let revObjType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjType )
        let revObjSubType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == revObj.RevObjSubType )
        select MapRevenueObject( revObj, revObjType, revObjSubType );

      return await revenueObjects.FirstOrDefaultAsync();
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
  }
}