using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Data
{
    public class PrimaryKey
    {
        public PrimaryKeyType KeyType { get; private set; }
        public string BvinValue { get; private set; }
        public int IntValue { get; private set; }
        public long LongValue { get; private set; }
        public Guid GuidValue { get; private set; }

        public PrimaryKey(string bvin)
        {
            BvinValue = bvin;
            KeyType = PrimaryKeyType.Bvin;
            KeyName = "bvin";
        }
        public PrimaryKey(int id)
        {
            IntValue = id;
            KeyType = PrimaryKeyType.Integer;
            KeyName = "Id";
        }
        public PrimaryKey(long id)
        {
            LongValue = id;
            KeyType = PrimaryKeyType.Long;
            KeyName = "Id";
        }
        public PrimaryKey(Guid id)
        {
            GuidValue = id;
            KeyType = PrimaryKeyType.Guid;
            KeyName = "Id";
        }

        public string KeyName { get; set; }

        public object KeyAsObject()
        {
            switch (KeyType)
            {                
                case PrimaryKeyType.Bvin:
                    return BvinValue;
                case PrimaryKeyType.Guid:
                    return GuidValue;
                case PrimaryKeyType.Integer:
                    return IntValue;
                case PrimaryKeyType.Long:
                    return LongValue;
            }
            return null;
        }

    }
}
