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
            var ps = new ProxyStorage();
            ps.Init();
            var ovds = ProxyStorage.OVDCollection;
            var pcs = ProxyStorage.PCInfoCollection;

            Assert.IsTrue(ProxyStorage.PCInfoCollection.Count > 0 && ProxyStorage.OVDCollection.Count > 0);

        }
    }
}
