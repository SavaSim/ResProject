using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
	public class HistoricalCollection : IHistoricalCollection
	{
		private IWorkerProperty workerProperty;
		public IWorkerProperty GetWorkerProperty()
		{
			return this.workerProperty;
		}

		public void AddWorkerProperty(IWorkerProperty workerProperty)
		{
			this.workerProperty = workerProperty;
		}
	}
}
