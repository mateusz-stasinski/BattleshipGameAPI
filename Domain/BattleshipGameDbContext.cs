using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BattleshipGameDbContext : DbContext
    {
        public BattleshipGameDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<Ship> Ships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().Property(p => p.Name).HasMaxLength(25).IsRequired();
            modelBuilder.Entity<Ship>().Property(s => s.Name).HasMaxLength(25).IsRequired();
        }
    }
}
