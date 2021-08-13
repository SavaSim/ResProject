using LoadBalancer.Data;
using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadBalancer
{
	public class Writer : IWriter
	{
		private ILogger logger = new Logger();
		private ILoadBalancer loadBalancer;
		private CodeList codeList = new CodeList();

		public Writer() { }

		public void SetLoadBalancer(ILoadBalancer loadBalancer)
		{
			this.loadBalancer = loadBalancer;
		}

		public void SetLogger(ILogger logger)
		{
			this.logger = logger;
		}

		public void ReadData()
		{
			while(true)
			{
				Random random = new Random(); //upisuje random na svake 2 sekunde, kao sto je trazeno u zadatku
				IItem data = new Item();
				data.SetCode(codeList.GetCode(random.Next(0, 8)));
				data.SetValue((random.Next(0, 100)));
				this.SendData(data); //salje LB-u
				////Thread.Sleep(2000);
			}
        }

		public void SendData(IItem data) 
		{
			loadBalancer.ReceiveData(data); //LB prima
		}

		public void ReadDataFromStdin(IItem data) //ovde valjda salje ono sto smo mi upisali
		{
			this.SendData(data);
		}

		public void ToggleWorkerWorkStatus(IWorker worker) //paljenje i gasenje workera rucno
		{
			if (worker.IsTurnedOn())
			{
				worker.TurnOff();
				logger.Log(String.Format("{0} Worker " + worker.GetID() + " turned off\n", DateTime.Now));
			}
			else
			{
				worker.TurnOn();
				logger.Log(String.Format("{0} Worker " + worker.GetID() + " turned on\n", DateTime.Now));
			}
		}
	}
}
