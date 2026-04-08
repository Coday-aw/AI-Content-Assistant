namespace K4U2.Exceptions;

public class ProxyApiUnavailableException : Exception
{
  public ProxyApiUnavailableException(string message, Exception?  exception) : base(message , exception)
  {
  }
}