using System.Collections.Generic;

namespace Carrot.Contracts.DTOs.Common
{
    public class ServiceErrorModel
    {
        public List<string> Errors { get; set; }
        public int ErrorCode { get; set; }

        public ServiceErrorModel()
        {
            Errors = new List<string>();
        }
    }
}
