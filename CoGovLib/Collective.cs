using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoGov.Models;

namespace CoGov.Library
{
    public static class Collective
    {
        public static IList<Models.Collective> GetPublicCollectives(ApplicationDbContext db)
        {
            return db.FromCache("PublicCollectives", new TimeSpan(0,10,0), db.Collectives.Where(c => c.Private == false));
        }

    }
}
