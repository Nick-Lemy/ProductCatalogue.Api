using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Consumer.Models;

namespace ProductCatalogue.Consumer.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<NotificationLog> NotificationLogs { get; set; }
}
