using System.Linq;

namespace Carrot.Contracts.DTOs.Common
{
    public class ServiceBaseModel
    {
        public ServiceErrorModel Error { get; set; }
        public bool IsSuccessful => !Error.Errors.Any();
        public ServiceBaseModel()
        {
            Error = new ServiceErrorModel();
        }
    }

    public class ServiceResponseModel<T> : ServiceBaseModel
    {
        public ServiceResponseModel()
        {

        }
        public ServiceResponseModel(T value)
        {
            Data = value;
            Error = new ServiceErrorModel();
        }
        public ServiceResponseModel(ServiceErrorModel error)
        {
            Error = error;
        }

        public T Data { get; set; }
    }
}
