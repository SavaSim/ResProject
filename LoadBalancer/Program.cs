using LoadBalancer.Data;
using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace LoadBalancer
{
	class Program
	{
        public static Object obj = new Object();
        public static IWriter writer = new Writer();
        public static ILogger logger = new Logger();
        private static CodeList codeList = new CodeList();
        private static IWorker worker1 = new Worker(2, "Raja", logger);
        private static IWorker worker2 = new Worker(5, "Gaja", logger);
        private static IWorker worker3 = new Worker(1, "Paja", logger);
        private static IWorker worker4 = new Worker(3, "Vlaja", logger);
        static void Main(string[] args)
		{
            Random ran = new Random();   
			ILoadBalancer loadBalancer = new LoadBalancerClass(ran.Next(0,3), logger);
            loadBalancer.AddWorker(worker1);
            loadBalancer.AddWorker(worker2);
            loadBalancer.AddWorker(worker3);
            loadBalancer.AddWorker(worker4);
            writer.SetLoadBalancer(loadBalancer);

            Thread t1 = new Thread(new ThreadStart(writer.ReadData));
            t1.Start();
            Menu();
		}

        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("1. Unesite vrednost");
                Console.WriteLine("2. Iscitaj iz baze");
                Console.WriteLine("3. Upali/Iskljuci Workera");
                Console.WriteLine("4. Exit");
                string str = Console.ReadLine();
                if (str == "1")
                {

                    lock (obj)
                    {
                        Console.WriteLine("Unesite vrednost:");
                        IItem item = new Item();
                        string data = Console.ReadLine();
                        try
                        {
                            item.SetValue(Int32.Parse(data));
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            return;
                        }


                        Console.WriteLine("Izaberi code: \n");
                        Console.WriteLine("1. CODE_ANALOG\n");
                        Console.WriteLine("2. CODE_DIGITAL\n");
                        Console.WriteLine("3. CODE_CUSTOM\n");
                        Console.WriteLine("4. CODE_LIMITSET\n");
                        Console.WriteLine("5. CODE_SINGLENOE\n");
                        Console.WriteLine("6. CODE_MULTIPLENODE\n");
                        Console.WriteLine("7. CODE_CONSUMER\n");
                        Console.WriteLine("8. CODE_SOURCE\n");
                        Console.WriteLine("Za izlaz pritisni x");
                        data = Console.ReadLine();

                        switch (data)
                        {
                            case "1":
                                item.SetCode(codeList.GetCode(0));
                                break;
                            case "2":
                                item.SetCode(codeList.GetCode(1));
                                break;
                            case "3":
                                item.SetCode(codeList.GetCode(2));
                                break;
                            case "4":
                                item.SetCode(codeList.GetCode(3));
                                break;
                            case "5":
                                item.SetCode(codeList.GetCode(4));
                                break;
                            case "6":
                                item.SetCode(codeList.GetCode(5));
                                break;
                            case "7":
                                item.SetCode(codeList.GetCode(6));
                                break;
                            case "8":
                                item.SetCode(codeList.GetCode(7));
                                break;
                            case "x":
                                return;
                            default:
                                break;
                        }
                        //Thread t2 = new Thread(() => writer.ReadDataFromStdin(item));
                        writer.ReadDataFromStdin(item);
                    }

                }
                else if (str == "2")
                {
                    lock (obj)
                    {
                        Reader reader = new Reader();
                        Console.WriteLine("Izaberi tabelu dataset: \n");
                        Console.WriteLine("1. CACD\n");
                        Console.WriteLine("2. CCCL\n");
                        Console.WriteLine("3. CCCS\n");
                        Console.WriteLine("4. CSCM\n");
                        int data = Int32.Parse(Console.ReadLine());
                        switch (data)
                        {
                            case 1:
                                reader.ReadFromDB("CACD");
                                break;
                            case 2:
                                reader.ReadFromDB("CCCL");
                                break;
                            case 3:
                                reader.ReadFromDB("CCCS");
                                break;
                            case 4:
                                reader.ReadFromDB("CSCM");
                                break;
                            default:
                                Console.WriteLine("Nema opcije!");
                                break;
                        }
                    }
                }
                else if (str == "3")
                {
                    Console.WriteLine("Izaberi workera kojeg zelis da iskljucis/ukljucis: \n");
                    Console.WriteLine("1. Worker Raja\n");
                    Console.WriteLine("2. Worker Gaja\n");
                    Console.WriteLine("3. Worker Paja\n");
                    Console.WriteLine("4. Worker Vlaja\n");
                    int data = Int32.Parse(Console.ReadLine());
                    switch (data)
                    {
                        case 1:
                            writer.ToggleWorkerWorkStatus(worker1);
                            break;
                        case 2:
                            writer.ToggleWorkerWorkStatus(worker2);
                            break;
                        case 3:
                            writer.ToggleWorkerWorkStatus(worker3);
                            break;
                        case 4:
                            writer.ToggleWorkerWorkStatus(worker4);
                            break;
                        default:
                            Console.WriteLine("Nema opcije!");
                            break;
                    }
                }
                else if (str == "4")
                {
                    lock (obj)
                    {
                        System.Environment.Exit(1);
                    }
                }
            }
        }
	}
}
