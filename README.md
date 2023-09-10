# YaskawaRobot
Yaskawa High Speed Ethernet Server Wrapper for .NET

[Main Documentation](https://www.motoman.com/getmedia/38CD89D5-C90D-4C5A-8628-0551C44C9A6C/178942-1CD.pdf.aspx?ext=.pdf)

## Controller Support
* YRC1000 (Tested)
* DX200   (UnTest)
* FS100   (UnTest)

## Platform Tested
* C# (require .Net Framework 4.7.2)
* LabVIEW

## Methods
High Speed Ethernet Server have 47 Robot Commands.<br />
Now, 22 functions (Ch01-16、19-24) is implemented, others still working...<br />

## Usage
```C#
using YRCC;

class Program
{
    static void Main()
    {
        YHSES yrc1000 = new YHSES("192.168.255.1");
        
        SystemInfo systemInfo = new SystemInfo();
        rt = yrc1000.ReadSystemInfoData(11, ref systemInfo, out err);
        Console.WriteLine(systemInfo);
    }
}
```

## Build Environment
* VS2019
* .Net Framework 4.7.2
* System.Net.Http (Socket)

## Ref
https://github.com/hsinkoyu/fs100
