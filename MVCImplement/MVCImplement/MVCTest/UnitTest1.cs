namespace MVCTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int a = 1;
            int b = 2;
            int sum = a + b;
            Assert.AreEqual(3, sum);
        }
    }
}