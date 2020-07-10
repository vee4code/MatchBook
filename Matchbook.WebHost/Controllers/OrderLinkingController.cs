using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Matchbook.Business;
using Matchbook.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Matchbook.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderLinkingController : Controller
    {
        private readonly ILinkOrder linkOrder;
        
        public OrderLinkingController(ILinkOrder _linkOrder)
        {
            linkOrder = _linkOrder;
        }

        [HttpPost]
        public OrderLinkSummary LinkOrders([FromBody]OrderLinkInput linkInput)
        {
            OrderLinkSummary orderLinkSummary = new OrderLinkSummary();

            try
            {
                //LinkOrder linkOrder = new LinkOrder();
                if(linkInput.OrderIds == null || linkInput.OrderIds.Count == 0)
                {
                    orderLinkSummary.LinkId = -1;
                    orderLinkSummary.Message = "OrderId is either null or 0";
                    orderLinkSummary.StatusCode = HttpStatusCode.OK;
                    return orderLinkSummary; 
                }                    
                orderLinkSummary = linkOrder.ValidateAndAdd(linkInput.OrderIds, linkInput.Name);
                orderLinkSummary.StatusCode = orderLinkSummary.LinkId == -1 ? HttpStatusCode.OK : HttpStatusCode.Created;
            }
            catch(Exception ex)
            {
                orderLinkSummary.StatusCode = HttpStatusCode.InternalServerError;
                orderLinkSummary.Message = ex.Message;
            }

            return orderLinkSummary;
        }        
    }
}
