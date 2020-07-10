using Matchbook.Db;
using Matchbook.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchbook.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly MatchbookDbContext dbContext;

        public OrdersController(MatchbookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderSummary>>> Get()
        {
            return await dbContext.Orders
                .Select(o => new OrderSummary
                {
                    Id = o.Id,
                    ProductSymbol = o.ProductSymbol,
                    Price = o.Price,
                    Currency = o.Product.Specification.PriceQuoteCurrency,
                    Quantity = o.Quantity,
                    UnitOfMeasure = o.Product.Specification.ContractUoM,
                    SubAccount = o.SubAccountId
                })
                .ToListAsync();
        }
    }
}
