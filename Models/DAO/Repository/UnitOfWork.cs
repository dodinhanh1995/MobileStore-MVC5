using Models.EF;
using System;
using System.Threading.Tasks;

namespace Models.DAO.Repository
{
    public class UnitOfWork : IDisposable
    {
        private EntitiesDbContext _db = new EntitiesDbContext();
        private GenericRepository<About> _aboutRepo;
        private ContactRepository _contactRepo;
        private FooterRepository _footerRepo;
        private GenericRepository<Map> _mapRepo;
        private MenuRepository _menuRepo;
        private MenuTypeRepository _menuTypeRepo;
        private OrderRepository _orderRepo;
        private OrderDetailRepository _orderDetailRepo;
        private ProductCategoryRepository _productCategoryRepo;
        private ProductSpecificationRepository _productSpecificationRepo;
        private ProductRepository _productRepo;
        private SlideRepository _slideRepo;
        private CommentRepository _commentRepo;
        private CartRepository _cartRepo;
        private StatisticsRepository _statisticsRepo;

        public GenericRepository<About> _AboutRepo
        {
            get
            {
                if (_aboutRepo == null)
                    _aboutRepo = new GenericRepository<About>(_db);
                return _aboutRepo;
            }
        }

        public ContactRepository _ContactRepo
        {
            get
            {
                if (_contactRepo == null)
                    _contactRepo = new ContactRepository(_db);
                return _contactRepo;
            }
        }

        public FooterRepository _FooterRepo
        {
            get
            {
                if (_footerRepo == null)
                    _footerRepo = new FooterRepository(_db);
                return _footerRepo;
            }
        }

        public GenericRepository<Map> _MapRepo
        {
            get
            {
                if (_mapRepo == null)
                    _mapRepo = new GenericRepository<Map>(_db);
                return _mapRepo;
            }
        }

        public MenuRepository _MenuRepo
        {
            get
            {
                if (_menuRepo == null)
                    _menuRepo = new MenuRepository(_db);
                return _menuRepo;
            }
        }

        public MenuTypeRepository _MenuTypeRepo
        {
            get
            {
                if (_menuTypeRepo == null)
                    _menuTypeRepo = new MenuTypeRepository(_db);
                return _menuTypeRepo;
            }
        }

        public OrderRepository _OrderRepo
        {
            get
            {
                if (_orderRepo == null)
                    _orderRepo = new OrderRepository(_db);
                return _orderRepo;
            }
        }

        public OrderDetailRepository _OrderDetailRepo
        {
            get
            {
                if (_orderDetailRepo == null)
                    _orderDetailRepo = new OrderDetailRepository(_db);
                return _orderDetailRepo;
            }
        }

        public ProductCategoryRepository _ProductCategoryRepo
        {
            get
            {
                if (_productCategoryRepo == null)
                    _productCategoryRepo = new ProductCategoryRepository(_db);
                return _productCategoryRepo;
            }
        }

        public ProductSpecificationRepository _ProductSpecificationRepo
        {
            get
            {
                if (_productSpecificationRepo == null)
                    _productSpecificationRepo = new ProductSpecificationRepository(_db);
                return _productSpecificationRepo;
            }
        }

        public ProductRepository _ProductRepo
        {
            get
            {
                if (_productRepo == null)
                    _productRepo = new ProductRepository(_db);
                return _productRepo;
            }
        }

        public SlideRepository _SlideRepo
        {
            get
            {
                if (_slideRepo == null)
                    _slideRepo = new SlideRepository(_db);
                return _slideRepo;
            }
        }

        public CommentRepository _CommentRepo
        {
            get
            {
                if (_commentRepo == null)
                    _commentRepo = new CommentRepository(_db);
                return _commentRepo;
            }
        }

        public CartRepository _CartRepo
        {
            get
            {
                if (_cartRepo == null)
                    _cartRepo = new CartRepository(_db);
                return _cartRepo;
            }
        }

        public StatisticsRepository _StatisticsRepo
        {
            get
            {
                if (_statisticsRepo == null)
                    _statisticsRepo = new StatisticsRepository(_db);
                return _statisticsRepo;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
