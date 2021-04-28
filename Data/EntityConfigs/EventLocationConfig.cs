using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigs
{
    public class EventLocationConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // builder.HasKey(x => x.EventId);
            // builder.OwnsOne(x => x.Location, l =>
            // {
            //     l.Property(h => h.City).HasColumnName("Location_City");
            //     l.Property(h => h.State).HasColumnName("Location_State");
            //     l.Property(h => h.PostalCode).HasColumnName("Location_Zip");
            //     l.Property(h => h.StreetName).HasColumnName("Location_StreetName");
            //     l.Property(h => h.StreetNumbers).HasColumnName("Location_StreetNumbers");
            //     l.Property(h => h.Name).HasColumnName("Location_Name");
            // });
            // builder.OwnsMany(x => x.PaymentHistory, a =>
            // {
            //     a.Property(h => h.Amount).HasColumnName("Payment_Amount").HasDefaultValue(0);
            //     a.Property(h => h.Date).HasColumnName("Payment_Date");
            // });
        }
    }
}
