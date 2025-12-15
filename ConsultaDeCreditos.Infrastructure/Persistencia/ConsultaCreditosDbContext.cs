using ConsultaDeCreditos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ConsultaDeCreditos.Infrastructure.Persistencia;

/// <summary>
/// Contexto do banco de dados usando Entity Framework Core
/// </summary>
public class ConsultaCreditosDbContext : DbContext
{
    public ConsultaCreditosDbContext(DbContextOptions<ConsultaCreditosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Credito> Creditos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Credito>(entity =>
        {
            entity.ToTable("credito");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.NumeroCredito)
                .HasColumnName("numero_credito")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.NumeroNfse)
                .HasColumnName("numero_nfse")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.DataConstituicao)
                .HasColumnName("data_constituicao")
                .IsRequired();

            entity.Property(e => e.ValorIssqn)
                .HasColumnName("valor_issqn")
                .HasPrecision(15, 2)
                .IsRequired();

            entity.Property(e => e.TipoCredito)
                .HasColumnName("tipo_credito")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.SimplesNacional)
                .HasColumnName("simples_nacional")
                .IsRequired();

            entity.Property(e => e.Aliquota)
                .HasColumnName("aliquota")
                .HasPrecision(5, 2)
                .IsRequired();

            entity.Property(e => e.ValorFaturado)
                .HasColumnName("valor_faturado")
                .HasPrecision(15, 2)
                .IsRequired();

            entity.Property(e => e.ValorDeducao)
                .HasColumnName("valor_deducao")
                .HasPrecision(15, 2)
                .IsRequired();

            entity.Property(e => e.BaseCalculo)
                .HasColumnName("base_calculo")
                .HasPrecision(15, 2)
                .IsRequired();

            entity.Property(e => e.DataCriacao)
                .HasColumnName("data_criacao")
                .IsRequired();

            entity.HasIndex(e => e.NumeroCredito).IsUnique();
            entity.HasIndex(e => e.NumeroNfse);
        });
    }
}
