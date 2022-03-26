using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            // definindo a tabela
            builder.ToTable("User");

            //definindo quem é a chave primária
            builder.HasKey(x => x.Id);

            ///DEFININDO AS PROPIEDADES DOS CAMPOS
            //Definir o identity e como o será o incremento da chave primária
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            /*
            [Name] NVARCHAR(80) NOT NULL,
            [Email] VARCHAR(200) NOT NULL,
            [PasswordHash] VARCHAR(255) NOT NULL,
            [Bio] TEXT NOT NULL,
            [Image] VARCHAR(2000) NOT NULL,
            [Slug] VARCHAR(80) NOT NULL,*/


            //Constrainsts da coluna Name
            builder.Property(x => x.Name)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Name") //Definando a coluna e o nome.
                .HasColumnType("NVARCHAR") //tipo da coluna
                .HasMaxLength(80); // tamanho máximo da coluna.


            //Constrainsts da coluna Email
            builder.Property(x => x.Email)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Email") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(200); // tamanho máximo da coluna.

            //Constrainsts da coluna PasswordHash
            builder.Property(x => x.PasswordHash)
                .IsRequired() // IS NOT NULL
                .HasColumnName("PasswordHash") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(255); // tamanho máximo da coluna.


            //Constrainsts da coluna Bio
            builder.Property(x => x.Bio)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Bio") //Definando a coluna e o nome.
                .HasColumnType("TEXT"); //tipo da coluna   

            //Constrainsts da coluna Slug
            builder.Property(x => x.Slug)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Slug") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(80); // tamanho máximo da coluna.   

            //Constrainsts da coluna Email
            builder.Property(x => x.Email)
                .IsRequired() // IS NOT NULL
                .HasColumnName("Email") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(200); // tamanho máximo da coluna.
                                    // 

            //Constrainsts da coluna Gitub
            builder.Property(x => x.GitHub)                
                .HasColumnName("GitHub") //Definando a coluna e o nome.
                .HasColumnType("VARCHAR") //tipo da coluna
                .HasMaxLength(200); // tamanho máximo da coluna.

            //DEFININDO OS INDICES
            /*
            CREATE NONCLUSTERED INDEX [IX_User_Email] ON [User]([Email])
            CREATE NONCLUSTERED INDEX [IX_User_Slug] ON [User]([Slug])*/

            builder.HasIndex(x => x.Email, "IX_User_Email") // definindo quem é o indice
                .IsUnique(); //definindo indice unico

            builder.HasIndex(x => x.Slug, "IX_User_Slug") // definindo quem é o indice
                .IsUnique(); //definindo indice unico

            //relacionamentos
            builder
                .HasMany(x => x.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                    user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                     .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.Cascade));
        }
    }
}