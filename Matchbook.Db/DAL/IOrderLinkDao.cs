using System;
using System.Collections.Generic;
using System.Text;

namespace Matchbook.Db.DAL
{
    public interface IOrderLinkDao
    {
        (int, string) CreateLink(string name);
        int GetLinkCountByName(string name);
    }
}
