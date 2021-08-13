using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
	public class CodeList
	{
        public string GetCode(int value)
        {
            if (value == 0)
            {
                return "CODE_ANALOG";
            }
            else if (value == 1)
            {
                return "CODE_DIGITAL";
            }
            else if (value == 2)
            {
                return "CODE_CUSTOM";
            }
            else if (value == 3)
            {
                return "CODE_LIMITSET";
            }
            else if (value == 4)
            {
                return "CODE_SINGLENOE";
            }
            else if (value == 5)
            {
                return "CODE_MULTIPLENODE";
            }
            else if (value == 6)
            {
                return "CODE_CONSUMER";
            }
            else if (value == 7)
            {
                return "CODE_SOURCE";
            }
            else
            {
                return "";
            }
        }
    }
}
