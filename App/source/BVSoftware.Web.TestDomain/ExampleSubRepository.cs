using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Web.Data;
using BVSoftware.Web.Logging;

namespace BVSoftware.Web.TestDomain
{
    class ExampleSubRepository: ConvertingRepositoryBase<ExampleSubObjectDb, ExampleSubObject>
    {

        protected override void CopyModelToData(ExampleSubObjectDb data, ExampleSubObject model)
        {
            data.BaseIdDb = model.BaseId;
            data.Id = model.Id;
            data.NameDb = model.Name;
            data.SortOrderDb = model.SortOrder;            
        }

        protected override void CopyDataToModel(ExampleSubObjectDb data, ExampleSubObject model)
        {
            model.BaseId = data.BaseIdDb;
            model.Id = data.Id;
            model.Name = data.NameDb;
            model.SortOrder = data.SortOrderDb;
        }

        public ExampleSubRepository(IRepositoryStrategy<ExampleSubObjectDb> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;            
        }

        public override bool Create(ExampleSubObject item)
        {
            item.SortOrder = FindMaxSort(item.BaseId) + 1;
            return base.Create(item);
        }
        private int FindMaxSort(string baseId)
        {
            int maxSort = 0;            
            List<ExampleSubObject> result = new List<ExampleSubObject>();
            IQueryable<ExampleSubObjectDb> items = repository.Find().Where(y => y.BaseIdDb == baseId)
                                                                      .OrderByDescending(y => y.SortOrderDb)
                                                                      .Take(1);
            if (items != null)
            {
                var i = items.ToList();
                if (i.Count > 0)
                {
                    maxSort = i[0].SortOrderDb;
                }
            }
            return maxSort;
        }

        public bool Update(ExampleSubObject item)
        {            
            return base.Update(item, new PrimaryKey(item.Id));
        }

        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<ExampleSubObject> FindForBase(string baseBvin)
        {
            var items = repository.Find().Where(y => y.BaseIdDb == baseBvin)
                                        .OrderBy(y => y.SortOrderDb);
            return ListPoco(items);
        }

        public void DeleteForBase(string baseBvin)
        {
            List<ExampleSubObject> existing = FindForBase(baseBvin);
            foreach (ExampleSubObject sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string baseBvin, List<ExampleSubObject> subitems)
        {
            // Set Base Key Field
            foreach (ExampleSubObject item in subitems)
            {
                item.BaseId = baseBvin;
            }

            // Create or Update
            foreach (ExampleSubObject itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<ExampleSubObject> existing = FindForBase(baseBvin);
            foreach (ExampleSubObject ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Id);
                }
            }
        }

    }
}
