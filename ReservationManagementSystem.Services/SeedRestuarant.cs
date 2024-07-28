using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Infrastructure
{
    public static class SeedRestuarant
    {
        public static WebApplication RestuarantSeed(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                try
                {
                    context.Database.EnsureCreated();

                    var seed = context.Restuarants.FirstOrDefault();
                    if(seed is null)
                    {
                        context.Restuarants.AddRange(
                            new Restuarant
                            {
                                Name = "Bistro Delight",
                                Description = "A cozy bistro offering a mix of local and international cuisines.",
                                PhoneNumber = "08012345678",
                                Email = "contact@bistrodelight.com",
                                Address = "123, Main Street, Lagos",
                                OpenTime = new TimeOnly(8, 0),
                                CloseTime = new TimeOnly(22, 0),
                                MinimumSpend = 25000,
                                DateCreated = DateTime.Now
                            },
                            new Restuarant
                            {
                                Name = "Ocean's Bounty",
                                Description = "Fresh seafood and spectacular ocean views.",
                                PhoneNumber = "08098765432",
                                Email = "info@oceansbounty.com",
                                Address = "456, Beach Road, Lagos",
                                OpenTime = new TimeOnly(10, 0),
                                CloseTime = new TimeOnly(23, 0),
                                MinimumSpend = 35000,
                                DateCreated = DateTime.Now.AddMinutes(10)
                            },
                            new Restuarant
                            {
                                Name = "The Garden Terrace",
                                Description = "Experience dining in a lush garden setting.",
                                PhoneNumber = "08023456789",
                                Email = "hello@gardenterrace.com",
                                Address = "789, Botanical Avenue, Abuja",
                                OpenTime = new TimeOnly(9, 0),
                                CloseTime = new TimeOnly(21, 0),
                                MinimumSpend = 50000,
                                DateCreated = DateTime.Now.AddMinutes(50)
                            },
                            new Restuarant
                            {
                                Name = "Mountain Top Cafe",
                                Description = "A serene cafe with a breathtaking view of the mountains.",
                                PhoneNumber = "08034567890",
                                Email = "support@mountaintopcafe.com",
                                Address = "321, Hilltop Drive, Jos",
                                OpenTime = new TimeOnly(7, 0),
                                CloseTime = new TimeOnly(20, 0),
                                MinimumSpend = 30000,
                                DateCreated = DateTime.Now.AddMinutes(100)
                            },
                            new Restuarant
                            {
                                Name = "Urban Eats",
                                Description = "Modern eatery with a variety of urban-inspired dishes.",
                                PhoneNumber = "08045678901",
                                Email = "contact@urbaneats.com",
                                Address = "654, City Boulevard, Lagos",
                                OpenTime = new TimeOnly(11, 0),
                                CloseTime = new TimeOnly(23, 59),
                                MinimumSpend = 40000,
                                DateCreated = DateTime.Now.AddMinutes(300)
                            },
                            new Restuarant
                            {
                                Name = "Savannah Grill",
                                Description = "Delicious grilled meats and a vibrant atmosphere.",
                                PhoneNumber = "08056789012",
                                Email = "info@savannahgrill.com",
                                Address = "987, Savannah Street, Abuja",
                                OpenTime = new TimeOnly(12, 0),
                                CloseTime = new TimeOnly(22, 0),
                                MinimumSpend = 45000,
                                DateCreated = DateTime.Now.AddMinutes(700)
                            },
                            new Restuarant
                            {
                                Name = "Lakeside Bistro",
                                Description = "Enjoy meals by the lake with a stunning view.",
                                PhoneNumber = "08067890123",
                                Email = "hello@lakesidebistro.com",
                                Address = "147, Lakeside Road, Kaduna",
                                OpenTime = new TimeOnly(8, 0),
                                CloseTime = new TimeOnly(21, 0),
                                MinimumSpend = 60000,
                                DateCreated = DateTime.Now.AddMinutes(1000)
                            },
                            new Restuarant
                            {
                                Name = "Cultural Flavors",
                                Description = "Authentic local dishes with a cultural twist.",
                                PhoneNumber = "08078901234",
                                Email = "support@culturalflavors.com",
                                Address = "369, Heritage Lane, Enugu",
                                OpenTime = new TimeOnly(9, 0),
                                CloseTime = new TimeOnly(22, 0),
                                MinimumSpend = 50000,
                                DateCreated = DateTime.Now.AddMinutes(2000)
                            },
                            new Restuarant
                            {
                                Name = "Highland Delights",
                                Description = "Exclusive dining experience with a view of the highlands.",
                                PhoneNumber = "08089012345",
                                Email = "contact@highlanddelights.com",
                                Address = "852, Highland Avenue, Jos",
                                OpenTime = new TimeOnly(10, 0),
                                CloseTime = new TimeOnly(22, 0),
                                MinimumSpend = 70000,
                                DateCreated = DateTime.Now.AddMinutes(3500)
                            },
                            new Restuarant
                            {
                                Name = "Sunset Paradise",
                                Description = "Romantic dining with spectacular sunset views.",
                                PhoneNumber = "08090123456",
                                Email = "info@sunsetparadise.com",
                                Address = "963, Sunset Boulevard, Port Harcourt",
                                OpenTime = new TimeOnly(17, 0),
                                CloseTime = new TimeOnly(23, 59),
                                MinimumSpend = 80000,
                                DateCreated = DateTime.Now.AddMinutes(5000)
                            }
                            );

                        context.SaveChanges();
                    }
                }
                catch( Exception ex )
                {
                    throw;
                }

                return app;
            }
        }
    }
}
