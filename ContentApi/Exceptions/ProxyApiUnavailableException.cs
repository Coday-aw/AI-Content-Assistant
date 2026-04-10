namespace K4U2.Exceptions;

public class ProxyApiUnavailableException : Exception
{
  public ProxyApiUnavailableException(string message, Exception  inner) : base(message , inner)
  {
  }

  public ProxyApiUnavailableException(string message) : base(message)
  {
  }
}