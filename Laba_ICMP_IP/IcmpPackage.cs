using System;
using System.Text;

namespace Laba_ICMP_IP
{
  public sealed class IcmpPackage : Package
  {
      public int MessageSize;
      public byte[] Message = new byte[1024];

      public byte Type { get; set; }
      public byte Code { get; set; }
      public UInt16 Checksum { get; set; }
      public int Id { get; set; }
    }
}
