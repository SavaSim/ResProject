using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Data
{
	public class Description : IDescription
	{
		private string ID { get; set; }
		private IItem item { get; set; }
		private string dataset { get; set; }

		public Description(IItem item)
		{
			this.ID = Guid.NewGuid().ToString();
			this.item = item;
			this.SetDataset(item.GetCode());
		}

		public string GetID()
		{
			return this.ID;
		}
		public void SetDataset(string code)
		{
			if (code == "CODE_ANALOG" || code == "CODE_DIGITAL")
			{
				this.dataset = "CACD";
			}
			else if (code == "CODE_CUSTOM" || code == "CODE_LIMITSET")
			{
				this.dataset = "CCCL";
			}
			else if (code == "CODE_SINGLENOE" || code == "CODE_MULTIPLENODE")
			{
				this.dataset = "CSCM";
			}
			else
			{
				this.dataset = "CCCS";
			}
		}

		public string GetDataset()
		{
			return this.dataset;
		}

		public IItem GetItem()
		{
			return this.item;
		}
	}
}
