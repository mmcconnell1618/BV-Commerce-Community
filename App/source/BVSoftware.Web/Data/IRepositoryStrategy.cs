using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Data
{
    public interface IRepositoryStrategy<T>
           where T : class, new()
    {

        bool AutoSubmit { get; set; }
        bool SubmitChanges();

        object ObjectContext { get; }

        T FindByPrimaryKey(PrimaryKey id);
        IQueryable<T> Find();        
        bool Create(T item);
        bool Delete(PrimaryKey id);                
        int CountOfAll();

        Logging.ILogger Logger { get; set; }

    }

}
