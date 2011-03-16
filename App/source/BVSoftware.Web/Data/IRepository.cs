using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Web.Logging;

namespace BVSoftware.Web.Data
{
    public interface IRepository<T>
     where T : class, new()
    {
        bool AutoSubmit { get; set; }
        bool SubmitChanges();

        bool Create(T item);
        int CountOfAll();

        ILogger Logger { get; set; }
    }
}
