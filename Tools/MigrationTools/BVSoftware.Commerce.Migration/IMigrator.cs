using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration
{
    public interface IMigrator
    {
        event MigrationService.ProgressReportDelegate ProgressReport;
        
        void Migrate(MigrationSettings settings);
    }
}
