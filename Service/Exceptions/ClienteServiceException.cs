namespace DeliveryManagementAPI.Service.Exceptions;

public class ClienteServiceException : Exception
{
    public ClienteServiceException(string message) : base(message) { }
    public ClienteServiceException(string message, Exception innerException) : base(message, innerException) { }
}
