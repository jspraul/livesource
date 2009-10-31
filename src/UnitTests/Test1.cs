using System;
using NUnit.Framework;

namespace LiveSource.UnitTests
{
	[TestFixture]
	public class Test1
	{
		[Test]
		public void ThisIsAnExampleTest()
		{
		    int next = new Random().Next(100);

            for(int i=0; i < next; i++)
            {
                if (i == next)
                {
                    Console.WriteLine("i == next");
                    return;
                }
            }

		    Assert.AreEqual(2, 1+1);
		}

        [Test]
        public void ItShouldPlaceLogStatementAfterTheIfBlock()
        {
            int next = new Random().Next(100);

            if (next > 50)
            {
                Console.WriteLine("next is greater than 50");
            }

            Console.WriteLine("it should place trace statement here");
        }
	}
}

