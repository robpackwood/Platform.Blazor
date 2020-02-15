using System;

namespace Platform.Model.Entities
{
  public class TaxBillTran
  {
    public int Id { get; set; }
    public int TaxBillId { get; set; }
    public int RevObjId { get; set; }
    public int AcctId { get; set; }
    public int TagId { get; set; }
    public int DateGroupId { get; set; }
    public int TbtType { get; set; }
    public DateTime TranDate { get; set; }
    public int RollCaste { get; set; }
    public int RollType { get; set; }
    public int Status { get; set; }
    public int TaxType { get; set; }
    public int ProcGrpNumber { get; set; }
    public int AsmtType { get; set; }
  }
}