# EasyNetWrapper

## 证书地址
https://choosealicense.com/licenses/mit/#  

## Package引用和项目依赖
* 1、log4net 2.0.7


## 项目结构
-- **EasyNet.Core** 项目
   |-- log4net.config

------

-- **EasyNet.Extension** 项目
	|-- Utils 文件夹
		|-- DictionaryExts.cs
		|-- IEnumerableExts.cs
		|-- IntPtrExts.cs
		|-- ObjectExts.cs
		|-- ReflectionExts.cs
		|-- StreamExts.cs
		|-- ObjectExtn.cs
		|-- StringExts.cs
		|-- StringRegexExts.cs

------

-- **EasyNet.Win32** 项目
	|-- User32.cs
	|-- Kernel32.cs
	|-- Kernel32.Wrapper.cs
	|-- Structs 文件夹
		|-- Input.cs
		|-- MouseEventFlag.cs

## 修改日志
### 1、2022-07-02 lanwah  
- 1、新增了**EasyNet.Extension**项目，并添加了**Utils**文件夹，添加了如下文件：
  IEnumerableExtn.cs
  IntPtrExtn.cs
  ObjectExtn.cs
  ReflectionExtn.cs
  StringExtn.cs
- 2、添加了**Helpers**文件夹，添加了如下文件：
  ValueConverter.cs
### 2、2022-07-04 lanwah
1、更改 **ReflectionExtn** 类中 **Description** 和 **DisplayName** 扩展方法的实现。

### 3、2022-07-05 lanwah
1、添加了 EasyNet.Win32 项目