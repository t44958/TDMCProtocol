# TDMCProtocol

https://blog.csdn.net/qq_26864945/article/details/143801368?spm=1001.2014.3001.5502


This is a communication library with Mitsubishi PLC.
这是一个与三菱PLC通讯库

I searched for many communication libraries with Mitsubishi PLC on NUGET,
However, they cannot communicate well.
The content of the agreement is also very complicated, and some of it is not very user-friendly.
Useful ones still require a fee.
When I finally find one, but it seems like it will take some time to connect, there will be an error.
So, I rewrote his program and found it surprisingly useful.
Below, I will introduce the usage method.

我在NUGET 上找了很多与三菱PLC通讯库，
然而都不能很好的进行通讯。
协议内容也很繁琐，都有些不太好用。
好用的还需要收费。
当我终于找到一个，但好像会连接一段时间，就会报错。
所以，我改写他的程序，发现意外的好用。
﻿
下面我来介绍一下使用方法。

##  1.初始化连接PLC,  (PLC IP地址和端口号)。
1. Initialize the connection to the PLC (PLC IP address and port number).

```
//  ip address, port

TDMCProtocol.QCPU plc=new TDMCProtocol.QCPU("192.168.0.10", 6000);
```


## 2. 打开连接
2.open a connection

```
 plc.Open();
 
```
 
 ## 3. 读数据
 
 读一个布尔值，short 整型，字符串类型。
 3.Read a Boolean value, short integer, string type.
 
```
 
bool bl = (bool)plc.Read("M1915",VarType.Bit);
 
 short aint = (short)plc.Read("D7460", VarType.Int);
 
  string idcode = plc.Read("D8370", VarType.String, 9).ToString()

 ```
 
##  4. 写数据

 写一个布尔值，short 整型，字符串类型。
 4.Write a boolean value, short integer, string type.
 
```
    plc.Write("M1916", VarType.Bit, false );
    plc.Write("D7460", VarType.Int,(short)0);

    plc.Write("D8370", VarType.String, "12345666");
	
 ```
  

 ## 5. 关闭连接
 5.Close connection
 
 ```
 plc.Close();
 
 ```

 ## VarType 类型
 数据类型
 
 
 ```
 
public enum VarType
{
    Bit,
    Byte,
    SByte,
    Word,
    DWord,
    Int,
    DInt,
    Float,
    Double,
    String,
    Timer,
    Counter
}

 ```
 
由于没有时间，没有写完，后面会继续完善。
float,double 类型未测试。

欢迎交流与讨论。
如果对你有帮忙,请点赞评论。本通讯库长期免费。
只为帮助更多同道之人，为上位机软件更好发展，为工业智能制造，略尽微薄之力。

Due to lack of time, I did not finish writing it. I will continue to improve it later.
Float, double type not tested.
Welcome to exchange and discuss.

If it is helpful to you, please like and comment. This communication library is free for a long time.
Just to help more like-minded people, promote the better development of upper computer software, and contribute to industrial intelligent manufacturing with a modest effort.

2024.11.15

Todd 
Shang Hai DT

https://blog.csdn.net/qq_26864945/article/details/143801368?spm=1001.2014.3001.5502
