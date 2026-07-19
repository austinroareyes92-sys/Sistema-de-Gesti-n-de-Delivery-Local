namespace DeliveryManagementAPI.Service.Exceptions;

public class RepartidorServiceException : Exception
{
    public RepartidorServiceException(string message) : base(message) { }
    public RepartidorServiceException(string message, Exception innerException) : base(message, innerException) { }
}
