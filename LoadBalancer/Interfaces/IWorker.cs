using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface IWorker
	{
		string GetID();
		void TurnOn();
		void TurnOff();
		void ToggleWorking();
		Boolean IsTurnedOn();
		Boolean IsFreeToWork();
		void Work(IDescription data);
		int GetBurstTime();
		void SetBurstTime(int burstTime);
		void ResetRemainingBurstTime();
		int GetRemainingBurstTime();
		void SetRemainingBurstTime(int SetRemainingBurstTime);
		void GetDataFromLoadBalancer(IDescription description);
	}
}
