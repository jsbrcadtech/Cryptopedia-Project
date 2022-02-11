using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CryptopediaWebApp.Models
{
    public class Token
    {
        [Key]
        public int TokenID { get; set; }
        public string TokenName { get; set; }
        public int TokenCreationYear { get; set; }
        public string TokenDescription { get; set; }

        //A token belongs to one networks
        //A networks can have many tokens
        [ForeignKey("Networks")]
        public int NetworksID { get; set; }

        public virtual Networks Networks { get; set; }
    }
    public class TokenDto
    {
        public int TokenID { get; set; }
        public string TokenName { get; set; }
        public int TokenCreationYear { get; set; }
        public string TokenDescription { get; set; }
        public int NetworksID { get; set; }
        public string NetworksName { get; set; }
        public string NetworksStandard { get; set; }




    }
}