using DevIO.Business.Models.Fornecedores;

using System.Data.Entity.ModelConfiguration;

namespace DevIO.Infra.Data.Mappings
{
    public class EnderecoConfig : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfig()
        {
            HasKey(p => p.Id);

            Property(p => p.Logradouro)
                .IsRequired()
                .HasMaxLength(200);

            Property(p => p.Numero)
                .IsRequired()
                .HasMaxLength(50);

            Property(p => p.Cep)
                .IsRequired()
                .HasMaxLength(8)
                .IsFixedLength();

            Property(p => p.Complemento)
                .HasMaxLength(250);

            Property(p => p.Bairro)
                .IsRequired();

            Property(p => p.Cidade)
                .IsRequired();

            Property(p => p.Estado)
                .IsRequired()
                .HasMaxLength(500);

            ToTable("Enderecos");
        }
    }
}
