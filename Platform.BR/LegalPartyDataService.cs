using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.DataAccess;
using Platform.Model;

namespace Platform.BR
{
  public interface ILegalPartyDataService
  {
    Task<List<LegalPartyAssociation>> GetLegalPartyAssociations(
      int objectType, int objectId, DateTime effectiveDate );
  }

  public class LegalPartyDataService : ILegalPartyDataService
  {
    private readonly ILegalPartyRepository _legalPartyRepository;

    public LegalPartyDataService( ILegalPartyRepository legalPartyRepository )
    {
      _legalPartyRepository = legalPartyRepository;
    }

    public async Task<List<LegalPartyAssociation>> GetLegalPartyAssociations(
      int objectType, int objectId, DateTime effectiveDate )
    {
      return await _legalPartyRepository.GetLegalPartyAssociations(
        objectType, objectId, effectiveDate );
    }
  }
}