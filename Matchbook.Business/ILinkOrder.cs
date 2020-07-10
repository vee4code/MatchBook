using Matchbook.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matchbook.Business
{
    public interface ILinkOrder
    {
        OrderLinkSummary ValidateAndAdd(List<int> orderIds, string name);
    }
}
