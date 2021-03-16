﻿
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace HackAPIs.Services.Teams
{
    /// <summary>
    /// Description of the configuration of an AzureAD public client application (desktop/mobile application). This should
    /// match the application registration done in the Azure portal
    /// </summary>
    public class AuthenticationConfig
    {
        /// <summary>
        /// instance of Azure AD, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
       
        /// <summary>
        /// Graph API endpoint, could be public Azure (default) or a Sovereign cloud (US government, etc ...)
        /// </summary>
        public string ApiUrl { get; set; } = "https://graph.microsoft.com/";

        /// <summary>
        /// The Tenant is:
        /// - either the tenant ID of the Azure AD tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
//              public string Tenant { get; set; } = "fc18e0b1-ee70-49bb-907b-809b1d8630c1"; // personal
        public string Tenant { get; set; } = "e773e193-89d3-44d9-ae4e-17766699f674";

        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
   //        public string ClientId { get; set; } = "d23fec01-f3c5-4890-99d1-81ad0a4aa87b"; // personal
        public string ClientId { get; set; } = "80b8fd49-0fff-4655-97c2-74dbe14a0c75";

        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            }
        }

        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by the CertificateName property belows)
        /// <remarks> 
 //             public string ClientSecret { get; set; } = "bs0V~TxLNL9ow-c6h.rj1j.wzkh31krwk6"; //personal
        public string ClientSecret { get; set; } = "MW_Gq-N-j8L1F8nKZ~LC6XtZli0sb_M_4_";

        /// <summary>
        /// Name of a certificate in the user certificate store
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: the property above)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by this CertificateName property)
        /// <remarks> 
        public string CertificateName { get; set; }

        /// <summary>
        /// Reads the configuration from a json file
        /// </summary>
        /// <param name="path">Path to the configuration json file</param>
        /// <returns>AuthenticationConfig read from the json file</returns>
        public static AuthenticationConfig ReadFromJsonFile(string path)
        {
            IConfigurationRoot Configuration;

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path);

            Configuration = builder.Build();
            return Configuration.Get<AuthenticationConfig>();
        }
    }



}

