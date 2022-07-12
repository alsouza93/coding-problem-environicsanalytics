using CodingProblem.Domain;
using CodingProblem.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodingProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new PostalCodeRepository();

            string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Customer> customers = File.ReadAllLines($@"{executingDirectory}\\CodingInterviewTestCustomerFileCSV_200.csv")
            .Skip(1)
            .Select(v => FromCsv(v))
            .ToList();


            var segmentCodeDictionary = new Dictionary<string, int>();

            var firstSegmentCodeCustumers = new List<Customer>();
            var secondSegmentCodeCustumers = new List<Customer>();
            for (int i = 0; i < customers.Count; i++)
            {
                int segmentCode = 0;
                if (!segmentCodeDictionary.TryGetValue(customers[i].CelPostalCode, out segmentCode))
                {
                    segmentCode = repository.GetSegmentCode(customers[i].CelPostalCode);
                    segmentCodeDictionary.Add(customers[i].CelPostalCode, segmentCode);
                }

                customers[i].SegmentCode = segmentCode;
                if (segmentCode >= 1 && segmentCode < 30)
                {
                    firstSegmentCodeCustumers.Add(customers[i]);
                }
                else if (segmentCode >= 31 && segmentCode < 67)
                {
                    secondSegmentCodeCustumers.Add(customers[i]);
                }
            }

            Console.WriteLine($"Summarize for the first group: {firstSegmentCodeCustumers.Sum(s => s.TotalVisits)}");
            Console.WriteLine($"Summarize for the second group: {secondSegmentCodeCustumers.Sum(s => s.TotalVisits)}");


        }

        public static Customer FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Customer customer = new Customer();
            customer.StoredId = Convert.ToInt32(values[0]);
            customer.CustomerId = values[1];
            customer.CelPostalCode = values[2];
            customer.TotalVisits = Convert.ToInt32(values[3]);
            return customer;
        }
    }
}
