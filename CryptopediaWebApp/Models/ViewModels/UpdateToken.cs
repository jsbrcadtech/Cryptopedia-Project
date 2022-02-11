using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptopediaWebApp.Models.ViewModels
{
    public class UpdateToken
    {
        //This viewmodel is a class which stores information that we need to present to /Token/Update/{}

        //the existing token information

        public TokenDto SelectedToken { get; set; }

        // all networks to choose from when updating this token

        public IEnumerable<NetworksDto> NetworksOptions { get; set; }
    }
}