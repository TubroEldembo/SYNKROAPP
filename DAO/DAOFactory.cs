using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.DAO
{
    public class DAOFactory
    {
        public static IDAO CreateDAO(string token, string projectName)
        {
            IDAO dao;
            dao = new DAOImpl(token, projectName);
            return dao;
        }

    }
}
