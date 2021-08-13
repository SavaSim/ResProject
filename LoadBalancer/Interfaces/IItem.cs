using LoadBalancer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Interfaces
{
	public interface IItem
	{
		string GetCode();
        string GetValue();
        void SetCode(string code);
        void SetValue(int value);
    }
}
