using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Model;
using Platform.Model.Entities;

namespace Platform.DataAccess
{
  public interface ITaxBillRepository
  {
    Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries(
      int revObjId, bool populateFinancialAmounts = true );
  }

  public class TaxBillRepository : ITaxBillRepository
  {
    private readonly DataContext _dc;

    public TaxBillRepository( DataContext dc )
    {
      _dc = dc;
    }

    public async Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries(
      int revObjId, bool populateFinancialAmounts = true )
    {
      IQueryable<TaxBillSummary> taxBillSummaryQuery =
        from taxBillTran in _dc.TaxBillTrans
        join taxBill in _dc.TaxBills
          on taxBillTran.TaxBillId equals taxBill.Id
        where taxBillTran.RevObjId == revObjId &&
              taxBillTran.TranDate == (
                from subTaxBillTran in _dc.TaxBillTrans
                where subTaxBillTran.TaxBillId == taxBillTran.TaxBillId &&
                      subTaxBillTran.Status == 350562 && // Posted
                      subTaxBillTran.ProcGrpNumber == 0
                select subTaxBillTran.TranDate ).Max()
        orderby taxBill.TaxYear descending, taxBillTran.RollCaste
        select new TaxBillSummary
        {
          AcctId = taxBillTran.AcctId,
          AsmtType = new SysType {Id = taxBillTran.AsmtType},
          BillNumber = taxBill.BillNumber,
          DateGroupId = taxBillTran.DateGroupId,
          TaxBillId = taxBill.Id,
          RevObjId = revObjId,
          RollCaste = new SysType {Id = taxBillTran.RollCaste},
          RollType = new SysType {Id = taxBillTran.RollType},
          TagId = taxBillTran.TagId,
          TaxBillStatus = new SysType {Id = taxBill.Status},
          TaxBillType = new SysType {Id = taxBill.TbType},
          TaxType = new SysType {Id = taxBillTran.TaxType},
          TaxYear = taxBill.TaxYear
        };

      List<TaxBillSummary> taxBillSummaries = await taxBillSummaryQuery.ToListAsync();
      await PopulateSysTypes( taxBillSummaries );

      if ( populateFinancialAmounts )
      {
        var taxBillIds = new HashSet<int>( taxBillSummaries.Select( x => x.TaxBillId ) );
        await PopulateFinancialAmounts( taxBillIds, taxBillSummaries );
      }

      return taxBillSummaries;
    }

    private async Task PopulateFinancialAmounts( HashSet<int> taxBillIds, List<TaxBillSummary> taxBillSummaries )
    {
      var taxBillAmountQuery = from fnclDetailTot in _dc.FnclDetailTots
        where taxBillIds.Contains( fnclDetailTot.TaxBillId )
        group fnclDetailTot
          by fnclDetailTot.TaxBillId
        into taxBillGroup
        select new
        {
          TaxBillId = taxBillGroup.Key,
          TotalCharges = taxBillGroup.Sum( x => x.Cat == 290021 ? x.Amount : 0 ),
          TotalPayments = taxBillGroup.Sum( x => x.Cat == 290022 ? x.Amount : 0 )
        };

      var taxBillAmounts = await taxBillAmountQuery.ToDictionaryAsync(
        key => key.TaxBillId, value => new {value.TotalCharges, value.TotalPayments} );

      foreach ( TaxBillSummary taxBillSummary in taxBillSummaries )
      {
        if ( taxBillAmounts.TryGetValue( taxBillSummary.TaxBillId, out var taxBillAmount ) )
        {
          taxBillSummary.TotalCharges = taxBillAmount.TotalCharges;
          taxBillSummary.TotalPayments = taxBillAmount.TotalPayments;
          taxBillSummary.BalanceDue = taxBillSummary.TotalCharges + taxBillSummary.TotalPayments;
        }
      }
    }

    /// <summary>
    ///   NOTE: This would be replaced with a reasonable SysTypeCache mechanism loaded on Startup
    /// </summary>
    private async Task PopulateSysTypes( List<TaxBillSummary> taxBillSummaries )
    {
      var sysTypeIds = new HashSet<int>();

      foreach ( TaxBillSummary taxBillSummary in taxBillSummaries )
      {
        sysTypeIds.Add( taxBillSummary.AsmtType.Id );
        sysTypeIds.Add( taxBillSummary.RollCaste.Id );
        sysTypeIds.Add( taxBillSummary.RollType.Id );
        sysTypeIds.Add( taxBillSummary.TaxBillStatus.Id );
        sysTypeIds.Add( taxBillSummary.TaxBillType.Id );
        sysTypeIds.Add( taxBillSummary.TaxType.Id );
      }

      var sysTypeLookup = await
        ( from sysType in _dc.SysTypes.WithMaxEffDate()
          where sysTypeIds.Contains( sysType.Id )
          select new
          {
            sysType.Id,
            sysType.ShortDescr,
            sysType.Descr
          } ).ToDictionaryAsync( key => key.Id, value => new {value.ShortDescr, value.Descr} );

      foreach ( TaxBillSummary taxBillSummary in taxBillSummaries )
      {
        if ( sysTypeLookup.TryGetValue( taxBillSummary.AsmtType.Id, out var asmtType ) )
        {
          taxBillSummary.AsmtType.ShortDescr = asmtType.ShortDescr;
          taxBillSummary.AsmtType.Descr = asmtType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( taxBillSummary.RollCaste.Id, out var rollCaste ) )
        {
          taxBillSummary.RollCaste.ShortDescr = rollCaste.ShortDescr;
          taxBillSummary.RollCaste.Descr = rollCaste.Descr;
        }

        if ( sysTypeLookup.TryGetValue( taxBillSummary.RollType.Id, out var rollType ) )
        {
          taxBillSummary.RollType.ShortDescr = rollType.ShortDescr;
          taxBillSummary.RollType.Descr = rollType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( taxBillSummary.TaxBillStatus.Id, out var taxBillStatus ) )
        {
          taxBillSummary.TaxBillStatus.ShortDescr = taxBillStatus.ShortDescr;
          taxBillSummary.TaxBillStatus.Descr = taxBillStatus.Descr;
        }

        if ( sysTypeLookup.TryGetValue( taxBillSummary.TaxBillType.Id, out var taxBillType ) )
        {
          taxBillSummary.TaxBillType.ShortDescr = taxBillType.ShortDescr;
          taxBillSummary.TaxBillType.Descr = taxBillType.Descr;
        }

        if ( sysTypeLookup.TryGetValue( taxBillSummary.TaxType.Id, out var taxType ) )
        {
          taxBillSummary.TaxType.ShortDescr = taxType.ShortDescr;
          taxBillSummary.TaxType.Descr = taxType.Descr;
        }
      }
    }
  }
}