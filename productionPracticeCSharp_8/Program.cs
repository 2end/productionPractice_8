using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace productionPracticeCSharp_8
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region Initialisation
                string[] lines = File.ReadAllLines("../../file.txt"); 
                DbTour db = new DbTour();
                db.ParseLines(lines);
                OnAction("Tours: ");
                Console.WriteLine(db);

                db.ReadFromConsole();
                OnAction("Tours: ");
                Console.WriteLine(db);
                #endregion

                #region Menu
                string key;
                loop:
                OnAction("Menu: ");
                OnAction("Keys: 'Update', 'Remove', 'Find'");
                Console.Write("Key: ");
                key = Console.ReadLine().ToLower();
                switch (key)
                {
                    case "update":
                        {
                            OnAction("Tour's Name is a PRIMARY KEY!");
                            Tour tour = new Tour();
                            tour.ReadFromConsole();
                            db.Update(tour);
                            OnAction("Tours: ");
                            Console.WriteLine(db);
                            goto loop;
                        }

                    case "remove":
                        {
                            OnAction("Tour's Name is a PRIMARY KEY!");
                            Console.Write("Name: ");
                            string name = Console.ReadLine();
                            db.Remove(name);
                            OnAction("Tours: ");
                            Console.WriteLine(db);
                            goto loop;
                        }

                    case "find":
                        {
                            string findKey;
                            OnAction("Find by: 'ClientSurname', 'Name', 'Date' in format(dd.mm.yyyy) ");
                            Console.Write("Key: ");
                            findKey = Console.ReadLine().ToLower();
                            switch (findKey)
                            {
                                case "clientsurname":
                                    Console.Write("ClientSurname: ");
                                    string clientSurname = Console.ReadLine();
                                    OnAction("Tours: ");
                                    foreach (Tour tour in db.FindByClientSurname(clientSurname))
                                    {
                                        Console.WriteLine(tour);
                                    }
                                    goto loop;

                                case "name":
                                    Console.Write("Name: ");
                                    string name = Console.ReadLine();
                                    OnAction("Tour: ");
                                    Console.WriteLine(db.FindByTourName(name));
                                    goto loop;

                                case "date":
                                    Console.Write("Date: ");
                                    DateTime date = Convert.ToDateTime(Console.ReadLine());
                                    OnAction("Tours: ");
                                    foreach (Tour tour in db.FindByDate(date))
                                    {
                                        Console.WriteLine(tour);
                                    }
                                    goto loop;

                                default:
                                    break;
                            }
                            goto loop;
                        }

                    default:
                        break;
                }
                #endregion 

                Console.ReadLine();
            }
            catch (Exception e)
            {
                OnError(e.Message);                
            }
            
        }

        private static void OnAction(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void OnError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    class Tour
    {
        #region Fileds
        public string Name { get; set; }
        public string ClientSurname { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public double DayPrice { get; set; }
        public uint CountDay { get; set; }
        public uint CountPerson { get; set; }
        public double JourneyPrice { get; set; }
        #endregion

        #region Static
        private static void OnError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion

        #region Initialization
        public bool ParseLine(string line)
        {
            try
            {
                string[] fields = line.Split(' ');
                Name = fields[0];
                ClientSurname = fields[1];
                Date = Convert.ToDateTime(fields[2]);
                DayPrice = Convert.ToDouble(fields[3]);
                CountDay = Convert.ToUInt32(fields[4]);
                CountPerson = Convert.ToUInt32(fields[5]);
                JourneyPrice = Convert.ToDouble(fields[6]);

                return true;
            }
            catch (Exception e)
            {
                OnError(e.Message);

                return false;
            }
            
        }

        public bool ReadFromConsole()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tour:");
                Console.ResetColor();
                Console.Write("Name: ");
                Name = Console.ReadLine();
                Console.Write("ClientSurname: ");
                ClientSurname = Console.ReadLine();
                Console.Write("Date(dd.mm.yyyy): ");
                Date = Convert.ToDateTime(Console.ReadLine());
                Console.Write("DayPrice: ");
                DayPrice = Convert.ToDouble(Console.ReadLine());
                Console.Write("CountDay: ");
                CountDay = Convert.ToUInt32(Console.ReadLine());
                Console.Write("CountPerson: ");
                CountPerson = Convert.ToUInt32(Console.ReadLine());
                Console.Write("JourneyPrice: ");
                JourneyPrice = Convert.ToDouble(Console.ReadLine());

                return true;
            }
            catch (Exception e)
            {
                OnError(e.Message);

                return false;
            }
                       
        }
        #endregion

        public override string ToString()
        {
            return $"TourName: {Name}, ClientSurname: {ClientSurname}, Date: {Date}, DayPrice: {DayPrice}, " +
                   $"CountDay: {CountDay},  CountPerson: {CountPerson}, JourneyPrice: {JourneyPrice}";
        }
    }

    class DbTour
    {
        #region Fileds
        public List<Tour> Tours { get; protected set; } = new List<Tour>();
        #endregion

        #region Static
        private static void OnError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion

        #region Initialization
        public void CheckPrimaryKey(string key)
        {
            if (Tours.FirstOrDefault(t => t.Name == key) != null)
            {
                throw new Exception("Name is a PRIMARY KEY: tour with this name is already exist!");
            }
        }

        public void ParseLines(string[] lines)
        {
            foreach (string line in lines)
            {
                Tour tour = new Tour();
                if (tour.ParseLine(line))
                {
                    CheckPrimaryKey(tour.Name);
                    Tours.Add(tour);
                }                
            }
        }

        public void ReadFromConsole()
        {
            try
            {
                string key = "yes";
                while (key == "yes")
                {
                    Tour tour = new Tour();
                    if (tour.ReadFromConsole())
                    {
                        CheckPrimaryKey(tour.Name);
                        Tours.Add(tour);
                    }                
                    Console.Write("To add another tour input 'yes': ");
                    key = Console.ReadLine().ToLower();
                }
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
            
        }
        #endregion

        #region WorkWithData
        public void Update(Tour tour)
        {
            for (int i = 0; i < Tours.Count; i++)
            {
                if (Tours[i].Name == tour.Name)
                {
                    Tours[i] = tour;
                    break;
                }
            }
        }

        public void Remove(string name) => Tours.Remove(Tours.FirstOrDefault(t => t.Name == name));

        public List<Tour> FindByClientSurname(string clientSurname) => Tours.Where(t => t.ClientSurname == clientSurname).ToList();
        public Tour FindByTourName(string TourName) => Tours.FirstOrDefault(t => t.Name == TourName);
        public List<Tour> FindByDate(DateTime date) => Tours.Where(t => t.Date == date).ToList();
        #endregion

        public override string ToString()
        {
            string result = "";
            foreach (Tour tour in Tours)
            {
                result += tour.ToString() + "\n";
            }
            return result;
        }
    }
}
