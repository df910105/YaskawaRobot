# YaskawaRobot
Yaskawa High Speed Ethernet Server Wrapper for .NET


## Controller Support
* YRC1000 (Tested)
* DX200   (UnTest)
* FS100   (UnTest)

## Platform Support (Require .Net Framework 4.7.2)
* C# 
* LabVIEW

## Methods
Complete 43/47 Robot control commands. ![91](https://progress-bar.dev/91)<br />

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

## Ref
* https://github.com/hsinkoyu/fs100
* [Documentation](https://www.motoman.com/getmedia/38CD89D5-C90D-4C5A-8628-0551C44C9A6C/178942-1CD.pdf.aspx?ext=.pdf)
