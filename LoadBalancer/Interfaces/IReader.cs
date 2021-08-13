using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	interface IReader
	{
		void ReadFromDB(string file);
	}
}
