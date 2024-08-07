using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;


    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);

            builder.Property(UserId => UserId.Id)
            .HasConversion(userID => userID!.Value, id => new UserId(id));

            builder.Property(x => x.Nombre)
                .HasMaxLength(200)
                .HasConversion(n => n!.Value, value => new Nombre(value));

            builder.Property(x => x.Apellido)
                .HasMaxLength(200)
                .HasConversion(n => n!.Value, value => new Apellido(value));
            
            builder.Property(x => x.Email)
                .HasMaxLength(400)
                .HasConversion(n => n!.Value, value => new Domain.Users.Email(value));

            builder.Property(user => user.PasswordHash)
            .HasMaxLength(2000)
            .HasConversion(password => password!.Value, value => new PasswordHash(value));

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<UserRole>();
                
        }
    }
