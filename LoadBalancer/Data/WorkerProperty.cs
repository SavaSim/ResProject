using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
	public class WorkerProperty : IWorkerProperty
	{
		private string code;
		private string workerValue;
		public WorkerProperty(string code, string workerValue)
		{
			this.code = code;
			this.workerValue = workerValue;
		}
		public string GetCode()
		{
			return this.code;
		}

		public string GetWorkerValue()
		{
			return this.workerValue;
		}
	}
}
