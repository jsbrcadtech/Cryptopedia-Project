using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptopediaWebApp.Models.ViewModels
{
    public class DetailsNetworks
    {
        //The networks itself that we want to display
        public NetworksDto SelectedNetworks { get; set; }

        //All of the related tokens to that particular species
        public IEnumerable<TokenDto> RelatedTokens { get; set; }
    }
}
