using Microsoft.EntityFrameworkCore;

namespace FluxoCaixaContext;

public class FluxoCaixaDbContext : DbContext
{
    public FluxoCaixaDbContext()
    {
    }

    public FluxoCaixaDbContext(DbContextOptions<FluxoCaixaDbContext> options) : base(options) 
    {
    }

    public virtual DbSet<LancamentoDiario> LancamentosDiarios { get; set; }
    public virtual DbSet<ConsolidadoDiario>  ConsolidadosDiarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LancamentoDiario>()
            .ToTable("Lancamentos", schema: "LancamentosDiarios");

        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.Id).HasColumnName("id");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.ComercianteId).HasColumnName("comerciante_id");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.Data).HasColumnName("data");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.Tipo).HasColumnName("tipo");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.Descricao).HasColumnName("descricao");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.Valor).HasColumnName("valor");
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(p => p.DataVencimento).HasColumnName("data_vencimento");
        
        // modelBuilder.Entity<LancamentoDiario>()
        //     .ToTable(t => t.HasCheckConstraint("CK_Data", "data >= CURRENT_DATE"));
        
        modelBuilder.Entity<LancamentoDiario>()
            .ToTable(t => t.HasCheckConstraint("CK_DataVencimento", "data_vencimento >= Data"));
        
        modelBuilder.Entity<LancamentoDiario>()
            .ToTable(t => t.HasCheckConstraint("CK_Valor", "valor > 0"));
        
        modelBuilder.Entity<LancamentoDiario>()
            .ToTable(t => t.HasCheckConstraint("CK_ComercianteId", "comerciante_id > 0"));
        
        modelBuilder.Entity<LancamentoDiario>()
            .Property(b => b.Descricao)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<LancamentoDiario>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<ConsolidadoDiario>()
            .ToTable("Consolidados", schema: "ConsolidadosDiarios");

        modelBuilder.Entity<ConsolidadoDiario>()
            .HasKey(c => c.Id);
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .HasIndex(c => new { c.Data, c.ComercianteId }, "IX_ConsolidadosDiarios_Unique").IsUnique(true);
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .Property(p => p.Id).HasColumnName("id");

        modelBuilder.Entity<ConsolidadoDiario>()
            .Property(p => p.Data).HasColumnName("data");
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .Property(p => p.SaldoInicial).HasColumnName("saldo_inicial");
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .Property(p => p.SaldoFinal).HasColumnName("saldo_final");
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .Property(p => p.ComercianteId).HasColumnName("comerciante_id");
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .ToTable(t => t.HasCheckConstraint("CK_Data", "data < CURRENT_DATE"));
        
        modelBuilder.Entity<ConsolidadoDiario>()
            .ToTable(t => t.HasCheckConstraint("CK_ComercianteId", "comerciante_id > 0"));
        
        modelBuilder.HasPostgresEnum<TipoLancamentoEnum>();
    }
}