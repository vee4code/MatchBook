using Matchbook.Business;
using Matchbook.Business.Models;
using Matchbook.Db.DAL;
using Matchbook.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Matchbook.Tests.Business
{
    public class LinkOrderTests
    {
        [Fact]
        public void TestForValidateAndAddCorrectValue()
        {
            // Arrange
            var orderLinkSummary = new OrderLinkSummary()
            {
                LinkId = 1,
                StatusCode = 0,
                Message = "Successfully Linked"
            };
            List<int> orderIds = new List<int>() { 1, 2, 3 };
            string name = "Link1";

            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, ProductSymbol = "8Ad6", SubAccountId = 19 });
            orders.Add(new Order() { Id = 2, ProductSymbol = "8Ad6", SubAccountId = 19 });
            orders.Add(new Order() { Id = 3, ProductSymbol = "8Ad6", SubAccountId = 19 });

            var orderLinkDao = new Mock<IOrderLinkDao>();
            var ordersDao = new Mock<IOrdersDao>();
            var linkOrder = new LinkOrder(orderLinkDao.Object, ordersDao.Object);
            ordersDao.Setup(x => x.GetOrders(It.IsAny<List<int>>())).Returns(orders);
            orderLinkDao.Setup(x => x.CreateLink(It.IsAny<string>())).Returns((1, string.Empty));
            ordersDao.Setup(x => x.UpdateOrders(It.IsAny<List<int>>(), It.IsAny<int>())).Returns((1, string.Empty));

            //Act  
            var data = linkOrder.ValidateAndAdd(orderIds, name);

            //Assert  
            Assert.Equal(data.Message, orderLinkSummary.Message);
            Assert.Equal(data.LinkId, orderLinkSummary.LinkId);
            Assert.Equal(data.StatusCode, orderLinkSummary.StatusCode);
        }
    }
}
