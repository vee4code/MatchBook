using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.WebPages;

namespace Matchbook.Model
{
    
    public class OrderLink
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<Order> LinkedOrders { get; set; }
    } 

}
