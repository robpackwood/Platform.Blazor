namespace Platform.Model.Entities
{
  public class TaxBill
  {
    public int Id { get; set; }
    public short TaxYear { get; set; }
    public string BillNumber { get; set; }
    public int TbType { get; set; }
    public int Status { get; set; }
  }
}