using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Text;

namespace BVSoftware.Web.Data
{

        // T = the linq to sql proxy class
        // V = your POCO class
        public abstract class ConvertingRepositoryBase<T, V>
            where T : class, new()
            where V : class, new()
        {
            protected Logging.ILogger logger = new Logging.NullLogger();
            protected IRepositoryStrategy<T> repository = null;

            // override these to translate between LINQ and POCO
            protected abstract void CopyModelToData(T data, V model);
            protected abstract void CopyDataToModel(T data, V model);
            protected virtual void GetSubItems(V model) { }
            protected virtual void MergeSubItems(V model) { }
            protected virtual void DeleteAllSubItems(V model) { }

            // Use these methods in your Find queries to tranlate from an
            // IQueryable<T> to a single or list of V
            protected virtual V SinglePoco(IQueryable<T> items)
            {
                T item = items.SingleOrDefault();
                if (item != null)
                {
                    V result = new V();
                    CopyDataToModel(item, result);
                    GetSubItems(result);
                    return result;
                }

                return null;
            }
            // Use these methods in your Find queries to tranlate from an
            // IQueryable<T> to a single or list of V
            protected virtual V FirstPoco(IQueryable<T> items)
            {
                T item = items.FirstOrDefault();
                if (item != null)
                {
                    V result = new V();
                    CopyDataToModel(item, result);
                    GetSubItems(result);
                    return result;
                }

                return null;
            }
            protected virtual List<V> ListPoco(IQueryable<T> items)
            {
                List<V> result = new List<V>();

                if (items != null)
                {
                    foreach (T item in items)
                    {
                        V temp = new V();
                        CopyDataToModel(item, temp);
                        GetSubItems(temp);
                        result.Add(temp);
                    }
                }

                return result;
            }
            

            // Note: No longer creates GUID values automatically
            public virtual bool Create(V item)
            {
                bool result = false;

                if (item != null)
                {
                    T dataObject = new T();
                    CopyModelToData(dataObject, item);
                    result = repository.Create(dataObject);

                    if (result)
                    {                        
                        CopyDataToModel(dataObject, item);
                        MergeSubItems(item);
                        GetSubItems(item);
                    }
                }


                return result;
            }

            protected virtual V Find(PrimaryKey key)
            {
                V result = default(V);

                T existing = repository.FindByPrimaryKey(key);
                if (existing != null)
                {
                    result = new V();
                    CopyDataToModel(existing, result);
                    GetSubItems(result);
                }
                else
                {
                    return null;
                }

                return result;
            }            
           
            public virtual List<V> FindAllPaged(int pageNumber, int pageSize)
            {
                List<V> result = new List<V>();

                if (pageNumber < 1) pageNumber = 1;

                int take = pageSize;
                int skip = (pageNumber - 1) * pageSize;

                // Note: silly OrderBy(y => true) is so that entity framework provider
                // won't freak out with skip and take operators.
                // They only work on a sorted result because they are LINQ operators
                IQueryable<T> items = repository.Find().OrderBy(y => true).Skip(skip).Take(take);
                if (items != null)
                {
                    result = ListPoco(items);
                }

                return result;
            }

            protected virtual IQueryable<T> PageItems(int pageNumber, int pageSize, IQueryable<T> items)
            {
                if (pageNumber < 1) pageNumber = 1;
                int take = pageSize;
                int skip = (pageNumber - 1) * pageSize;
                return items.Skip(skip).Take(take);                
            }

            protected virtual bool Update(V m, PrimaryKey key)
            {
                bool result = false;

                if (m != null)
                {
                    try
                    {
                        T existing = repository.FindByPrimaryKey(key);
                        if (existing == null)
                        {
                            return false;
                        }
                        CopyModelToData(existing, m);
                        result = repository.SubmitChanges();
                        if (result)
                        {
                            MergeSubItems(m);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        logger.LogException(ex);                        
                    }
                }

                return result;
            }
           
            protected virtual bool Delete(PrimaryKey key)
            {
                DeleteAllSubItems(Find(key));                
                return repository.Delete(key);
            }
            public virtual int CountOfAll()
            {
                return repository.CountOfAll();                
            }

            public virtual bool AutoSubmit
            {
                get { return this.repository.AutoSubmit; }
                set { this.repository.AutoSubmit = value; }
            }
            public virtual bool SubmitChanges()
            {
                return this.repository.SubmitChanges();
            }
        }



}
