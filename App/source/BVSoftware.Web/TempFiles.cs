using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.Web
{
    public class TempFiles
    {
        public static FileInfo GetTemporaryFileInfo()
        {
            string tempFileName;
            FileInfo myFileInfo;
            try
            {
                tempFileName = Path.GetTempFileName();
                myFileInfo = new FileInfo(tempFileName);
                myFileInfo.Attributes = FileAttributes.Temporary;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Unable to create temporary file: {0}" + e.Message);                
            }

            return myFileInfo;
        }
    }
}
