using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    public class Migrator: IMigrator
    {

        public event MigrationService.ProgressReportDelegate ProgressReport;
        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }

        public void Migrate(MigrationSettings settings)
        {
            wl("BVC 2004 Migrator Started");
        }
    }
}
