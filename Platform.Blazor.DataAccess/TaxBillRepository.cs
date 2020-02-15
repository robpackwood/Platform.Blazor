using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Model;

namespace Platform.DataAccess
{
  public interface ITaxBillRepository
  {
    Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( int revObjId );
  }

  public class TaxBillRepository : ITaxBillRepository
  {
    private readonly DataContext _dc;

    public TaxBillRepository( DataContext dc )
    {
      _dc = dc;
    }

    public async Task<List<TaxBillSummary>> GetRevenueObjectTaxBillSummaries( int revObjId )
    {
      return await (
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
          AsmtType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBillTran.AsmtType ),
          BillNumber = taxBill.BillNumber,
          DateGroupId = taxBillTran.DateGroupId,
          TaxBillId = taxBill.Id,
          RevObjId = revObjId,
          RollCaste = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBillTran.RollCaste ),
          RollType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBillTran.RollType ),
          TagId = taxBillTran.TagId,
          TaxBillStatus = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBill.Status ),
          TaxBillType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBill.TbType ),
          TaxType = _dc.SysTypes.WithMaxEffDate().SingleOrDefault( st => st.Id == taxBillTran.TaxType ),
          TaxYear = taxBill.TaxYear,
          BalanceDue = _dc.FnclDetailTots.Where( fd => fd.TaxBillId == taxBill.Id ).Sum( fd => fd.Amount )
        } ).ToListAsync();
    }
  }
}