using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Services.Db
{
    public class DbService
    {

        /*
            Get all users of the domain including members and guests
        */
      

        public async Task<string> GetTeamSolutions()
        {
            //    string urlExt = "v1.0/teams/" + teamID + "/channels";
            string json = ""; // await RunAsync(urlExt, HttpMethodType.Get, null);
            return json;
        }

        public async Task<string> GetSolutionMembers(string solutionID)
        {
            //    string urlExt = "v1.0/teams/" + teamID + "/channels";
            string json = ""; // await RunAsync(urlExt, HttpMethodType.Get, null);
            return json;
        }
    }
}
