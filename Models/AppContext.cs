using Microsoft.EntityFrameworkCore;

public class AppContext: DbContext{
    public DbSet<User> Users { get; set; }

    public AppContext(){
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}