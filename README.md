系统依赖参考 xxlib_cpp 项目

工具列表：
Db3ToCSharpTemplate
	扫描 sqlite db3 类结构，生成 class { columns.... } 这样的类代码 用于进一步加工使用

PackageGenerator
	可将以 c# 语法书写的 结构描述文本 转为目标语言代码生成物, 可用于网络序列化收发，数据库查询等
	
生成用批处理参考: gen_test_cpp_pkg.bat
测试项目：test_cpp*




听说: gcc 编译选项可以加上 -fdata-sections -ffunction-sections，链接选项加上 -Wl,--gc-sections 可以给程序减肥?