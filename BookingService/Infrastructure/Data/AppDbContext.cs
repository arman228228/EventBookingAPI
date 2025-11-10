using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserTicket>()
            .HasKey(ut => new { ut.UserId, ut.TicketId });

        modelBuilder.Entity<UserTicket>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTickets)
            .HasForeignKey(ut => ut.UserId);

        modelBuilder.Entity<UserTicket>()
            .HasOne(ut => ut.Ticket)
            .WithMany(t => t.UserTickets)
            .HasForeignKey(ut => ut.TicketId);

        modelBuilder.Entity<EventDetails>()
            .HasKey(ed => ed.EventId);

        modelBuilder.Entity<EventDetails>()
            .HasOne(ed => ed.Event)
            .WithOne(e => e.Details)
            .HasForeignKey<EventDetails>(ed => ed.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public DbSet<Event> Events { get; set; }
    public DbSet<EventDetails> EventDetails { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserTicket> UserTickets { get; set; }
    public DbSet<Venue> Venues { get; set; }
}