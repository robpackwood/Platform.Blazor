using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.DataAccess;
using Platform.Model;

namespace Platform.BR
{
  public interface ITaxBillDataService
  {
    Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( 
      int revObjId, bool populateFinancialAmounts = true );
  }

  public class TaxBillDataService : ITaxBillDataService
  {
    private readonly ITaxBillRepository _taxBillRepository;

    public TaxBillDataService( ITaxBillRepository taxBillRepository )
    {
      _taxBillRepository = taxBillRepository;
    }

    public Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( 
      int revObjId, bool populateFinancialAmounts = true )
    {
      return _taxBillRepository.GetRevenueObjectTaxBillSummaries( revObjId, populateFinancialAmounts );
    }
  }
}