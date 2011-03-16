using System;
using System.Collections.Generic;
using System.Text;

namespace BVSoftware.Web.Barcodes
{
    interface IBarcode
    {
        string Encoded_Value
        {
            get;
        }

        string RawData
        {
            get;
        }

        string FormattedData
        {
            get;
        }
    }
}
