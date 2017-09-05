using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using CoGov.Models;

namespace CoGov.DAL
{
    public class EntityObject : Startbutton.Data.EntityObject
    {
        ApplicationDbContext _db = null;

        [NotMapped]
        public new ApplicationDbContext db
        {
            get
            {
                if (_db == null)
                    _db = ApplicationDbContext.GetDbContextFromObject(this) as ApplicationDbContext;

                return _db;
            }

        }

    }

}
