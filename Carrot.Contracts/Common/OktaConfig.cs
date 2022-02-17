using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Contracts.Common
{
    public class OktaConfig
    {
        public string ClientId { get; set; }
        public string APIKey { get; set; }
        public string RootUrl { get; set; }
    }
}
