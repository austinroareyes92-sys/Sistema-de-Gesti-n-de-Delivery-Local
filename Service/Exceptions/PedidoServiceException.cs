namespace DeliveryManagementAPI.Service.Exceptions;

public class PedidoServiceException : Exception
{
    public PedidoServiceException(string message) : base(message) { }
    public PedidoServiceException(string message, Exception innerException) : base(message, innerException) { }
}
