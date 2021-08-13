using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface ICollectionDescription
	{
		string GetID();
		string GetDataset();
		IHistoricalCollection GetHistoricalCollection();
	}
}
