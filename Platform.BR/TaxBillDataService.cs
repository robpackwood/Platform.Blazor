using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.DataAccess;
using Platform.Model;

namespace Platform.BR
{
  public interface ITaxBillDataService
  {
    Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( int revObjId );
  }

  public class TaxBillDataService : ITaxBillDataService
  {
    private readonly ITaxBillRepository _taxBillRepository;

    public TaxBillDataService( ITaxBillRepository taxBillRepository )
    {
      _taxBillRepository = taxBillRepository;
    }

    public Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( int revObjId )
    {
      return _taxBillRepository.GetRevenueObjectTaxBillSummaries( revObjId );
    }
  }
}