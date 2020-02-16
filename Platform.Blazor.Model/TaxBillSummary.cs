using Platform.Model.Entities;

namespace Platform.Model
{
  public class TaxBillSummary
  {
    public int TaxBillId { get; set; }
    public short TaxYear { get; set; }
    public string BillNumber { get; set; }
    public SysType TaxBillType { get; set; }
    public SysType TaxBillStatus { get; set; }
    public int RevObjId { get; set; }
    public int AcctId { get; set; }
    public int TagId { get; set; }
    public int DateGroupId { get; set; }
    public SysType RollCaste { get; set; }
    public SysType RollType { get; set; }
    public SysType TaxType { get; set; }
    public SysType AsmtType { get; set; }
    public decimal TotalCharges { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal BalanceDue { get; set; }
  }
}