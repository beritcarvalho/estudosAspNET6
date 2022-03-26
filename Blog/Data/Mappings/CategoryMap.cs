using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // definindo a tabela
            builder.ToTable("Category");

            //definindo quem é a chave primária
            builder.HasKey(x => x.Id);

            ///DEFININDO AS PROPIEDADES DOS CAMPOS
            //Definir o identity e como o será o incremento da chave primária
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Constrainsts da coluna Name
            builder.Property(x => x.Name)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Name") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(80); // tamanho máximo da coluna.

            //DEFININDO OS INDICES
            builder.HasIndex(x => x.Slug, "IX_Category_Slug") // definindo quem é o indice
                .IsUnique(); //definindo indice unico
        }
    }
}