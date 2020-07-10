using Matchbook.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Matchbook.Db.DAL
{
    public class OrdersDao: IOrdersDao
    {        
        public (int, string) UpdateOrders(List<int> orderIds, int linkId)
        {
            int success = 0;
            string message = string.Empty;

            try
            {
                MatchbookDbContext matchDbContext = new MatchbookDbContext();
                foreach(int orderId in orderIds)
                {
                    Order order = matchDbContext.Orders.Find(orderId);
                    order.LinkId = linkId;
                    matchDbContext.Orders.Update(order);
                }

                matchDbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (success, message);
        }

        public List<Order> GetOrders(List<int> orderIds)
        {
            MatchbookDbContext dbContext = new MatchbookDbContext();
            return dbContext.Orders.Where(order => orderIds.Contains(order.Id)).ToList();
        }
    }
}
