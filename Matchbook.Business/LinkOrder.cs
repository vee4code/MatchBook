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
                    List<int> noExistingLinkId = new List<int>();
                    noExistingLinkId = orders.Where(order => string.IsNullOrEmpty(order.LinkId.ToString())).Select(o => o.Id).ToList();
                    if (noExistingLinkId.Count() != orderIds.Count)
                    {
                        orderLinkSummary.LinkId = orders.Where(order => !string.IsNullOrEmpty(order.LinkId.ToString())).FirstOrDefault().LinkId ?? -1;
                        if(orderLinkSummary.LinkId != -1)
                        {
                            ordersDao.UpdateOrders(noExistingLinkId, orderLinkSummary.LinkId);
                        }                        
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

        public bool IsUnique(string name)
        {
            OrderLinkDao orderLinkDao = new OrderLinkDao();
            if (orderLinkDao.GetLinkCountByName(name) > 0)
                return false;
            return true;
        }
    }
}
