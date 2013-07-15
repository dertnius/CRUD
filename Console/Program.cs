
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccess;
using System.Data.Entity;
using System.Configuration;
using System;

namespace Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var emp = new Employee
            {

                Name = "Alejandro Rivas",
                Id = 500,
                Address = "Jana Pawla 45",
                Telephone = "657890542"

            };

            
            
            
            string connectionString = ConfigurationManager.ConnectionStrings["AcmeContext"].ConnectionString;
            DbContext cs = new AcmeContext();
            
       
     
            using (var context = new AcmeContext()) {

                context.Employees.Add(emp);
                context.SaveChanges();
                var emp2 = (from empl in context.Employees
                            select empl).ToList();

                System.Console.WriteLine(emp2);
            
            }

         
            
            

        }
    }
}
