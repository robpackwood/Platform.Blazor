using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Platform.BR;
using Platform.Components.Map;
using Platform.Model;

namespace Platform.Blazor.Shared
{
  public class RevenueObjectDetailBase : ComponentBase
  {
    [Inject] 
    private IRevenueObjectDataService RevenueObjectDataService { get; set; }

    [Parameter] 
    public string RevObjId { get; set; }

    public RevenueObject RevenueObject { get; set; } = new RevenueObject();
    public List<Marker> MapMarkers { get; set; } = new List<Marker>();

    protected override async Task OnInitializedAsync()
    {
      int.TryParse( RevObjId, out int revObjId );

      if ( revObjId != 0 )
      {
        RevenueObject = await RevenueObjectDataService.GetRevenueObjectById( revObjId, DateTime.Now );

        if ( double.TryParse( RevenueObject.XCoord, out double xCoord ) &&
             double.TryParse( RevenueObject.YCoord, out double yCoord ) )

          MapMarkers = new List<Marker>
          {
            new Marker
            {
              Description = $"{RevenueObject.Pin}",
              ShowPopup = false,
              X = xCoord,
              Y = yCoord
            }
          };
      }
    }
  }
}