using System;

namespace Platform.Model.Entities
{
  public class FnclDetailTot
  {
    public int FnclHeaderId { get; set; }
    public int DetailLine { get; set; }
    public DateTime BusDate { get; set; }
    public DateTime? EffPayDate { get; set; }
    public int FnclTranId { get; set; }
    public int RevenueSource { get; set; }
    public short TaxYear { get; set; }
    public int RollType { get; set; }
    public int ReceiptDetailId { get; set; }
    public int AcctId { get; set; }
    public int RevObjId { get; set; }
    public int TaxBillId { get; set; }
    public int TagId { get; set; }
    public int TaxAuthorityId { get; set; }
    public int TafId { get; set; }
    public int TifId { get; set; }
    public int Cat { get; set; }
    public int CatType { get; set; }
    public int Cd { get; set; }
    public int SubCd { get; set; }
    public decimal Amount { get; set; }
    public short Inst { get; set; }
    public int DistCd { get; set; }
    public int ObjectType { get; set; }
    public int ObjectId { get; set; }
    public int RevObjSiteId { get; set; }
    public int CollectionType { get; set; }
  }
}