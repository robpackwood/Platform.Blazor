using Microsoft.AspNetCore.Components;

namespace Platform.Blazor.Pages
{
  public class RevenueObjectSummaryBase : ComponentBase
  {
    [Parameter] 
    public string RevObjId { get; set; }
  }
}