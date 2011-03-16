using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace BVSoftware.Web.Data
{
    public class EntityFrameworkRepository<T>: IRepositoryStrategy<T> where T : class, new()
    {
        private System.Data.Objects.ObjectContext _objectContext = null;
        private System.Data.Objects.ObjectSet<T> _objectSet = null;

        private IObjectSet<T> objectSet
        {
            get
            {
                if (_objectSet == null)
                {
                    _objectSet = this._objectContext
                        .CreateObjectSet<T>();
                }
                return _objectSet;
            }
        }
        
        private bool _AutoSubmit = true;
        public bool AutoSubmit
        {
            get
            {
                return _AutoSubmit;
            }
            set
            {
                _AutoSubmit = value;
            }
        }
        public Logging.ILogger Logger { get; set; }
        public object ObjectContext
        {
            get { return _objectContext; }
        }

        public EntityFrameworkRepository(System.Data.Objects.ObjectContext context)
        {
            Logger = new Logging.NullLogger();

            _objectContext = context;            

            // Call this to force init on _objectset
            IObjectSet<T> obj = objectSet;
            
        }
        
        public bool SubmitChanges()
        {
            if (_objectContext != null)
            {
                try
                {
                    // Use optimistic concurrency when saving changes.
                    int result = _objectContext.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
                    return true;
                }
                catch (System.Data.OptimisticConcurrencyException cex)
                {                    
                    Logger.LogMessage("Concurrency exception while saving changes in EntityFrameworkRespository" + cex.Message + " " + cex.StackTrace);                    
                }                
            }
            return false;            
        }
        public T FindByPrimaryKey(PrimaryKey id)
        {
            string setName = _objectSet.Context.DefaultContainerName + "." + _objectSet.EntitySet.Name;

            System.Data.EntityKey key = new System.Data.EntityKey(setName,id.KeyName,id.KeyAsObject());
            
            try
            {
                object found = _objectContext.GetObjectByKey(key);
                if (found != null)
                {
                    return (T)found;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public IQueryable<T> Find()
        {            
            try
            {
                return objectSet;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public bool Create(T item)
        {
            try
            {
                objectSet.AddObject(item);
                if (AutoSubmit) SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);                
            }
            return false;
        }
        public bool Delete(PrimaryKey id)
        {
            T found = FindByPrimaryKey(id);
            if (found == null) return false;

            objectSet.DeleteObject(found);
            if (_AutoSubmit) SubmitChanges();
            return true;            
        }
        public int CountOfAll()
        {
            try
            {
                return objectSet.Count();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return -1;
        }
      
    }
}
