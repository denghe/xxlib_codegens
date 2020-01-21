#include <xx_file.h>
#include <xx_sqlite.h>
#include <string_view>

// 扫描当前工作目录( working dir ) 这一级所有 *.db3, 表 -> class, 生成 *.cs 用于模板生成

namespace FS = std::filesystem;
namespace SQ = xx::SQLite;

struct Table;
struct Column {
	int cid = 0;
	std::string name;
	std::string type;
	int notnull = 0;
	std::string dflt_value;
	int pk = 0;

	bool readOnly = false;	// 如果是主键，且建表脚本含有 AUTOINCREMENT
	bool NotNull() {
		return pk || notnull;
	}

	std::string GetCsharpDataType() {
		if (type == "TEXT") {
			if (NotNull()) return "string";
			else return "Nullable<string>";
		}
		else if (type == "INTEGER") {
			if (NotNull()) return "long";
			else return "Nullable<long>";
		}
		else if (type == "BLOB") {
			if (NotNull()) return "BBuffer";
			else return "Nullable<BBuffer>";
		}
		else if (type == "NUMERIC") {
			if (NotNull()) return "double";
			else return "Nullable<double>";
		}
		else {
			throw - 1;	// unhandled
		}
	}
};

struct Table {
	std::string name;
	std::string sql;
	std::vector<Column> columns;
};

std::vector<Table> Db3ToTables(FS::path const& db3Path) {
	std::vector<Table> tables;
	SQ::Connection conn(db3Path.string().c_str(), true);
	SQ::Query query(conn, "SELECT name, sql FROM sqlite_master where type = 'table'");
	query.Execute([&](SQ::Reader& r) {
		auto&& row = tables.emplace_back();
		r.Reads(row.name, row.sql);
		});
	query.SetQuery("select * from pragma_table_info(?)");
	for (auto& t : tables) {
		query.SetParameters(t.name);
		query.Execute([&](SQ::Reader& r) {
			auto&& row = t.columns.emplace_back();
			r.Reads(row.cid, row.name, row.type, row.notnull, row.dflt_value, row.pk);
			row.readOnly = row.pk == 1 && t.sql.find("AUTOINCREMENT") != std::string::npos;
			});
	}
	return tables;
}

void Gen(FS::path const& db3Path) {
	auto csPath = db3Path;
	csPath.replace_extension(".cs");
	auto nsName = db3Path.filename().replace_extension("").string();

	std::string sb;
	auto&& tables = Db3ToTables(db3Path);

	xx::Append(sb, R"##(#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace )##", nsName, R"##(
{)##");
	for (auto&& t : tables) {
		xx::Append(sb, R"##(
    class )##", t.name, R"##(
    {)##");
		for (auto&& c : t.columns) {
			xx::Append(sb, R"##(
		[Column)##", (c.readOnly ? "(true)" : ""), "]", c.GetCsharpDataType(), " ", c.name, ";");
		}
		xx::Append(sb, R"##(
	})##");
	}
	xx::Append(sb, R"##(
}

[SQLite]
partial interface SQLiteFuncs
{)##");
	for (auto&& t : tables) {

		std::string sb1;			// 拼接 `col1`, `col2`, `col3`, ...
		std::string sb2;			// 拼接 `col1`, `col2`, `col3`, ... 跳过只读字段
		std::string sb3;			// 拼接 {0}, {1}, {2}, ... 不算只读字段
		std::string sb4;			// 拼接 函数参数部分, 不算只读字段
		std::vector<Column> pks;	// 主键列表
		int i = 0;
		for (auto&& c : t.columns) {
			if (c.pk) {
				pks.push_back(c);
			}
			if (c.readOnly) continue;
			xx::Append(sb1, "`", c.name, "`, ");
			xx::Append(sb2, "`", c.name, "`, ");
			xx::Append(sb3, "{", i++, "}, ");
			xx::Append(sb4, c.GetCsharpDataType(), " ", c.name, ", ");
		}
		// 去最后的 ", "
		sb1.resize(sb1.size() - 2);
		if (i) {
			sb2.resize(sb2.size() - 2);
			sb3.resize(sb3.size() - 2);
			sb4.resize(sb4.size() - 2);
		}

		xx::Append(sb, R"##(
	[Desc("查 )##", t.name, R"##( 表所有数据")]
    [Sql(@"
select )##", sb1, R"##(
  from `)##", t.name, R"##(`")]
	List<)##", nsName, ".", t.name, "> ", t.name, R"##(__SelectAll();
)##");

		xx::Append(sb, R"##(
	[Desc("往 )##", t.name, R"##( 表插入单条数据")]
    [Sql(@"
insert into `)##", t.name, "` (", sb2, R"##()
values ()##", sb3, R"##()")]
	void )##", t.name, R"##(__Insert()##", sb4, R"##();
)##");

		if (pks.size()) {
			sb3.clear();		// 拼接 where 部分的条件
			sb4.clear();		// 拼接 函数参数部分
			i = 0;
			for (auto&& c : pks) {
				xx::Append(sb3, "`", c.name, "` = {", i++, "} and ");
				xx::Append(sb4, c.GetCsharpDataType(), " ", c.name, ", ");
			}
			sb3.resize(sb3.size() - 5);
			sb4.resize(sb4.size() - 2);

			xx::Append(sb, R"##(
	[Desc("根据主键查 )##", t.name, R"##( 表指定数据行")]
    [Sql(@"
select )##", sb1, R"##(
  from `)##", t.name, R"##(`
 where )##", sb3, R"##(")]
	)##", nsName, ".", t.name, " ", t.name, R"##(__SelectByPrimaryKey()##", sb4, R"##();
)##");
		}
	}
	xx::Append(sb, R"##(
}
)##");

	xx::WriteAllBytes(csPath, sb);
}

int main() {
	for (auto&& fi : FS::directory_iterator(FS::current_path())) {
		if (fi.path().extension() == ".db3") {
			Gen(fi.path());
		}
	}
	return 0;
}
