using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WellService.Models;

namespace WellService.Helpers
{
    public class DbTool
    {
        public static void InitDB()
        {
           
            if (!File.Exists(Constants.DBFileName))
            {
                using (var dbContext = new WellDbContext())
                {
                    //Ensure database is created
                    dbContext.Database.EnsureCreated();
                    if (!dbContext.ClientQuotas.Any())
                    {
                        dbContext.ClientQuotas.AddRange(new Models.ClientQuota[]
                            {
                             new ClientQuota{ ClientName="Client 1", Email="client1@gmail.com", Quota=2 },
                             new ClientQuota{ ClientName="Client 2", Email="client2@gmail.com", Quota=5}
                            });
                        dbContext.SaveChanges();
                        foreach (var item in dbContext.ClientQuotas)
                        {
                            Console.WriteLine($"ID={item.ClientID}\tName={item.ClientName}");
                        }
                    }


                   
                }
            }
          
        }
    }
}
