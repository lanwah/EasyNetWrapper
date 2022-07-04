# EasyNet.Extension（扩展库）

## 证书地址

https://choosealicense.com/licenses/mit/#  

## Package引用和项目依赖
* 1、

## 项目结构

1. ### EasyNet.Extension.Utils 中存放扩展方法的实现

| 类名                | 类说明/类函数说明                                            | 接口说明/接口参数说明                             |
| ------------------- | ------------------------------------------------------------ | ------------------------------------------------- |
| **ObjectExtn**      | **object 扩展方法**                                          |                                                   |
|                     | bool IsNull(this object obj)                                 | 判断对象是否为空                                  |
|                     | bool IsNotNull(this object obj)                              | 判断对象是否非空                                  |
|                     | void NotNullCheck(this object argumentValue, string argumentName) | 检查参数是否非空，空时抛出ArgumentNullException   |
| **StringExtn**      | **String 扩展方法**                                          |                                                   |
|                     | void NotNullOrEmptyCheck(this string argumentValue, string argumentName) | 检查参数是否空字符，空时抛出ArgumentNullException |
|                     | bool IsNullOrEmptyEx(this string value)                      | 判断字符串是否为空                                |
|                     | T ConvertFromString<T>(this string value)                    | 字符串转对象                                      |
|                     | bool IsFileExist(this string filePath)                       | 判断文件是否存在                                  |
|                     | bool IsFileUsing(this string filePath)                       | 判断文件是否被占用                                |
|                     | byte[] ReadFileBytes(this string filePath)                   | 读取文件的二进制内容                              |
| **IEnumerableExtn** | **IEnumerable 扩展方法，相关接口如下：**                     |                                                   |
|                     | IEnumerable<T> DistinctBy<T, V>(this IEnumerable<T> source, Func<T, V> keySelector) | 根据字段去重                                      |
|                     | bool HasData<T>(this IEnumerable<T> source)                  | 判断集合是否有值                                  |
| **IntPtrExtn**      | **IntPtr 扩展方法，相关接口如下：**                          |                                                   |
|                     | string ToAnsiString(this IntPtr handle, string defaultValue = "") | IntPtr 转 Ansi 字符串                             |
| **ReflectionExtn**  | **Type/Reflection 扩展方法**                                 |                                                   |
|                     | bool As(this Type type, Type baseType)                       | 判断类型是否能够转为指定基类                      |
|                     | bool IsDictionary(this Type type)                            | 判断是否为字典<br/>依赖于**As**方法               |
|                     | bool IsList(this Type type)                                  | 判断是否为List                                    |
|                     | Type[] GetParameterTypes(this MethodBase method)             | 获取参数类型                                      |
|                     | string Description(this MemberInfo member, string memberName = "") | 获取Description特性值                             |
|                     | string DisplayName(this MemberInfo member, string memberName = "") | 获取DisplayName特性值                             |
|                     | bool HasAttribute<T>(this ICustomAttributeProvider member, bool inherit = false) | 判断成员是否包含特定的特性                        |
|                     | T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = false) | 获取指定的特性                                    |
| **IOExtn**          | **IO 扩展方法**                                              |                                                   |
|                     | byte[] ToBytes(this Stream stream)                           | 读取Stream中内容到的Byte数组                      |
|                     | bool ToFile(this Stream stream, string filePath)             | 把stream写入filePath指定的文件                    |

