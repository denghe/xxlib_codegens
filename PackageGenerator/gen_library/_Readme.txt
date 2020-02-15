利用 c# 当作 IDL 的代码生成库

*******************************************************
可用数据类型 / 关键字:
*******************************************************

struct                                   // 结构体
class                                    // 类( 可加 [Struct] 当作 struct 生成 )
enum                                     // 枚举( 可 : 指定数据类型 )

sbyte                                    // 数值类型
byte                                     // 
                                         // 
short                                    // 
ushort                                   // 
                                         // 
int                                      // 
uint                                     // 
                                         // 
long                                     // 
ulong                                    // 
                                         // 
float                                    // 
double                                   // 
                                         // 
bool                                     // 

string                                   // 文本容器( 编码后形态为 utf8 ). C++: 对应 std::string. C# & Lua: 对应 string, 逻辑不可空

byte[]                                   // 二进制容器. C++: 对应 xx::Data. C#: 对应 xx.BBuffer, 逻辑不可空. Lua: 对应 user data, 逻辑不可空

Nullable<T>, ?                           // 可空标志. 值类型. 可套在上面这些类型上, 以及 class & struct. ?写法主要针对 数值类型. C++: 对应 std::optional<>. C#: string, class 本来就可空. 其他对应 Nullable<>. Lua: 忽略( 本来就可空 )

Shared<T>                                // C++: 对应 std::shared_ptr<class>. C#, Lua: 忽略( 默认就是这种模式 )
Weak<T>                                  // C++: 对应 std::weak_ptr<class>. C#, Lua: 忽略( 通常需要在 class 中添加 disposed 等失效判定字段 以避免因生命周期延长导致的问题 )

object                                   // 可承载任意 class( xx.Object 基类 ), 用于上面 3 种引用类型的 T. 直接写 object 仅针对 C#, Lua 有效.

List<T>                                  // 变长容器, 可多层. T 可以是上述所有类型之一. C++: 对应 std::vector<T>. C#: 对应 System.Collections.Generic.List<T>. Lua: 对应 table

*******************************************************
[标记]
*******************************************************

Struct                                   // 标记一个 class 生成时视作 struct( 通常用于写继承之时以突破 c# 描述语言 本身限制 )

Desc( "注释" )                           // 标记 class | struct | field 生成指定备注
Limit(限长)                              // 标记 field, 对应 string, bbuffer, list 等容器类型, 提供 反序列化 时的长度保护, 避免申请大量内存. 多级 list 标多个
Include                                  // C++: 生成物中的相应类将生成一句 #include "rootname_namespace_class.inc" 在 .h 的类代码 的最上方
Custom                                   // 令指定 field 使用 自定义 序列化 / 反序列化 所用函数

External                                 // 标记在 enum | class | struct 上，表达其为外部引用类型，不生成任何代码. 其本身所处命名空间与实际对应

Property                                 // 标记一个field 以 "Property" 方式生成. 例如 int xxxx { get; set; } ( c# only )

CppFilter                                // 标记在 interface 上( 随便写个interface ), 以告知生成器 namespace 白名单过滤规则. 可多行( for cpp )
CSharpFilter                             // 标记在 interface 上( 随便写个interface ), 以告知生成器 namespace 白名单过滤规则. 可多行( for c# )
LuaFilter                                // 标记在 interface 上( 随便写个interface ), 以告知生成器 namespace 白名单过滤规则. 可多行( for lua )

TypeIdRange                              // 标记在 interface 上( 随便写个interface ), 以告知生成器 所使用的 type id 范围

TypeIdMappings                           // 通常标记在 "由 生成器 自行生成, 用于保存上次的 class type id 映射" 的特殊 interface 上

*******************************************************
[数据库生成相关 标记]
*******************************************************

MySql, MsSql, SQLite                     // 标记在 interface 上, 以告知生成器生成该 interface 时按指定数据库用法来处理 methods
Sql                                      // 标记在 method 上以提供原始的 sql 模板内容
Column                                   // 标记一个 field 是一个数据库字段
SkipReadOnly                             // 标记一个 field 在生成数据库填充代码时跳过某些 只读( 自增? 计算? ) 列
Literal                                  // 标记一个 parameter( string 类型 ) 非正常参数, 不转义, 直接拼接到 Sql 模板
Tuple<T1, T2, ... T7>                    // 对应一次返回多结果集查询结果


*******************************************************
补充说明
*******************************************************

C++ 生成中关于模板类写作 class, struct 的异同:
    struct 无继承, 不分配 typeId
    class 继承自 xx::Object, 分配 typeId
    class 配 [Struct] 等同于 struct ( 可实现继承 )

    class & struct 均以值类型方式使用

    class 可直接用于 收发( 发送形态不论, 接收时使用 std::shared_ptr<xx::Object> 来作为基础容器 )
    struct 不可直接用于收发

    class 可包裹 Shared<> 或 Weak<> 产生引用类型, 在一次关联的发送数据中递归引用
    struct 不可包裹 Shared<> 或 Weak<>

    class & struct 可包裹 Nullable<>, 算可空值类型

C# 生成中关于模板类写作 class, struct 的异同:
    struct 无继承, 不分配 typeId
    class 继承自 xx::Object, 分配 typeId

    class 可直接用于 收发( 接收时使用 xx.Object 或 xx.IObject 来作为基础容器 )
    struct 不可直接用于收发

    struct 可包裹 Nullable<>

Lua 各种模拟, 贴近 c#, 无 struct

针对上述差异, 如果一个模板要复用跨语言生成, 通常配 Filter<Lang> 以 namespace 为界, 过滤生成
跨语言 互通时, 需避开上述差异, 写共通类型


生成物主要采用挖洞 style, 针对定制需求, 在生成物使用期间自行定制. 具体为:
C++:
    各种 .h .cpp 前后都嵌入 各种 #include 以及覆盖检测宏
    各种 class & struct 的内部( .h 部分 ) 也嵌入 #include

C#:
    自己 partial 覆盖

Lua:
    弱类型, 自己运行时魔改

// todo: 针对上述差异, 做生成时作检查
// todo: 可开启严格模式??, 所有 float 都应该处理 NaN Infinity 问题?? 所有 buf性质 成员都应该标记 Limit ??


*******************************************************
模板文件去警告 pragma 与 using( 每个 .cs 都应以下面 2 行内容打头 )
*******************************************************

#pragma warning disable 0169, 0414
using TemplateLibrary;
