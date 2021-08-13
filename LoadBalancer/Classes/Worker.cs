using LoadBalancer.Data;
using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoadBalancer
{
	public class Worker : IWorker
	{
		private bool freeToWork = true;
		private bool turnedOn = true;
		private int burstTime;
		private int remainingBurstTime;
		public string id;
		public bool deadband = false;
		public List<IDescription> descriptions = new List<IDescription>();
		private List<ICollectionDescription> collectionDescriptions = new List<ICollectionDescription>();
		string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
		private ILogger logger = null;
		Object obj = new object();

		public Worker(int burstTime, string id, ILogger logger)
		{
			this.burstTime = burstTime;
			this.remainingBurstTime = burstTime;
			this.id = id;
			this.logger = logger;
		}
		public void SetRemainingBurstTime(int remainingBurstTime) //postavlja preostalo vreme za rad
        {
			this.remainingBurstTime = remainingBurstTime;
        }

		public void ResetRemainingBurstTime() //resetuje vreme za rad
        {
			this.remainingBurstTime = this.burstTime;
        }

		public int GetRemainingBurstTime() 
        {
			return this.remainingBurstTime;
        }

		public int GetBurstTime()
		{
			return this.burstTime;
		}

		public bool IsFreeToWork()
		{
			return this.freeToWork;
		}

		public bool IsTurnedOn()
		{
			return this.turnedOn;
		}

		public void SetBurstTime(int burstTime)
		{
			this.burstTime = burstTime;
		}

		public void ToggleWorking()
		{
			if (this.freeToWork)
			{
				this.freeToWork = false;
			}
			else 
			{
				this.freeToWork = true;
			}
		}

		public void TurnOff()
		{
			if (this.turnedOn)
			{
				this.turnedOn = false;
			}
		}

		public void TurnOn()
		{
			if (!this.turnedOn)
			{
				this.turnedOn = true;
			}
		}

		public void GetDataFromLoadBalancer(IDescription description)
		{
			logger.Log(String.Format("{0} Worker " + this.GetID() + " got data from LB!\n", DateTime.Now));
			this.descriptions.Add(description); //
			this.Work(this.descriptions.Find((x) => x.GetID() == description.GetID())); //vadi po ID-ju iz liste description
			return;
		}

		public void Work(IDescription data)
		{
			this.ToggleWorking();
			logger.Log(String.Format("{0} Worker " + this.GetID() + " recived data and start with working\n", DateTime.Now));
			for (int i = 0; i < this.descriptions.Count(); i++)
			{
				IDescription description = this.descriptions[i];
				IHistoricalCollection historicalCollection = new HistoricalCollection();
				IItem item = description.GetItem();
				IWorkerProperty workerProperty = new WorkerProperty(item.GetCode(), item.GetValue());
				historicalCollection.AddWorkerProperty(workerProperty);

				if (description.GetDataset() == "CACD")
				{
					ICollectionDescription collectionDescription = new CollectionDescription(description.GetDataset(), historicalCollection);
					this.collectionDescriptions.Add(collectionDescription);
					this.descriptions.Remove(description);
				}
				else if (description.GetDataset() == "CCCL")
				{
					ICollectionDescription collectionDescription = new CollectionDescription(description.GetDataset(), historicalCollection);
					this.collectionDescriptions.Add(collectionDescription);
					this.descriptions.Remove(description);
				}
				else if (description.GetDataset() == "CCCS")
				{
					ICollectionDescription collectionDescription = new CollectionDescription(description.GetDataset(), historicalCollection);
					this.collectionDescriptions.Add(collectionDescription);
					this.descriptions.Remove(description);
				}
				else 
				{
					ICollectionDescription collectionDescription = new CollectionDescription(description.GetDataset(), historicalCollection);
					this.collectionDescriptions.Add(collectionDescription);
					this.descriptions.Remove(description);
				}
			}
			this.Categorize();
			this.ToggleWorking();
		}

		public void Categorize()
		{
			Object obj = new Object();
			for (int i = 0; i < this.collectionDescriptions.Count(); i++)
			{
				ICollectionDescription firstCollectionDescription = this.collectionDescriptions[i];
				if (firstCollectionDescription.GetDataset() == "CACD")
				{
					for (int j = 0; j < this.collectionDescriptions.Count(); j++)
					{
						ICollectionDescription secondCollectionDescription = collectionDescriptions[j];
						if (secondCollectionDescription.GetID() != firstCollectionDescription.GetID()) //proverava da li je isti ID
						{
							IHistoricalCollection firstHistoricalCollection = firstCollectionDescription.GetHistoricalCollection();
							IWorkerProperty firstWorkerProperty = firstHistoricalCollection.GetWorkerProperty();
							if (secondCollectionDescription.GetDataset() == "CACD")
							{
								IHistoricalCollection secondHistoricalCollection = secondCollectionDescription.GetHistoricalCollection();
								IWorkerProperty secondWorkerProperty = secondHistoricalCollection.GetWorkerProperty();
								if (firstWorkerProperty.GetCode() != secondWorkerProperty.GetCode())
								{
									if (firstWorkerProperty.GetCode().ToString() == "CODE_ANALOG")
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(firstCollectionDescription, secondCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CACD", tuple, false);

										}
									}
									else
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(secondCollectionDescription, firstCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CACD", tuple, false);

										}
									}
								}
							}
						}
					}
				} else if (firstCollectionDescription.GetDataset() == "CCCS")
				{
					for (int j = 0; j < this.collectionDescriptions.Count(); j++)
					{
						ICollectionDescription secondCollectionDescription = this.collectionDescriptions[j];
						if (secondCollectionDescription.GetID() != firstCollectionDescription.GetID())
						{
							IHistoricalCollection firstHistoricalCollection = firstCollectionDescription.GetHistoricalCollection();
							IWorkerProperty firstWorkerProperty = firstHistoricalCollection.GetWorkerProperty();
							if (secondCollectionDescription.GetDataset() == "CCCS")
							{
								IHistoricalCollection secondHistoricalCollection = secondCollectionDescription.GetHistoricalCollection();
								IWorkerProperty secondWorkerProperty = secondHistoricalCollection.GetWorkerProperty();
								if (firstWorkerProperty.GetCode() != secondWorkerProperty.GetCode())
								{
									if (firstWorkerProperty.GetCode().ToString() == "CODE_CONSUMER")
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(firstCollectionDescription, secondCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CCCS", tuple, true);
										}
									}
									else
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(secondCollectionDescription, firstCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CCCS", tuple, true);
										}
									}
								}
							}
						}
					}
				} else if (firstCollectionDescription.GetDataset() == "CCCL")
				{
					for (int j = 0; j < this.collectionDescriptions.Count(); j++)
					{
						ICollectionDescription secondCollectionDescription = this.collectionDescriptions[j];
						if (secondCollectionDescription.GetID() != firstCollectionDescription.GetID())
						{
							IHistoricalCollection firstHistoricalCollection = firstCollectionDescription.GetHistoricalCollection();
							IWorkerProperty firstWorkerProperty = firstHistoricalCollection.GetWorkerProperty();
							if (secondCollectionDescription.GetDataset() == "CCCL")
							{
								IHistoricalCollection secondHistoricalCollection = secondCollectionDescription.GetHistoricalCollection();
								IWorkerProperty secondWorkerProperty = secondHistoricalCollection.GetWorkerProperty();
								if (firstWorkerProperty.GetCode() != secondWorkerProperty.GetCode())
								{
									if (firstWorkerProperty.GetCode().ToString() == "CODE_CUSTOM")
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(firstCollectionDescription, secondCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CCCL", tuple, true);
										}
									}
									else
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(secondCollectionDescription, firstCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CCCL", tuple, true);
										}
									}
								}
							}
						}
					}
				} else if (firstCollectionDescription.GetDataset() == "CSCM")
				{
					for (int j = 0; j < this.collectionDescriptions.Count(); j++)
					{
						ICollectionDescription secondCollectionDescription = this.collectionDescriptions[j];
						if (secondCollectionDescription.GetID() != firstCollectionDescription.GetID())
						{
							IHistoricalCollection firstHistoricalCollection = firstCollectionDescription.GetHistoricalCollection();
							IWorkerProperty firstWorkerProperty = firstHistoricalCollection.GetWorkerProperty();
							if (secondCollectionDescription.GetDataset() == "CSCM")
							{
								IHistoricalCollection secondHistoricalCollection = secondCollectionDescription.GetHistoricalCollection();
								IWorkerProperty secondWorkerProperty = secondHistoricalCollection.GetWorkerProperty();
								if (firstWorkerProperty.GetCode() != secondWorkerProperty.GetCode())
								{
									if (firstWorkerProperty.GetCode().ToString() == "CODE_SINGLENOE")
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(firstCollectionDescription, secondCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CSCM", tuple, true);
										}
									}
									else
									{
										Tuple<ICollectionDescription, ICollectionDescription> tuple = new Tuple<ICollectionDescription, ICollectionDescription>(secondCollectionDescription, firstCollectionDescription);
										this.collectionDescriptions.Remove(firstCollectionDescription);
										this.collectionDescriptions.Remove(secondCollectionDescription);
										lock (obj)
										{
											WriteToFile("CSCM", tuple, true);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		public void WriteToFile(string dataset, Tuple<ICollectionDescription, ICollectionDescription> tuple, bool deadbandNeeded)
		{

				logger.Log(String.Format("{0} Worker " + this.GetID() + " started writing to DataBase!\n", DateTime.Now));
				string file = Path.Combine(solutionPath, dataset + ".txt");
				if (deadbandNeeded)
				{
					if (Deadband(file, tuple))
					{
						using (StreamWriter streamWriter = new StreamWriter(file, true))
						{
							string firstWorkerValue = $"{tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetCode()} {tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()}";
							string secondWorkerValue = $"{tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetCode()} {tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()}";

							streamWriter.WriteLine($"{firstWorkerValue} {secondWorkerValue}");
						}
					}
					else { return; }
				}
				else
				{
					using (StreamWriter streamWriter = new StreamWriter(file, true))
					{
						string firstWorkerValue = $"{tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetCode()} {tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()}";
						string secondWorkerValue = $"{tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetCode()} {tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()}";

						streamWriter.WriteLine($"{firstWorkerValue} {secondWorkerValue}");
					}
				}
		}

		public bool Deadband(string file, Tuple<ICollectionDescription, ICollectionDescription> tuple)
		{

				string[] allText = File.ReadAllLines(file);
				foreach (string line in allText)
				{
					if (!String.IsNullOrWhiteSpace(line))
					{
						string[] arrayOfReadWorkerProperties = line.Split(' ');
						string code1 = arrayOfReadWorkerProperties[0];
						int code1Value = 0;
						Int32.TryParse(arrayOfReadWorkerProperties[1], out code1Value);
						string code2 = arrayOfReadWorkerProperties[2];
						int code2Value = 0;
						Int32.TryParse(arrayOfReadWorkerProperties[3], out code2Value);

						if (code1 == tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetCode().ToString())
						{
							if (Int32.Parse(tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) > code1Value * 0.98 &&
								Int32.Parse(tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) < code1Value * 1.02 &&
								(Int32.Parse(tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) > code2Value * 0.98 &&
								Int32.Parse(tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) < code2Value * 1.02))
							{
								return false;
							}
						}
						/*else if (code1 == tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetCode().ToString())
						{
							if (Int32.Parse(tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) > code2Value * 0.98 &&
								Int32.Parse(tuple.Item1.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) < code2Value * 1.02 &&
								(Int32.Parse(tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) > code1Value * 0.98 &&
								Int32.Parse(tuple.Item2.GetHistoricalCollection().GetWorkerProperty().GetWorkerValue()) < code1Value * 1.02))
							{
								return false;
							}
						}*/
					}
					else
					{
						return true;
					}
				}
				return true;
			
		}

		public string GetID()
		{
			return this.id;
		}
	}
}
