using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Web;
using BVSoftware.Web.Data;

namespace BVSoftware.Web.TestDomain
{
    public class ExampleRepository: ConvertingRepositoryBase<ExampleBaseDb, ExampleBase>
    {

        public static ExampleRepository InstantiateForMemory()
        {
            ExampleRepository result = null;

            result = new ExampleRepository(new MemoryStrategy<ExampleBaseDb>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<ExampleSubObjectDb>(PrimaryKeyType.Long));

            return result;
        }
        public static ExampleRepository InstantiateForDatabase()
        {
            throw new NotImplementedException();
        }

        private ExampleSubRepository subrepository = null;

        private ExampleRepository(IRepositoryStrategy<ExampleBaseDb> r, IRepositoryStrategy<ExampleSubObjectDb> subr)
        {
            repository = r;
            this.logger = new BVSoftware.Web.Logging.NullLogger();
            repository.Logger = this.logger;
            
            subrepository = new ExampleSubRepository(subr, this.logger);            
        }

        protected override void CopyModelToData(ExampleBaseDb data, ExampleBase model)
        {
            data.bvin = model.bvin;
            data.DescriptionDb = model.Description;
            data.IsActiveDb = model.IsActive;
            data.LastUpdatedUtcDb = model.LastUpdatedUtc;                        
        }

        protected override void CopyDataToModel(ExampleBaseDb data, ExampleBase model)
        {
            model.bvin = data.bvin;
            model.Description = data.DescriptionDb;
            model.IsActive = data.IsActiveDb;
            model.LastUpdatedUtc = data.LastUpdatedUtcDb;            
        }
       
        protected override void DeleteAllSubItems(ExampleBase model)
        {
            subrepository.DeleteForBase(model.bvin);
        }
        protected override void GetSubItems(ExampleBase model)
        {
            model.SubObjects = subrepository.FindForBase(model.bvin);
        }
        protected override void MergeSubItems(ExampleBase model)
        {
            subrepository.MergeList(model.bvin, model.SubObjects);
        }
       
       
        public override bool Create(ExampleBase item)
        {
            if (item == null) return false;

            if (item.bvin == string.Empty) item.bvin = System.Guid.NewGuid().ToString();
            item.LastUpdatedUtc = DateTime.UtcNow;

            return base.Create(item);
        }

        public bool Update(ExampleBase item)
        {
            return base.Update(item, new PrimaryKey(item.bvin));
        }
        public ExampleBase Find(string bvin)
        {
            return Find(new PrimaryKey(bvin));
        }
        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }

        public List<ExampleSubObject> PeakIntoSubObjects(string baseBvin)
        {
            return subrepository.FindForBase(baseBvin);
        }
    }
}
