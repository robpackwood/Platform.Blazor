using Microsoft.EntityFrameworkCore;
using Platform.Model.Entities;

namespace Platform.DataAccess
{
  public class DataContext : DbContext
  {
    public DataContext( DbContextOptions<DataContext> options ) : base( options )
    {
    }

    public DbSet<RevObj> RevObjs { get; set; }
    public DbSet<LegalParty> LegalParties { get; set; }
    public DbSet<LegalPartyRole> LegalPartyRoles { get; set; }
    public DbSet<SysType> SysTypes { get; set; }
    public DbSet<TaxBill> TaxBills { get; set; }
    public DbSet<TaxBillTran> TaxBillTrans { get; set; }
    public DbSet<FnclDetailTot> FnclDetailTots { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<RevObj>().ToTable( "RevObj" );
      modelBuilder.Entity<LegalParty>().ToTable( "LegalParty" );
      modelBuilder.Entity<LegalPartyRole>().ToTable( "LegalPartyRole" );
      modelBuilder.Entity<SysType>().ToTable( "SysType" );
      modelBuilder.Entity<TaxBill>().ToTable( "TaxBill" );
      modelBuilder.Entity<TaxBillTran>().ToTable( "TaxBillTran" );
      modelBuilder.Entity<FnclDetailTot>().ToView( "FnclDetailTot_V" );
      modelBuilder.Entity<FnclDetailTot>().HasKey( fd => new {fd.FnclHeaderId, fd.DetailLine} );
    }
  }
}