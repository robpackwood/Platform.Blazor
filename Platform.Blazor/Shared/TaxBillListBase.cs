using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Platform.BR;
using Platform.Model;

namespace Platform.Blazor.Shared
{
  public class TaxBillListBase : ComponentBase
  {
    [Inject] 
    private ITaxBillDataService TaxBillDataService { get; set; }

    [Parameter] 
    public string RevObjId { get; set; }

    public List<TaxBillSummary> TaxBillSummaries { get; private set; }

    protected override async Task OnInitializedAsync()
    {
      if ( int.TryParse( RevObjId, out int revObjId ) && revObjId != 0 )
      {
        TaxBillSummaries = await TaxBillDataService.GetRevenueObjectTaxBillSummaries( revObjId );
      }
    }
  }
}