using System;
using System.Threading;
using System.Threading.Tasks;
public class StockPrice
{
    public String name {get; private set; }
    private int price;
	private int readerCount;
	private int writerCount;
    public StockPrice(String _name, int _price)
	{
		name = _name;
		price = _price;
	}

    public int Price(int? threadID)
    {
		// Acquire read lock.
		lock (this)
		{
			// Condition: There are no writes in progress.
			while (writerCount > 0)
				Monitor.Wait(this);
			readerCount++;
		}

		try
		{
			System.Console.WriteLine("{0} read price of {1}: {2}", threadID, name, price);
			return price;
		}
		finally
		{
			// Release read lock.
			lock (this)
			{
				readerCount--;
				Monitor.PulseAll(this);
			}
			
		}
    }

    public void updatePrice (int? threadID, int amount)
	{
		// Acquire write lock.
		lock (this)
		{
			// Condition: There are no reads or writes in progress.
			while (readerCount > 0 || writerCount > 0)
				Monitor.Wait(this);
			writerCount++;
		}

		try
		{
			System.Console.WriteLine("Thread: {0} updated Price of {1}: {2}", threadID, name, amount);
            
			price += amount;
		}
		finally
		{
			// Release write lock.
			lock (this)
			{
				writerCount--;
				Monitor.PulseAll(this);
			}
            
		}
	}
}