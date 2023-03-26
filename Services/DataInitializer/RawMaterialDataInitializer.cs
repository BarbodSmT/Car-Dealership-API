using Data.Repositories;
using Entities;
using System.Linq;
using Data.Contracts;
/*
namespace Services.DataInitializer
{
    
    public class RawMaterialDataInitializer : IDataInitializer
    {
        private readonly IRepository<RawMaterial> rrepository;
        private readonly IRepository<Service> srepository;

        public RawMaterialDataInitializer(IRepository<RawMaterial> rrepository, IRepository<Service> srepository)
        {
            this.rrepository = rrepository;
            this.srepository = srepository;
        }

        public void InitializeData()
        {
            if (!rrepository.TableNoTracking.Any(p => p.Name == "مواد اولیه"))
            {
                rrepository.Add(new RawMaterial
                {
                    Name = "مواد اولیه",
                    Price = 100,
                    Service = null,
                    ServiceId = null
                });
            }
            if (!srepository.TableNoTracking.Any(p => p.Name == "سرویس اولیه"))
            {
                srepository.Add(new Service
                {
                    Name = "سرویس اولیه",
                    Price = 0,
                    RawMaterials = new List<RawMaterial>(),
                    Date = new DateTime()
                    
                });
            }
        }
    }
}
*/