namespace Models.EF
{
    using System.Data.Entity;

    public partial class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext()
            : base("name=EntitiesDbContext")
        {
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Footer> Footers { get; set; }
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(e => e.CartId)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Footer>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Map>()
                .Property(e => e.APIKey)
                .IsUnicode(false);

            modelBuilder.Entity<Map>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Map>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Target)
                .IsUnicode(false);

            modelBuilder.Entity<MenuType>()
                .HasMany(e => e.Menus)
                .WithOptional(e => e.MenuType)
                .HasForeignKey(e => e.TypeID);

            modelBuilder.Entity<Order>()
                .Property(e => e.ShipMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ShipEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Total)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.PromotionPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.ProductSpecification)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .HasForeignKey(e => e.CategoryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.Target)
                .IsUnicode(false);
        }
    }
}
