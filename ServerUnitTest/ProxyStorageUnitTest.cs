using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARMInfoServer;

namespace ServerUnitTest
{
    [TestClass]
    public class ProxyStorageUnitTest
    {
        [TestMethod]
        public void LoadingCollectionsTest()
        {
            var ps = ProxyStorage.Instance;
            ps.Load();
            var ovds = ps.OVDCollection;
            var pcs = ps.PCInfoCollection;

            Assert.IsTrue(ps.PCInfoCollection.Count > 0 && ps.OVDCollection.Count > 0);

        }
    }
}
