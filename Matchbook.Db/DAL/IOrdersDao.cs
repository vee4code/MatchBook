using Matchbook.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matchbook.Db.DAL
{
    public interface IOrdersDao
    {
        (int, string) UpdateOrders(List<int> orderIds, int linkId);
        List<Order> GetOrders(List<int> orderIds);
    }
}
