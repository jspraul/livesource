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
	}
}

