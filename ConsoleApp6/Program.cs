using ConsoleApp6.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace ConsoleApp6
{
    public static class Program
    {

        static List<Person> people = new List<Person>
        {
            new Person { Name = "Pavel", DateOfBirth = new DateTime(2022, 9, 1) },
            new Person { Name = "Ivan", DateOfBirth = new DateTime(2022, 9, 2) },
            new Person { Name = "Nazar", DateOfBirth = new DateTime(2022, 9, 10) },
            new Person { Name = "Nikita", DateOfBirth = new DateTime(2022, 9, 3) },
            new Person { Name = "Yura", DateOfBirth = new DateTime(2022, 9, 20) }
        };

        
        static void Main(string[] args)
        {
            clsPerson p = new clsPerson();
            p.FirstName = "Jeff";
            p.MI = "A";
            p.LastName = "Price";
            bool exist = File.Exists("..\\..\\doc.txt");
            Console.WriteLine(exist);
            using (FileStream docRead = new FileStream("..\\..\\doc.txt", FileMode.Open))
            {
                byte[] res = new byte[docRead.Length];
                docRead.Read(res, 0, res.Length);
                p.result = Convert.ToBase64String(res, 0, res.Length,
                    Base64FormattingOptions.InsertLineBreaks);
            }

            XmlSerializer x = new XmlSerializer(p.GetType());
            using (FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "test.xml"), FileMode.Create))
            {
                x.Serialize(fs, p);
            }
            
            IQueryable<Person> result = people.AsQueryable().Filter(f => f.CompareDates(f.DateOfBirth));
            var testPortal = ConfigurationManager.AppSettings.Get("testPortal");
            Console.WriteLine(Resources.ResourceManager.GetString("Pavel"));
            foreach (var item in testPortal)
            {
                Console.WriteLine(item);
            }

            Console.Read();
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    public static class Helper
    {
        /**
         <summary>Запрос</summary> 
         */
        public static IQueryable<T> Filter<T>(this IQueryable<T> seq, Func<T, bool> func)
        {
            return seq.Where(func).AsQueryable();
        }

        public static bool CompareDates<T>(this T ts, DateTime date)
        {
            return date > DateTime.Now;
        }
    }
    public class clsPerson
    {
        public string FirstName;
        public string MI;
        public string LastName;
        public string result;
    }
}
