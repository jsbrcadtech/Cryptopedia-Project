using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CryptopediaWebApp.Models
{
    public class Networks
    {
        [Key]
        public int NetworksID { get; set; }
        public string NetworksName { get; set; }
        public string NetworksStandard { get; set; }
    }

    public class NetworksDto
    {
        public int NetworksID { get; set; }
        public string NetworksName { get; set; }
        public string NetworksStandard { get; set; }

    }
}