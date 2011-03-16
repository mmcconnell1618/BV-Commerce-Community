using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BVSoftware.Web.Data;
using BVSoftware.Web.Logging;
using BVSoftware.Web;
using BVSoftware.Web.TestDomain;

namespace BVSoftware.Web.Test
{
    [TestClass]
    public class ExampleRepositoryTests
    {
        [TestMethod]
        public void CanCreateRepository()
        {                                 
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();

            Assert.IsNotNull(repository, "Repository shouldn't be null");
        }

        [TestMethod]
        public void CanCreateObjectInRepository()
        {
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();

            ExampleBase o = new ExampleBase()
            {
                LastUpdatedUtc = new DateTime(2001, 1, 1),
                Description = "This is an example base",
                IsActive = true
            };

            Assert.IsTrue(repository.Create(o), "Create should be true");
            Assert.AreNotEqual(string.Empty, o.bvin, "Bvin should not be empty");
            Assert.AreEqual(DateTime.UtcNow.Year, o.LastUpdatedUtc.Year, "Last updated date should match current year");
        }

        [TestMethod]
        public void CanFindObjectInRepositoryAfterCreate()
        {
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();

            ExampleBase o = new ExampleBase()
            {
                LastUpdatedUtc = new DateTime(2001, 1, 1),
                Description = "This is an example base",
                IsActive = true
            };

            repository.Create(o);

            string targetId = o.bvin;

            ExampleBase found = repository.Find(targetId);

            Assert.IsNotNull(found, "Found item should not be null");
            Assert.AreEqual(o.bvin, found.bvin, "Bvin should match");
            Assert.AreEqual(o.Description, found.Description, "Bvin should match");
            Assert.AreEqual(o.IsActive, found.IsActive, "IsActive should match");
            Assert.AreEqual(o.LastUpdatedUtc.Ticks, found.LastUpdatedUtc.Ticks, "Last Updated should match");
            Assert.AreEqual(o.SubObjects.Count, found.SubObjects.Count, "Sub object count should match");
        }

        [TestMethod]
        public void CanCreateSubItemsDuringCreate()
        {
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();

            ExampleBase o = new ExampleBase();
            o.Description = "Test Base";
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub A" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub B" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub C" });

            bool result = repository.Create(o);
            Assert.IsTrue(result, "Create should return true");

            Assert.AreEqual(3, o.SubObjects.Count, "Sub object count should be three");
            foreach (ExampleSubObject sub in o.SubObjects)
            {
                Assert.IsTrue(sub.Id > 0, "Sub object " + sub.Name + " should have and ID > 0");
                Assert.IsTrue(sub.BaseId == o.bvin, "Sub object " + sub.Name + " should have received base bvin");
                Assert.IsTrue(sub.SortOrder > 0, "Sub object " + sub.Name + " should have sort order > 0");
            }
            
        }

        [TestMethod]
        public void CanMergeSubItems()
        {
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();


            // Create Basic Sample
            ExampleBase o = new ExampleBase();
            o.Description = "Test Base";
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub A" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub B" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub C" });
            bool result = repository.Create(o);

            ExampleBase existing = repository.Find(o.bvin);
            Assert.AreEqual(o.SubObjects.Count, existing.SubObjects.Count, "Sub object count should be equal");
            Assert.AreEqual(o.SubObjects[0].Name, existing.SubObjects[0].Name, "First item name should be equal");

            existing.SubObjects.Add(new ExampleSubObject() { Name = "New Sub A" }); // index 3, then 2
            existing.SubObjects.Add(new ExampleSubObject() { Name = "New Sub B" }); // index 4, then 3
            existing.SubObjects[0].Name = "Updated Sub A";
            existing.SubObjects.RemoveAt(2);

            Assert.IsTrue(repository.Update(existing), "update should be true");

            ExampleBase target = repository.Find(o.bvin);
            Assert.IsNotNull(target, "target should not be null");
            Assert.AreEqual(4, target.SubObjects.Count, "Sub object count should be four after merge");

            Assert.AreEqual("Updated Sub A", target.SubObjects[0].Name, "First sub name didn't match");
            Assert.AreEqual("Test Sub B", target.SubObjects[1].Name, "Second name didn't match");
            Assert.AreEqual("New Sub A", target.SubObjects[2].Name, "Third name didn't match");
            Assert.AreEqual("New Sub B", target.SubObjects[3].Name, "Fourth Name didn't match");
        }

        [TestMethod]
        public void CanDeleteSubsOnDelete()
        {
            ExampleRepository repository = ExampleRepository.InstantiateForMemory();


            // Create Basic Sample
            ExampleBase o = new ExampleBase();
            o.Description = "Test Base";
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub A" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub B" });
            o.SubObjects.Add(new ExampleSubObject() { Name = "Test Sub C" });
            bool result = repository.Create(o);

            ExampleBase existing = repository.Find(o.bvin);
            Assert.AreEqual(o.SubObjects.Count, existing.SubObjects.Count, "Sub object count should be equal");
            Assert.AreEqual(o.SubObjects[0].Name, existing.SubObjects[0].Name, "First item name should be equal");

            Assert.IsTrue(repository.Delete(o.bvin));            

            ExampleBase target = repository.Find(o.bvin);
            Assert.IsNull(target, "target should not null after delete");

            List<ExampleSubObject> subs = repository.PeakIntoSubObjects(o.bvin);
            Assert.IsNotNull(subs, "Sub list should NOT be null but should be empty.");
            Assert.AreEqual(0, subs.Count, "Sub list should be empty.");
        }
    }
}
