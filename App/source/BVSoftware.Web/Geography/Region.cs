using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BVSoftware.Web.Geography
{
    public class Region
    {
        private string _Name = string.Empty;
        private string _Abbreviation = string.Empty;

        public string Name
        {
            get { return _Name; }

            set { _Name = value; }
        }
        public string Abbreviation
        {
            get { return _Abbreviation; }

            set { _Abbreviation = value; }
        }

        public Region()
        {
        }

        public Region(string name, string abbreviation)
        {
            _Name = name;
            _Abbreviation = abbreviation;
        }

    }
}
