using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BVSoftware.Web.Logging;

namespace BVSoftware.Web.Data
{
    public class MemoryStrategy<T> : IRepositoryStrategy<T> where T : class, new()
    {
        private long _IdentityCounter = 0;
        private PrimaryKeyType _keyType = PrimaryKeyType.Bvin;
        private List<T> items = new List<T>();        
        public BVSoftware.Web.Logging.ILogger Logger { get; set; }

        public bool AutoSubmit
        {
            get { return true; }
            set { } // do nothing
        }
        public MemoryStrategy(PrimaryKeyType keyType)
        {
            _keyType = keyType;
            Logger = new Logging.NullLogger();
        }        
        public MemoryStrategy(PrimaryKeyType keyType, BVSoftware.Web.Logging.ILogger logger)
        {
            _keyType = keyType;
            Logger = logger;         
        }
        public object ObjectContext
        {
            get { return null; }
        }

        public T FindByPrimaryKey(PrimaryKey key)
        {
            return items.SingleOrDefault<T>(delegate(T t)
            {
                //long currentId = (long)t.GetType().GetProperty("StoreId").GetValue(t, null);
                switch (key.KeyType)
                {
                    case PrimaryKeyType.Bvin:
                        string memberId = t.GetType().GetProperty(key.KeyName).GetValue(t, null).ToString();
                        return memberId.Trim().ToLowerInvariant() == key.BvinValue.Trim().ToLowerInvariant();
                    case PrimaryKeyType.Guid:
                        Guid guidmemberId = (Guid)t.GetType().GetProperty(key.KeyName).GetValue(t, null);
                        return guidmemberId.ToString() == key.GuidValue.ToString();                        
                    case PrimaryKeyType.Integer:
                        int intmemberId = (int)t.GetType().GetProperty(key.KeyName).GetValue(t, null);
                        return intmemberId == key.IntValue;                        
                    case PrimaryKeyType.Long:
                        long longmemberId = (long)t.GetType().GetProperty(key.KeyName).GetValue(t, null);
                        return longmemberId == key.LongValue;                        
                }
                return false;
            });
        }
       
        public IQueryable<T> Find()
        {
            try
            {
                return items.AsQueryable<T>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public bool Create(T item)
        {
            bool result = false;
            
            try
            {
                SetPrimaryKey(item);
                items.Add(item);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Logger.LogException(ex);
            }

            return result;
        }

        private void SetPrimaryKey(T item)
        {
            _IdentityCounter += 1;
            PropertyInfo prop = item.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

            switch (_keyType)
            {
                case PrimaryKeyType.Bvin:
                    // do nothing, handled by external code
                    break;
                case PrimaryKeyType.Guid:
                    // Assign Guid                                        
                    if(null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, System.Guid.NewGuid(), null);                        
                    }
                    break;
                case PrimaryKeyType.Integer:
                    // Assign Integer
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, (int)_IdentityCounter, null);
                    }
                    break;
                case PrimaryKeyType.Long:
                    // Assign Long
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, _IdentityCounter, null);
                    }
                    break;
            }
        }

        public bool SubmitChanges()
        {
            return true;
        }

        public bool Delete(PrimaryKey key)
        {
            bool result = false;


            try
            {
                T existing = FindByPrimaryKey(key);

                if (existing != null)
                {
                    items.Remove(existing);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logger.LogException(ex);
            }

            return result;
        }
       
        public int CountOfAll()
        {
            int result = 0;

            try
            {
                result = items.Count();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
    }
}
