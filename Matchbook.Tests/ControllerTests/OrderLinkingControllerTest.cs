using Matchbook.Business;
using Matchbook.Business.Models;
using Matchbook.WebHost.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Matchbook.Tests
{
    public class OrderLinkingControllerTest 
    {
        [Fact]
        public void TestForLinkOrdersMissingValue()
        {
            // Arrange
            var orderLinkSummary = new OrderLinkSummary()
            {
                LinkId = -1,
                StatusCode = HttpStatusCode.OK,
                Message = "OrderId is either null or 0"
            };
            OrderLinkInput orderLinkInput = new OrderLinkInput() { OrderIds = null, Name = null};
            var linkOrder = new Mock<ILinkOrder>();
            var controller = new OrderLinkingController(linkOrder.Object);
            linkOrder.Setup(x => x.ValidateAndAdd(It.IsAny<List<int>>(), It.IsAny<string>())).Returns(orderLinkSummary);

            //Act  
            var data = controller.LinkOrders(orderLinkInput);

            //Assert  
            Assert.Equal(data.Message, orderLinkSummary.Message);
            Assert.Equal(data.LinkId, orderLinkSummary.LinkId);
            Assert.Equal(data.StatusCode, orderLinkSummary.StatusCode);
        }

        [Fact]
        public void TestForLinkOrdersCorrectValue()
        {
            // Arrange
            var orderLinkSummary = new OrderLinkSummary()
            {
                LinkId = 1,
                StatusCode = HttpStatusCode.Created,
                Message = "Successfully Linked"
            };
            OrderLinkInput orderLinkInput = new OrderLinkInput() 
            { 
                OrderIds = new List<int>(){ 1, 2, 3 }, Name = "Link1" 
            };
            var linkOrder = new Mock<ILinkOrder>();
            var controller = new OrderLinkingController(linkOrder.Object);
            linkOrder.Setup(x => x.ValidateAndAdd(It.IsAny<List<int>>(), It.IsAny<string>())).Returns(orderLinkSummary);

            //Act  
            var data = controller.LinkOrders(orderLinkInput);

            //Assert  
            Assert.Equal(data.Message, orderLinkSummary.Message);
            Assert.Equal(data.LinkId, orderLinkSummary.LinkId);
            Assert.Equal(data.StatusCode, orderLinkSummary.StatusCode);
        }
    }
}
