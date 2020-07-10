using Matchbook.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Matchbook.Db.DAL
{
    public class OrderLinkDao: IOrderLinkDao
    {
        public (int, string) CreateLink(string name)
        {
            int linkId = -1;
            string message = string.Empty;

            try
            {
                MatchbookDbContext dbContext = new MatchbookDbContext();
                OrderLink orderLink = new OrderLink() { Name = name };
                dbContext.OrderLink.Add(orderLink);
                dbContext.SaveChanges();
                linkId = dbContext.OrderLink.OrderByDescending(link => link.Id).FirstOrDefault().Id ?? -1;
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }
            return (linkId, message);
        }

        public (int,string) GetLinkCountByName(string name)
        {
            int count = 0;
            string message = string.Empty;

            try
            {
                MatchbookDbContext dbContext = new MatchbookDbContext();
                count = dbContext.OrderLink.Where(link => link.Name.ToLower() == name.ToLower()).Count();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (count,message);
        }
    }
}
