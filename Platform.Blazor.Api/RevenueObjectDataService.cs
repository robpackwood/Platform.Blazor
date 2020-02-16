using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.DataAccess;
using Platform.Model;

namespace Platform.BR
{
  public interface IRevenueObjectDataService
  {
    Task<RevenueObject> GetRevenueObjectByPin(
      string pin, DateTime effectiveDate, bool includeInactive = true );

    Task<List<RevenueObject>> GetRevenueObjects(
      DateTime effectiveDate, int maxCount = 100, bool includeInactive = true );

    Task<RevenueObject> GetRevenueObjectById( int id, DateTime effectiveDate );
  }

  public class RevenueObjectDataService : IRevenueObjectDataService
  {
    private readonly IRevenueObjectRepository _revenueObjectRepository;

    public RevenueObjectDataService( IRevenueObjectRepository revenueObjectRepository )
    {
      _revenueObjectRepository = revenueObjectRepository;
    }

    public async Task<RevenueObject> GetRevenueObjectById( int id, DateTime effectiveDate )
    {
      return await _revenueObjectRepository.GetRevenueObjectById( id, effectiveDate );
    }

    public async Task<RevenueObject> GetRevenueObjectByPin(
      string pin, DateTime effectiveDate, bool includeInactive = true )
    {
      return await _revenueObjectRepository.GetRevenueObjectByPin( pin, effectiveDate, includeInactive );
    }

    public async Task<List<RevenueObject>> GetRevenueObjects(
      DateTime effectiveDate, int maxCount = 100, bool includeInactive = true )
    {
      return await _revenueObjectRepository.GetRevenueObjects( effectiveDate, maxCount, includeInactive );
    }
  }
}