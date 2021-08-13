using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
	public class CollectionDescription : ICollectionDescription
	{
		private string ID;
		private string dataset;
		private IHistoricalCollection historicalCollection = new HistoricalCollection();
		public CollectionDescription(string dataset, IHistoricalCollection historicalCollection)
		{
			this.ID = Guid.NewGuid().ToString();
			this.dataset = dataset;
			this.historicalCollection = historicalCollection;
		}

		public string GetDataset()
		{
			return this.dataset;
		}

		public IHistoricalCollection GetHistoricalCollection()
		{
			return this.historicalCollection;
		}

		public string GetID()
		{
			return this.ID;
		}
	}
}
