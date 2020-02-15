using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Platform.BR;
using Platform.Model;

namespace Platform.Blazor.Shared
{
  public class LegalPartyListBase : ComponentBase
  {
    [Inject] 
    private ILegalPartyDataService LegalPartyDataService { get; set; }

    [Parameter] 
    public int ObjectType { get; set; }

    [Parameter] 
    public string ObjectId { get; set; }

    public List<LegalPartyAssociation> LegalPartyAssociations { get; private set; }

    protected override async Task OnInitializedAsync()
    {
      if ( ObjectType != 0 && int.TryParse( ObjectId, out int objectId ) && objectId != 0 )
      {
        LegalPartyAssociations = await LegalPartyDataService.GetLegalPartyAssociations(
          ObjectType, objectId, DateTime.Now );
      }
    }
  }
}