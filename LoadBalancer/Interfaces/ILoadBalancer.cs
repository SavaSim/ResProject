using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface ILoadBalancer
	{
		void ReceiveData(IItem data);
		void SendDataToWorker();
		IWorker GetWorker();
		void AddWorker(IWorker worker);
		Queue<IDescription> GetBuffer();
	}
}
