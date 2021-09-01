using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Services.Db.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NurseHackContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}