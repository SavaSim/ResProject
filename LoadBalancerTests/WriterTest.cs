
using LoadBalancer;
using LoadBalancer.Data;
using LoadBalancer.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LoadBalancerTests
{
	[TestFixture]
	public class WriterTest
	{
        public static IWriter writer = new Writer();
        public static ILogger logger = new Logger();
        private static CodeList codeList = new CodeList();
        private static IWorker worker1 = new Worker(2, "Raja", logger);
        private static IWorker worker2 = new Worker(5, "Gaja", logger);
        private static IWorker worker3 = new Worker(1, "Paja", logger);
        private static IWorker worker4 = new Worker(3, "Vlaja", logger);
        [TestCase("CODE_ANALOG", 1)]
        [TestCase("CODE_DIGITAL", 1)]
        [TestCase("CODE_CUSTOM", 1)]
        [TestCase("CODE_LIMITSET", 1)]
        [TestCase("CODE_SOURCE", 1)]
        [TestCase("CODE_CONSUMER", 1)]
        [TestCase("CODE_SINGLENOE", 1)]
        [TestCase("CODE_MULTIPLENODE", 1)]
        public void TestReadData(string code, int value)
        {
            ILoadBalancer loadBalancer = new LoadBalancer.LoadBalancer(2, logger);
            loadBalancer.AddWorker(worker1);
            loadBalancer.AddWorker(worker2);
            loadBalancer.AddWorker(worker3);
            loadBalancer.AddWorker(worker4);
            writer.SetLoadBalancer(loadBalancer);
            IItem item = new Item();
            item.SetValue(value);
            item.SetCode(code);
            Queue<IDescription> testCollection = new Queue<IDescription>();
            IDescription description = new Description(item);
            testCollection.Enqueue(description);

            writer.ReadDataFromStdin(item);
            Assert.AreEqual(loadBalancer.GetBuffer(), testCollection);
        }
    }
}
