using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    public class FulfillmentController : StorefrontControllerBase
    {
        public FulfillmentController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder) : base(workContextAccessor, urlBuilder)
        {
        }

        [HttpGet]
        public IActionResult Fulfillments()
        {
            return View("fulfillment-center", WorkContext);
        }

        [HttpGet]
        public IActionResult FulfillmentDetails(string fulfillmentId)
        {
            var result = new VirtoCommerce.Storefront.Model.Inventory.FulfillmentCenter
            {
                Id = fulfillmentId,
                Name = "Fulfillment Center # 1",
                Address = new Address
                {
                    Organization = "Org",
                    Zip = "77777",
                    PostalCode = "8888888",
                    City = "New York",
                    CountryCode = "USA",
                    CountryName = "USA",
                    Phone = "1-800-777-7777",
                    Line1 = "123 Street",
                    RegionId = "NY",
                    RegionName = "NY"
                }
            };

            WorkContext.FulfillmentCenter = result;

            return View("fulfillment-center", WorkContext);
        }

        
    }
}
