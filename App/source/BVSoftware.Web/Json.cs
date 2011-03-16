using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace BVSoftware.Web
{
    public static class Json
    {
            public static string ObjectToJson(this object target)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Serialize(target);
            }

            public static T ObjectFromJson<T>(string json)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Deserialize<T>(json);
            }

            public static T ObjectFromJson<T>(Stream stream)
            {
                StreamReader rdr = new StreamReader(stream);
                return ObjectFromJson<T>(rdr.ReadToEnd());
            }
    }
}
