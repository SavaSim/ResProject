
using LoadBalancer.Data;
using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadBalancer
{
	public class LoadBalancerClass : ILoadBalancer
	{
		private Queue<IDescription> buffer = new Queue<IDescription>(); //baza podataka load balancer-a
		private Queue<IWorker> allWorkers = new Queue<IWorker>();		//lista svih workera
		private ILogger logger = null;
		private int quantumTime; // fiksno vreme koje je odredjeno za obavljanje posla
		public LoadBalancerClass(int quantumTime, ILogger logger) 
		{
			this.quantumTime = quantumTime;
			this.logger = logger;
		}

		public void AddWorker(IWorker worker) //dodaje workera ukoliko ne postoji worker
		{
			if (!this.allWorkers.Contains(worker)) 
			{
				this.allWorkers.Enqueue(worker);
			}
		}

		public IWorker GetWorker()
		{
			while (true)
			{
				if (this.allWorkers.Count() == 0) return null; //provera liste workera
				IWorker currentWorker = this.allWorkers.Dequeue(); //izbacuje iz liste ako ima neki slobodan
				if (currentWorker.IsFreeToWork() && currentWorker.IsTurnedOn())
				{
					if (currentWorker.GetRemainingBurstTime() <= this.quantumTime) //round robin pocetak, ukoliko je qT vece ili jednako od GRBT onda ce zavrsiti posao i vratiti workera 
					{
						return currentWorker;
					}
					else
					{
						int workerBurstTime = currentWorker.GetRemainingBurstTime(); //ukoliko nije, onda to vreme oduzimamo od qT sve dok ne dodje do 0
						int remainingBurstTime = workerBurstTime - this.quantumTime;
						if(remainingBurstTime <= this.quantumTime)
                        {
							currentWorker.ResetRemainingBurstTime();
							return currentWorker;
                        } 
						else
                        {
							currentWorker.SetRemainingBurstTime(remainingBurstTime);
                        }
						this.allWorkers.Enqueue(currentWorker);
					}
				}
				else
				{
					this.allWorkers.Enqueue(currentWorker); //?
				}
			}
		}

		public void ReceiveData(IItem item)
		{
			logger.Log(String.Format("{0} Received Data from Writer\n", DateTime.Now)); //loger
			IDescription description = new Description(item); //smestamo u description jer trazi u zadatku
			buffer.Enqueue(description); //punimo privremenu bazu
			this.SendDataToWorker(); //saljemo workeru na obradu
		}

		public void SendDataToWorker()
		{
			IWorker availableWorker = null;
			do
			{
				availableWorker = this.GetWorker();
			} while (availableWorker == null);
			IDescription data = this.DequeueData();
			logger.Log(String.Format("{0} Worker " + availableWorker.GetID() + " started working!\n", DateTime.Now)); //loger
			Thread thread = new Thread(new ThreadStart(() => this.PromptWorkerToWork(availableWorker, data)));
			thread.Start();
			return;
		}

		public void PromptWorkerToWork(IWorker worker, IDescription data) //worker prima podatke iz LoadBalancera
		{
			worker.GetDataFromLoadBalancer(data);
			this.AddWorker(worker);
		}

		private IDescription DequeueData(int quantumTime = 1) //?
		{
			if (this.buffer.Count > 0)
			{
				logger.Log(String.Format("{0} Dequeued Data from Buffer\n", DateTime.Now));
				return this.buffer.Dequeue();
			}
			throw new Exception("No data in buffer!");
		}

		public Queue<IDescription> GetBuffer()
		{
			return this.buffer;
		}
	}
}
