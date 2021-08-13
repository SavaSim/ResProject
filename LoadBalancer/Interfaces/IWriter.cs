using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface IWriter
	{
		void SetLogger(ILogger logger);
		void SetLoadBalancer(ILoadBalancer loadBalancer);
		void ReadData();
		void SendData(IItem data);
		void ToggleWorkerWorkStatus(IWorker worker);
		void ReadDataFromStdin(IItem data);
	}
}
