using Matchbook.Business.Models;
using Matchbook.Db.DAL;
using Matchbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matchbook.Business
{
    public class LinkOrder: ILinkOrder
    {
        private readonly IOrderLinkDao orderLinkDao;
        private readonly IOrdersDao ordersDao;
        public LinkOrder()
        {
        }
        public LinkOrder(IOrderLinkDao _orderLinkDao, IOrdersDao _ordersDao)
        {
            orderLinkDao = _orderLinkDao;
            ordersDao = _ordersDao;
        }
        public OrderLinkSummary ValidateAndAdd(List<int> orderIds, string name)
        {
            OrderLinkSummary orderLinkSummary = new OrderLinkSummary();
            orderLinkSummary.LinkId = -1;
            try
            {
                List<Order> orders = new List<Order>();
                orders = ordersDao.GetOrders(orderIds).ToList();
                int count = orders.GroupBy(group => new { group.ProductSymbol, group.SubAccountId }).Count();
                if(count == 1)
                {
                    int existingLinkIdCount = orders.Where(order => !string.IsNullOrEmpty(order.LinkId.ToString())).Count();
                    if (existingLinkIdCount > 0)
                    {
                        orderLinkSummary.Message = "Link already exists";                       
                    }
                    else
                    {                       
                        (orderLinkSummary.LinkId, orderLinkSummary.Message) = orderLinkDao.CreateLink(name);
                        if (orderLinkSummary.LinkId != -1)
                            ordersDao.UpdateOrders(orderIds, orderLinkSummary.LinkId);
                        orderLinkSummary.Message = "Successfully Linked";
                    }                    
                }
                else
                {
                    orderLinkSummary.Message = "ProductSymbol and SubAccount do not match.";
                }
    
            }
            catch(Exception ex)
            {
                orderLinkSummary.Message = ex.Message;
            }

            return orderLinkSummary;

        }

        public (bool,string) IsUnique(string name)
        {
            OrderLinkDao orderLinkDao = new OrderLinkDao();
            bool isUnique = false;
            string message;
            int count;
            (count, message) = orderLinkDao.GetLinkCountByName(name);
            if (count == 0 && string.IsNullOrEmpty(message))
                isUnique = true;
            return (isUnique,message);
        }
    }
}
