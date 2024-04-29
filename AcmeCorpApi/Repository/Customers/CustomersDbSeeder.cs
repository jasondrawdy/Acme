using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class CustomersDbSeeder
    {
        readonly ILogger _logger;

        public CustomersDbSeeder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("CustomersDbSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var customersDb = serviceScope.ServiceProvider.GetService<CustomersDbContext>();
                if (await customersDb.Database.EnsureCreatedAsync())
                {
                    if (!await customersDb.Customers.AnyAsync()) 
                        await InsertCustomersSampleData(customersDb);
                }
            }
        }

        public async Task InsertCustomersSampleData(CustomersDbContext db)
        {
            var states = GetStates();
            db.States.AddRange(states);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception exp)
            {                
                _logger.LogError($"Error in {nameof(CustomersDbSeeder)}: " + exp.Message);
                throw; 
            }

            var customers = GetCustomers(states);
            db.Customers.AddRange(customers);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(CustomersDbSeeder)}: " + exp.Message);
                throw;
            }

        }

        private List<Customer> GetCustomers(List<State> states) {
            var customerNames = new string[]
            {
                "Sarah,Johnson,Female,live.com",
                "Michael,Adams,Male,gmail.com",
                "Emily,Clark,Female,yahoo.com",
                "David,Williams,Male,outlook.com",
                "Jessica,Lane,Female,gmail.com",
                "Brian,Harris,Male,live.com",
                "Amanda,Cooper,Female,gmail.com",
                "Eric,Morris,Male,outlook.com",
                "Samantha,Ryan,Female,yahoo.com",
                "Kevin,Porter,Male,gmail.com",
                "Laura,Carter,Female,yahoo.com",
                "Alex,Miller,Male,outlook.com",
                "Megan,Stewart,Female,live.com",
                "Paul,Barnes,Male,outlook.com",
                "Natalie,Reed,Female,yahoo.com",
                "Peter,Collins,Male,live.com",
                "Julia,Price,Female,gmail.com",
                "Ryan,Fisher,Male,yahoo.com",
                "Lisa,Young,Female,oceanic.com",
                "Alex,Gordon,Male,gmail.com",
                "Linda,Murphy,Female,live.com",
                "Greg,Evans,Male,yahoo.com",
                "Rachel,Hall,Female,gmail.com",
                "Simon,King,Male,oceanic.com"
            };
            var addresses = new string[]
            {
                "1 Atomic Street",
                "345 Pine Avenue",
                "2214 Tulip Lane",
                "754 Sunflower Circle",
                "6323 Willow Court",
                "112 Oak Way",
                "933 Cherry Street",
                "267 Maple Drive",
                "4317 Rosewood Lane",
                "5782 Aspen Road",
                "124 Birch Street",
                "8960 Cedar Avenue",
                "3321 Magnolia Boulevard",
                "2098 Juniper Lane",
                "5238 Pine Street",
                "7472 Cedar Lane",
                "1106 Birch Avenue",
                "2831 Elm Court",
                "638 Maple Avenue",
                "4619 Pinehurst Drive",
                "8799 Aspen Lane",
                "1743 Birchwood Court",
                "335 Willow Lane"
            };

            var citiesStates = new string[]
            {
                "Miami,FL,Florida",
                "San Francisco,CA,California",
                "Denver,CO,Colorado",
                "Austin,TX,Texas",
                "Boston,MA,Massachusetts",
                "Nashville,TN,Tennessee",
                "Minneapolis,MN,Minnesota",
                "Philadelphia,PA,Pennsylvania",
                "Detroit,MI,Michigan",
                "San Diego,CA,California",
                "Kansas City,MO,Missouri",
                "Charlotte,NC,North Carolina",
                "Phoenix,AZ,Arizona",
                "Indianapolis,IN,Indiana",
                "Jacksonville,FL,Florida",
                "New Orleans,LA,Louisiana",
                "Portland,ME,Maine",
                "Columbus,OH,Ohio",
                "Santa Fe,NM,New Mexico",
                "Anchorage,AK,Alaska",
                "Houston,TX,Texas",
                "Salt Lake City,UT,Utah",
                "Pittsburgh,PA,Pennsylvania",
                "Richmond,VA,Virginia"
            };

            var homePhoneNumbers = new string[]
            {
                "123-456-7890",
                "(555)-555-5555",
                "555-321-0987",
                "800-555-0199",
                "234-567-8901",
                "777-888-9999",
                "555-678-1234",
                "(555)-123-4567",
                "987-654-3210",
                "555-777-8888",
                "321-456-7890"
            };

            var mobilePhoneNumbers = new string[]
            {
                "444-555-6666",
                "(123) 456-7890",
                "800-123-4567",
                "999-888-7777",
                "555-999-8888",
                "(555) 123-4567",
                "777-888-9999",
                "234-567-8901",
                "555-666-7777",
                "111-222-3333",
                "999-888-7777",
                "(123) 555-7890",
                "222-333-4444",
                "777-999-8888",
                "555-555-1234"
            };

            var zip = 33333;
            var customers = new List<Customer>();
            var random = new Random();

            for (var i = 0; i < 10; i++) 
            {
                var nameGenderHost = customerNames[i].Split(',');
                var cityState = citiesStates[i].Split(',');
                var state = states.Where(s => s.Abbreviation == cityState[1]).SingleOrDefault();

                var customer = new Customer 
                {
                    FirstName = nameGenderHost[0],
                    LastName = nameGenderHost[1],
                    Email = nameGenderHost[0] + '.' + nameGenderHost[1] + '@' + nameGenderHost[3],
                    HomePhone = homePhoneNumbers[i],
                    MobilePhone = mobilePhoneNumbers[i],
                    Address = addresses[i],
                    City = cityState[0],
                    State = state,
                    Zip = zip + i,
                    Gender = nameGenderHost[2],
                    OrderCount = 1
                };
                customers.Add(customer);
            }

            return customers;
        }

        private List<State> GetStates() 
        {
            var states = new List<State> 
            {
                new State { Name = "Alabama", Abbreviation = "AL" },
                new State { Name = "Montana", Abbreviation = "MT" },
                new State { Name = "Alaska", Abbreviation = "AK" },
                new State { Name = "Nebraska", Abbreviation = "NE" },
                new State { Name = "Arizona", Abbreviation = "AZ" },
                new State { Name = "Nevada", Abbreviation = "NV" },
                new State { Name = "Arkansas", Abbreviation = "AR" },
                new State { Name = "New Hampshire", Abbreviation = "NH" },
                new State { Name = "California", Abbreviation = "CA" },
                new State { Name = "New Jersey", Abbreviation = "NJ" },
                new State { Name = "Colorado", Abbreviation = "CO" },
                new State { Name = "New Mexico", Abbreviation = "NM" },
                new State { Name = "Connecticut", Abbreviation = "CT" },
                new State { Name = "New York", Abbreviation = "NY" },
                new State { Name = "Delaware", Abbreviation = "DE" },
                new State { Name = "North Carolina", Abbreviation = "NC" },
                new State { Name = "Florida", Abbreviation = "FL" },
                new State { Name = "North Dakota", Abbreviation = "ND" },
                new State { Name = "Georgia", Abbreviation = "GA" },
                new State { Name = "Ohio", Abbreviation = "OH" },
                new State { Name = "Hawaii", Abbreviation = "HI" },
                new State { Name = "Oklahoma", Abbreviation = "OK" },
                new State { Name = "Idaho", Abbreviation = "ID" },
                new State { Name = "Oregon", Abbreviation = "OR" },
                new State { Name = "Illinois", Abbreviation = "IL" },
                new State { Name = "Pennsylvania", Abbreviation = "PA" },
                new State { Name = "Indiana", Abbreviation = "IN" },
                new State { Name = "Rhode Island", Abbreviation = "RI" },
                new State { Name = "Iowa", Abbreviation = "IA" },
                new State { Name = "South Carolina", Abbreviation = "SC" },
                new State { Name = "Kansas", Abbreviation = "KS" },
                new State { Name = "South Dakota", Abbreviation = "SD" },
                new State { Name = "Kentucky", Abbreviation = "KY" },
                new State { Name = "Tennessee", Abbreviation = "TN" },
                new State { Name = "Louisiana", Abbreviation = "LA" },
                new State { Name = "Texas", Abbreviation = "TX" },
                new State { Name = "Maine", Abbreviation = "ME" },
                new State { Name = "Utah", Abbreviation = "UT" },
                new State { Name = "Maryland", Abbreviation = "MD" },
                new State { Name = "Vermont", Abbreviation = "VT" },
                new State { Name = "Massachusetts", Abbreviation = "MA" },
                new State { Name = "Virginia", Abbreviation = "VA" },
                new State { Name = "Michigan", Abbreviation = "MI" },
                new State { Name = "Washington", Abbreviation = "WA" },
                new State { Name = "Minnesota", Abbreviation = "MN" },
                new State { Name = "West Virginia", Abbreviation = "WV" },
                new State { Name = "Mississippi", Abbreviation = "MS" },
                new State { Name = "Wisconsin", Abbreviation = "WI" },
                new State { Name = "Missouri", Abbreviation = "MO" },
                new State { Name = "Wyoming", Abbreviation = "WY" }
            };

            return states;
        }
    }
}