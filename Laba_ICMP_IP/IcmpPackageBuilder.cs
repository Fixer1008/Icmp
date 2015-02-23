using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Laba_ICMP_IP
{
  public class IcmpPackageBuilder
  {
      /// <summary>
      /// Build Icmp package
      /// </summary>
      /// <returns>Icmp Package</returns>
      public IcmpPackage Build()
      {
        return new IcmpPackage
        {
          Type = 8,
          Code = 0,
          Checksum = 0,
          //Id = Process.GetCurrentProcess().Id
        };
      }

      /// <summary>
      /// Convert Icmp package to array of bytes
      /// </summary>
      /// <param name="package"></param>
      /// <returns>Array of bytes</returns>
      public byte[] PackageToByteArray(IcmpPackage package)
      {
        byte[] data = new byte[package.MessageSize + 9];
        Buffer.BlockCopy(BitConverter.GetBytes(package.Type), 0, data, 0, 1);
        Buffer.BlockCopy(BitConverter.GetBytes(package.Code), 0, data, 1, 1);
        Buffer.BlockCopy(BitConverter.GetBytes(package.Checksum), 0, data, 2, 2);
        Buffer.BlockCopy(package.Message, 0, data, 4, package.MessageSize);
        return data;
      }

      /// <summary>
      /// Convert array of bytes to Icmp package 
      /// </summary>
      /// <param name="data"></param>
      /// <param name="size"></param>
      /// <returns>Icmp Package</returns>
      public IcmpPackage ByteArrayToPackage(byte[] data, int size)
      {
        IcmpPackage package = new IcmpPackage
        {
          Type = data[20],
          Code = data[21],
          Checksum = BitConverter.ToUInt16(data, 22),
          MessageSize = size - 24
        };

        Buffer.BlockCopy(data, 24, package.Message, 0, package.MessageSize);

        return package;
      }

      public UInt16 CalculateChecksum(IcmpPackage package)
      {
          UInt32 chcksm = 0;
          byte[] data = this.PackageToByteArray(package);

          int packetsize = package.MessageSize + 8;
          int index = 0;

          while (index < packetsize)
          {
            chcksm += Convert.ToUInt32(BitConverter.ToUInt16(data, index));
            index += 2;
          }

          chcksm = (chcksm >> 16) + (chcksm & 0xffff);
          chcksm += (chcksm >> 16);

          return (UInt16)(~chcksm);
      }
  }
}
