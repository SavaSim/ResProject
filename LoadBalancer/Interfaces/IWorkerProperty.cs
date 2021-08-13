using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface IWorkerProperty
	{
		string GetCode();
		string GetWorkerValue();
	}
}
