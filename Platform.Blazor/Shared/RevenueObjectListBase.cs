using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Platform.BR;
using Platform.Model;

namespace Platform.Blazor.Shared
{
  public class RevenueObjectListBase : ComponentBase
  {
    [Inject] 
    private IRevenueObjectDataService RevenueObjectDataService { get; set; }

    public List<RevenueObject> RevenueObjects { get; private set; }

    protected override async Task OnInitializedAsync()
    {
      RevenueObjects = await RevenueObjectDataService.GetRevenueObjects( DateTime.Now );
    }
  }
}