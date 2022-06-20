# EasyNetWrapper
# 证书地址
https://choosealicense.com/licenses/mit/#  

# Package引用和项目依赖
* 1、log4net 2.0.7


# 项目结构
-- Configs
  |-- log4net.config
-- Controls
  |-- AutoCompleteTextBox
     |-- AutoCompleteIMEControl 控件，说明详见AutoCompleteIMEControl.txt
  |-- Graphics
     |-- AntiAliasGraphics.cs - 抗锯齿画刷
  |-- CloseLabel - 可关闭标签
-- Extension
  |-- ExtensionUnity.cs - 扩展方法添加类
-- Reflection
  |-- DynamicMethodFactory.cs - 反射相关扩展方法
-- Units
  |-- Guard.cs
-- LogProvider.cs
-- ServiceSingleton.cs

# 2020-11-18 lanwah  
* 1、添加了 ServiceSingleton 服务提供程序管理类；    
* 2、添加了 LogProvider 默认使用 log4net的日志功能； 
* 3、添加了 Guard 参数检查类； 
* 4、添加了 AutoCompleteIMEControl 控件（选择文本框，根据输入进行检索，从检索结果中进行选择）；
* 5、添加了 CloseLabel 可关闭标签
* 6、添加了 

