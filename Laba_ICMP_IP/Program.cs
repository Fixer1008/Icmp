using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Permissions;
using System.Diagnostics;

namespace Laba_ICMP_IP
{
  class Program
  {
    public static void Main(string[] argv)
    {
      var remoteIPs = Dns.GetHostAddresses("www.google.com");
      IPEndPoint ipEndPoint = new IPEndPoint(remoteIPs[0], 0);
      EndPoint endPoint = (EndPoint)ipEndPoint;

      IcmpPackageBuilder icmpBuilder = new IcmpPackageBuilder();
      IcmpPackage icmpPackage = icmpBuilder.Build();  

      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
      socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);

      icmpPackage.Checksum = icmpBuilder.CalculateChecksum(icmpPackage);

      socket.SendTo(icmpBuilder.PackageToByteArray(icmpPackage), ipEndPoint);

      int recv;
      byte[] data = new byte[1024];

      try
      {
        recv = socket.ReceiveFrom(data, ref endPoint);
      }
      catch (SocketException e)
      {
        Console.WriteLine(e.Message);
        Console.ReadLine();
        return;
      }

      Console.WriteLine("Response from: {0}", endPoint.ToString());
      Console.WriteLine("  Type {0}", responsePackage.Type);
      Console.WriteLine("  Code: {0}", responsePackage.Code);

      int Sequence = BitConverter.ToInt16(responsePackage.Message, 2);

      Console.WriteLine("  Identifier: {0}", responsePackage.Id);
      Console.WriteLine("  Sequence: {0}", Sequence);

      string stringData = Encoding.ASCII.GetString(responsePackage.Message, 4, responsePackage.MessageSize - 4);
      Console.WriteLine("  Data: {0}", stringData);
      Console.ReadLine();

      socket.Close();
    }
  }
}
