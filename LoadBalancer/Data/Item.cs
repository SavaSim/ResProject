using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
    public struct Item : IItem
    {
        
        private string code;
        private int value;


        public Item(string code, int value)
        {
            this.code = code;
            this.value = value;
        }
        public string GetCode()
        {
            return this.code.ToString();
        }

        public string GetValue()
        {
            return this.value.ToString();
        }

        public void SetCode(string code)
        {
            this.code = code;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }

    }
}
