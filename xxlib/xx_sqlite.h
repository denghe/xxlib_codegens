#pragma once
#include "xx_data.h"
#include "sqlite3.h"

// todo: 补 const, throw, 进一步明确并规范 try 的用法, 增强 query() 的结果填充输出能力

namespace xx {
	namespace SQLite {
		struct Connection;
		struct Query;
		struct Reader;
		struct Null {};

		// 保持与 SQLite 的宏一致
		enum class DataTypes : uint8_t {
			Integer = 1,	// int, int64_t
			Float = 2,		// double
			Text = 3,		// char*, int len
			Blob = 4,		// uint8_t*, int len
			Null = 5		// nil
		};

		// 数据写盘模式
		enum class SynchronousTypes : uint8_t {
			Full,			// 等完全写入磁盘
			Normal,			// 阶段性等写磁盘
			Off,			// 完全不等
		};
		static const char* const strSynchronousTypes[] = {
			"FULL", "NORMAL", "OFF"
		};

		// 事务数据记录模式
		enum class JournalModes : uint8_t {
			Delete,			// 终止事务时删掉 journal 文件
			Truncate,		// 不删, 字节清 0 ( 可能比删快 )
			Persist,		// 在文件头打标记( 可能比字节清 0 快 )
			Memory,			// 内存模式( 可能丢数据 )
			WAL,			// write-ahead 模式( 似乎比上面都快, 不会丢数据 )
			Off,			// 无事务支持( 最快 )
		};
		static const char* const strJournalModes[] = {
			"DELETE", "TRUNCATE", "PERSIST", "MEMORY", "WAL", "OFF"
		};

		// 临时表处理模式
		enum class TempStoreTypes : uint8_t {
			Default,		// 默认( 视 C TEMP_STORE 宏而定 )
			File,			// 在文件中建临时表
			Memory,			// 在内存中建临时表
		};
		static const char* const strTempStoreTypes[] = {
			"DEFAULT", "FILE", "MEMORY"
		};

		// 排它锁持有方式，仅当关闭数据库连接，或者将锁模式改回为NORMAL时，再次访问数据库文件（读或写）才会放掉
		enum class LockingModes : uint8_t {
			Normal,			// 数据库连接在每一个读或写事务终点的时候放掉文件锁
			Exclusive,		// 连接永远不会释放文件锁. 第一次执行读操作时，会获取并持有共享锁，第一次写，会获取并持有排它锁
		};
		static const char* const strLockingModes[] = {
			"NORMAL", "EXCLUSIVE"
		};


		// 用于判断合法的字段类型区间
		template<typename T>
		struct IsValidDataType : std::false_type {};

		template<> struct IsValidDataType<int> : std::true_type {};
		template<> struct IsValidDataType<int64_t> : std::true_type {};
		template<> struct IsValidDataType<double> : std::true_type {};
		template<> struct IsValidDataType<std::string> : std::true_type {};
		template<> struct IsValidDataType<xx::Data> : std::true_type {};

		template<typename T>
		struct IsValidDataType<std::optional<T>> : IsValidDataType<T> {};

		template<typename T>
		constexpr bool IsValidDataType_v = IsValidDataType<T>::value;


		// 查询主体
		struct Query {
		protected:
			Connection& conn;
			sqlite3_stmt* stmt = nullptr;
		public:
			typedef std::function<void(Reader & sr)> ReadFunc;

			Query(Connection& conn);
			Query(Connection& conn, char const* const& sql, int const& sqlLen = 0);
			template<size_t sqlLen>
			Query(Connection& conn, char const(&sql)[sqlLen]);
			~Query();
			Query() = delete;
			Query(Query const&) = delete;
			Query& operator=(Query const&) = delete;

			// 主用于判断 stmt 是否为空( 是否传入 sql )
			operator bool() const noexcept;

			// 配合 Query(Connection& conn) 后期传入 sql 以初始化 stmt
			void SetQuery(char const* const& sql, int const& sqlLen = 0);

			// 释放 stmt
			void Clear() noexcept;

			// 下面这些函数都是靠 try 来检测错误

			// SQLITE 支持的几种基础数据类型
			void SetParameter(int const& parmIdx, Null const& = {});
			void SetParameter(int const& parmIdx, int const& v);
			void SetParameter(int const& parmIdx, int64_t const& v);
			void SetParameter(int const& parmIdx, double const& v);
			void SetParameter(int const& parmIdx, char const* const& str, size_t const& len, bool const& makeCopy = false);		// str 传入 nullptr 将视作空值. sqlite 不支持 len > 2G
			void SetParameter(int const& parmIdx, uint8_t const* const& buf, size_t const& len, bool const& makeCopy = false);		// buf 传入 nullptr 将视作空值. sqlite 不支持 len > 2G

			// 枚举( 会转为 int / int64_t 再填 )
			template<typename EnumType, typename ENABLED = std::enable_if_t<std::is_enum_v<EnumType>>>
			void SetParameter(int const& parmIdx, EnumType const& v);

			// 字串类
			template<size_t len>
			void SetParameter(int const& parmIdx, char const(&str)[len], bool const& makeCopy = false);
			void SetParameter(int const& parmIdx, char const* const& str, bool const& makeCopy = false);
			void SetParameter(int const& parmIdx, std::string const& str, bool const& makeCopy = false);

			// 二进制类
			void SetParameter(int const& parmIdx, Data const& d, bool const& makeCopy = false);

			// 可空类型
			template<typename T>
			void SetParameter(int const& parmIdx, std::optional<T> const& v);
			template<typename T>
			void SetParameter(int const& parmIdx, std::shared_ptr<T> const& v);

			// 一次设置多个参数
			template<typename...Parameters>
			Query& SetParameters(Parameters const& ...ps);
		protected:
			template<typename Parameter, typename...Parameters>
			void SetParametersCore(int& parmIdx, Parameter const& p, Parameters const& ...ps);
			void SetParametersCore(int& parmIdx);

		public:
			// 执行查询并可选择使用 Reader 在传入回调中读出每一行数据
			Query& Execute(ReadFunc&& rf = nullptr);

			// 执行查询
			Query& operator()();

			// 执行查询 将返回结果集的第一行第一列的值 以指定数据类型 填充到 outVal. 如果没有返回值 则不会填充
			// 如果返回 null 但是 T 并非 optional 将 throw exception
			template<typename T>
			Query& operator()(T& outVal);
		};


		// 数据库主体
		struct Connection {
			// 保存 db 所在路径
			std::string path;

			// 保存最后一次的错误码
			int lastErrorCode = 0;

			// 保存 / 指向 最后一次的错误信息
			const char* lastErrorMessage = nullptr;

			friend struct Query;
			friend struct Reader;
		protected:

			// sqlite3 上下文
			sqlite3* ctx = nullptr;

			// 临时拿来拼 sql 串
			std::string sqlBuilder;

			// 预创建的一堆常用查询语句
			Query qBeginTransaction;
			Query qCommit;
			Query qRollback;
			Query qEndTransaction;
			Query qTableExists;
			Query qGetTableCount;
			Query qAttach;
			Query qDetach;

			// 为throw错误提供便利
			void ThrowError(int const& errCode, char const* const& errMsg = nullptr);
		public:
			// fn 可以是 :memory: 以创建内存数据库
			Connection(std::string const& path, bool const& readOnly = false) noexcept;
			~Connection();
			Connection() = delete;
			Connection(Connection const&) = delete;
			Connection& operator=(Connection const&) = delete;

			// 主用于判断构造函数是否执行成功( db 成功打开 )
			operator bool() const noexcept;

			// 获取受影响行数
			int GetAffectedRows();

			// 下列函数均靠 try 检测是否执行出错

			// 各种 set pragma( 通常推荐设置 JournalModes::WAL 似乎能提升一些 insert 的性能 )

			// 事务数据记录模式( 设成 WAL 能提升一些性能 )
			void SetPragmaJournalMode(JournalModes jm);

			// 启用外键约束( 默认为未启用 )
			void SetPragmaForeignKeys(bool enable);

			// 数据写盘模式( Off 最快 )
			void SetPragmaSynchronousType(SynchronousTypes st);

			// 临时表处理模式
			void SetPragmaTempStoreType(TempStoreTypes tst);

			// 排它锁持有方式( 文件被 单个应用独占情况下 EXCLUSIVE 最快 )
			void SetPragmaLockingMode(LockingModes lm);

			// 内存数据库页数( 4096 似乎最快 )
			void SetPragmaCacheSize(int cacheSize);


			// 附加另外一个库
			void Attach(char const* const& alias, char const* const& fn);	// ATTACH DATABASE 'fn' AS 'alias'

			// 反附加另外一个库
			void Detach(char const* const& alias);							// DETACH DATABASE 'alias'

			// 启动事务
			void BeginTransaction();										// BEGIN TRANSACTION

			// 提交事务
			void Commit();													// COMMIT TRANSACTION

			// 回滚
			void Rollback();												// ROLLBACK TRANSACTION

			// 结束事务( 同 Commit )
			void EndTransaction();											// END TRANSACTION

			// 返回 1 表示只包含 'sqlite_sequence' 这样一个预创建表. 
			// android 下也有可能返回 2, 有张 android 字样的预创建表存在
			int GetTableCount();											// SELECT count(*) FROM sqlite_master

			// 判断表是否存在
			bool TableExists(char const* const& tn);						// SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = ?

			// 会清空表数据, 并且重置自增计数. 如果存在约束, 或 sqlite_sequence 不存在, 有可能清空失败.
			void TruncateTable(char const* const& tn);						// DELETE FROM ?; DELETE FROM sqlite_sequence WHERE name = ?;

			// 直接执行一个 SQL 语句
			void Call(char const* const& sql, int(*selectRowCB)(void* userData, int numCols, char** colValues, char** colNames) = nullptr, void* const& userData = nullptr);

			// 直接执行一个 SQL 语句. 如果 T 不为 void, 将返回第一行第一列的值. 
			template<typename T = void>
			T Execute(char const* const& sql, int const& sqlLen);

			template<typename T = void, size_t sqlLen>
			T Execute(char const(&sql)[sqlLen]);
		};






		struct Reader {
		protected:
			Connection& conn;
			sqlite3_stmt* stmt = nullptr;
		public:
			int numCols = 0;

			Reader(Connection& conn, sqlite3_stmt* const& stmt);
			Reader() = delete;
			Reader(Reader const&) = delete;
			Reader& operator=(Reader const&) = delete;

			// 一组结构检索函数
			int GetColumnCount();
			DataTypes GetColumnDataType(int const& colIdx);
			char const* GetColumnName(int const& colIdx);
			int GetColumnIndex(char const* const& colName);
			int GetColumnIndex(std::string const& colName);

			// 判断字段值是否为空
			template<bool safeCheck = true>
			bool IsDBNull(int const& colIdx);

			// 一组数据转换/读取函数( 将复制数据 )
			template<bool safeCheck = true>
			int ReadInt32(int const& colIdx);
			template<bool safeCheck = true>
			int64_t ReadInt64(int const& colIdx);
			template<bool safeCheck = true>
			double ReadDouble(int const& colIdx);
			template<bool safeCheck = true>
			std::string ReadString(int const& colIdx);
			template<bool safeCheck = true>
			xx::Data ReadData(int const& colIdx);

			// 这几个函数不会复制数据, 需谨慎, 读出来马上就要用, 否则查询结束时 sqlite 回收结果集内存将导致失效!
			template<bool safeCheck = true>
			char const* ReadCString(int const& colIdx);
			template<bool safeCheck = true>
			std::pair<char const*, int> ReadText(int const& colIdx);
			template<bool safeCheck = true>
			std::pair<char const*, int> ReadBlob(int const& colIdx);
			template<bool safeCheck = true>
			std::string_view ReadStringView(int const& colIdx);

			// 填充
			template<bool safeCheck = true, typename T, class ENABLED = std::enable_if_t<IsValidDataType_v<T>>>
			void Read(int const& colIdx, T& outVal);
			template<typename T>
			void Read(char const* const& colName, T& outVal);
			template<typename T>
			void Read(std::string const& colName, T& outVal);

			// 一次填充多个
			template<typename...Args>
			void Reads(Args& ...args);
		protected:
			template<typename Arg, typename...Args>
			void ReadsCore(int& colIdx, Arg& arg, Args& ...args);
			void ReadsCore(int& colIdx);
		};





		/******************************************************************************************************/
		// impls
		/******************************************************************************************************/

		/***************************************************************/
		// Connection

		inline Connection::Connection(std::string const& path, bool const& readOnly) noexcept
			: path(path)
			, qBeginTransaction(*this)
			, qCommit(*this)
			, qRollback(*this)
			, qEndTransaction(*this)
			, qTableExists(*this)
			, qGetTableCount(*this)
			, qAttach(*this)
			, qDetach(*this)
		{
			int r = sqlite3_open_v2(path.c_str(), &ctx, readOnly ? SQLITE_OPEN_READONLY : (SQLITE_OPEN_READWRITE | SQLITE_OPEN_CREATE), nullptr);
			if (r != SQLITE_OK) {
				ctx = nullptr;
				lastErrorCode = r;
				lastErrorMessage = "db file open failed.";
				return;
			}

			qAttach.SetQuery("ATTACH DATABASE ? AS ?");
			qDetach.SetQuery("DETACH DATABASE ?");
			qBeginTransaction.SetQuery("BEGIN TRANSACTION");
			qCommit.SetQuery("COMMIT TRANSACTION");
			qRollback.SetQuery("ROLLBACK TRANSACTION");
			qEndTransaction.SetQuery("END TRANSACTION");
			qTableExists.SetQuery("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = ?");
			qGetTableCount.SetQuery("SELECT count(*) FROM sqlite_master WHERE type = 'table'");
		}

		inline Connection::~Connection() {
			if (ctx) {
				sqlite3_close(ctx);
				ctx = nullptr;
			}
		}

		inline Connection::operator bool() const noexcept {
			return ctx != nullptr;
		}

		inline void Connection::ThrowError(int const& errCode, char const* const& errMsg) {
			lastErrorCode = errCode;
			lastErrorMessage = errMsg ? errMsg : sqlite3_errmsg(ctx);
			throw errCode;
		}

		inline int Connection::GetAffectedRows() {
			return sqlite3_changes(ctx);
		}

		inline void Connection::SetPragmaSynchronousType(SynchronousTypes st) {
			if ((int)st > (int)SynchronousTypes::Off) ThrowError(-1, "bad SynchronousTypes");
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA synchronous = ");
			sqlBuilder.append(strSynchronousTypes[(int)st]);
			Call(sqlBuilder.c_str());
		}

		inline void Connection::SetPragmaJournalMode(JournalModes jm) {
			if ((int)jm > (int)JournalModes::Off) ThrowError(-1, "bad JournalModes");
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA journal_mode = ");
			sqlBuilder.append(strJournalModes[(int)jm]);
			Call(sqlBuilder.c_str());
		}

		inline void Connection::SetPragmaTempStoreType(TempStoreTypes tst) {
			if ((int)tst > (int)TempStoreTypes::Memory) ThrowError(-1, "bad TempStoreTypes");
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA temp_store = ");
			sqlBuilder.append(strTempStoreTypes[(int)tst]);
			Call(sqlBuilder.c_str());
		}

		inline void Connection::SetPragmaLockingMode(LockingModes lm) {
			if ((int)lm > (int)LockingModes::Exclusive) ThrowError(-1, "bad LockingModes");
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA locking_mode = ");
			sqlBuilder.append(strLockingModes[(int)lm]);
			Call(sqlBuilder.c_str());
		}

		inline void Connection::SetPragmaCacheSize(int cacheSize) {
			if (cacheSize < 1) ThrowError(-1, "bad cacheSize( default is 2000 )");
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA cache_size = ");
			sqlBuilder.append(std::to_string(cacheSize));
			Call(sqlBuilder.c_str());
		}

		inline void Connection::SetPragmaForeignKeys(bool enable) {
			sqlBuilder.clear();
			sqlBuilder.append("PRAGMA foreign_keys = ");
			sqlBuilder.append(enable ? "true" : "false");
			Call(sqlBuilder.c_str());
		}

		inline void Connection::Call(char const* const& sql, int(*selectRowCB)(void* userData, int numCols, char** colValues, char** colNames), void* const& userData) {
			lastErrorCode = sqlite3_exec(ctx, sql, selectRowCB, userData, (char**)&lastErrorMessage);
			if (lastErrorCode != SQLITE_OK) throw lastErrorCode;
		}

		template<typename T>
		inline T Connection::Execute(char const* const& sql, int const& sqlLen) {
			Query q(*this, sql, sqlLen);
			if constexpr (std::is_void_v<T>) {
				q();
			}
			else {
				T rtv{};
				q(rtv);
				return rtv;
			}
		}

		template<typename T, size_t sqlLen>
		T Connection::Execute(char const(&sql)[sqlLen]) {
			return Execute<T>(sql, (int)sqlLen);
		}

		inline void Connection::Attach(char const* const& alias, char const* const& fn) {
			qAttach.SetParameters(fn, alias)();
		}

		inline void Connection::Detach(char const* const& alias) {
			qDetach.SetParameters(alias)();
		}

		inline void Connection::BeginTransaction() {
			qBeginTransaction();
		}

		inline void Connection::Commit() {
			qCommit();
		}

		inline void Connection::Rollback() {
			qRollback();
		}

		inline void Connection::EndTransaction() {
			qEndTransaction();
		}

		inline bool Connection::TableExists(char const* const& tn) {
			int exists = 0;
			qTableExists.SetParameters(tn)(exists);
			return exists != 0;
		}

		inline int Connection::GetTableCount() {
			int count = 0;
			qGetTableCount(count);
			return count;
		}

		inline void Connection::TruncateTable(char const* const& tn) {
			// todo: 对 tn 转义
			sqlBuilder.clear();
			sqlBuilder += std::string("BEGIN; DELETE FROM `") + tn + "`; DELETE FROM `sqlite_sequence` WHERE `name` = '" + tn + "'; COMMIT;";
			Call(sqlBuilder.c_str());
		}




		/***************************************************************/
		// Query

		inline Query::Query(Connection& conn)
			: conn(conn) {
		}

		inline Query::Query(Connection& conn, char const* const& sql, int const& sqlLen)
			: conn(conn) {
			SetQuery(sql, sqlLen);
		}

		template<size_t sqlLen>
		inline Query::Query(Connection& conn, char const(&sql)[sqlLen])
			: conn(conn) {
			SetQuery(sql, sqlLen - 1);
		}

		inline Query::~Query() {
			Clear();
		}

		inline Query::operator bool() const noexcept {
			return stmt != nullptr;
		}

		inline void Query::SetQuery(char const* const& sql, int const& sqlLen) {
			Clear();
			auto r = sqlite3_prepare_v2(conn.ctx, sql, sqlLen ? sqlLen : (int)strlen(sql), &stmt, nullptr);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::Clear() noexcept {
			if (stmt) {
				sqlite3_finalize(stmt);
				stmt = nullptr;
			}
		}

		inline void Query::SetParameter(int const& parmIdx, Null const&) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			auto r = sqlite3_bind_null(stmt, parmIdx);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::SetParameter(int const& parmIdx, int const& v) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			auto r = sqlite3_bind_int(stmt, parmIdx, v);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::SetParameter(int const& parmIdx, int64_t const& v) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			auto r = sqlite3_bind_int64(stmt, parmIdx, v);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::SetParameter(int const& parmIdx, double const& v) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			auto r = sqlite3_bind_double(stmt, parmIdx, v);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::SetParameter(int const& parmIdx, char const* const& str, size_t const& len, bool const& makeCopy) {
			if (!str) {
				SetParameter(parmIdx, Null{});
				return;
			}
			if (!stmt) conn.ThrowError(-1, "query is empty");
			int r = SQLITE_OK;
			r = sqlite3_bind_text(stmt, parmIdx, str, len ? (int)len : (int)strlen(str), makeCopy ? SQLITE_TRANSIENT : SQLITE_STATIC);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		inline void Query::SetParameter(int const& parmIdx, uint8_t const* const& buf, size_t const& len, bool const& makeCopy) {
			if (!buf) {
				SetParameter(parmIdx, Null{});
				return;
			}
			if (!stmt) conn.ThrowError(-1, "query is empty");
			int r = SQLITE_OK;
			r = sqlite3_bind_blob(stmt, parmIdx, buf, (int)len, makeCopy ? SQLITE_TRANSIENT : SQLITE_STATIC);
			if (r != SQLITE_OK) conn.ThrowError(r);
		}

		template<size_t len>
		inline void Query::SetParameter(int const& parmIdx, char const(&str)[len], bool const& makeCopy) {
			SetParameter(parmIdx, str, len - 1, makeCopy);
		}
		inline void Query::SetParameter(int const& parmIdx, char const* const& str, bool const& makeCopy) {
			SetParameter(parmIdx, str, 0, makeCopy);
		}
		inline void Query::SetParameter(int const& parmIdx, std::string const& str, bool const& makeCopy) {
			SetParameter(parmIdx, (char*)str.c_str(), str.size(), makeCopy);
		}

		inline void Query::SetParameter(int const& parmIdx, Data const& d, bool const& makeCopy) {
			SetParameter(parmIdx, d.buf, d.len, makeCopy);
		}

		template<typename T>
		inline void Query::SetParameter(int const& parmIdx, std::optional<T> const& v) {
			if (v.has_value()) {
				SetParameter(parmIdx, v.value());
			}
			else {
				SetParameter(parmIdx, Null{});
			}
		}

		template<typename T>
		inline void Query::SetParameter(int const& parmIdx, std::shared_ptr<T> const& v) {
			if (v) {
				SetParameter(parmIdx, *v);
			}
			else {
				SetParameter(parmIdx, Null{});
			}
		}

		template<typename EnumType, typename ENABLED>
		void Query::SetParameter(int const& parmIdx, EnumType const& v) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			if constexpr (sizeof(EnumType) <= 4) {
				SetParameter(parmIdx, (int)(typename std::underlying_type<EnumType>::type)v);
			}
			else {
				SetParameter(parmIdx, (int64_t)(typename std::underlying_type<EnumType>::type)v);
			}
		}

		template<typename...Parameters>
		Query& Query::SetParameters(Parameters const& ...ps) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			int parmIdx = 1;
			SetParametersCore(parmIdx, ps...);
			return *this;
		}

		template<typename Parameter, typename...Parameters>
		void Query::SetParametersCore(int& parmIdx, Parameter const& p, Parameters const& ...ps) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			SetParameter(parmIdx, p);
			SetParametersCore(++parmIdx, ps...);
		}

		inline void Query::SetParametersCore(int& parmIdx) {
			(void)parmIdx;
		}

		inline Query& Query::Execute(ReadFunc&& rf) {
			if (!stmt) conn.ThrowError(-1, "query is empty");
			Reader dr(conn, stmt);

			int r = sqlite3_step(stmt);
			if (r == SQLITE_DONE || (r == SQLITE_ROW && !rf)) goto LabEnd;
			else if (r != SQLITE_ROW) goto LabErr;

			dr.numCols = sqlite3_column_count(stmt);
			do
			{
				rf(dr);
				r = sqlite3_step(stmt);
			} while (r == SQLITE_ROW);
			assert(r == SQLITE_DONE);

		LabEnd:
			r = sqlite3_reset(stmt);
			if (r == SQLITE_OK) return *this;

		LabErr:
			auto ec = r;
			auto em = sqlite3_errmsg(conn.ctx);
			sqlite3_reset(stmt);
			conn.ThrowError(ec, em);
			return *this;
		}

		inline Query& Query::operator()() {
			return Execute(nullptr);
		}

		template<typename T>
		Query& Query::operator()(T& outVal) {
			return Execute([&](Reader& r) {
				if (!r.numCols) return;
				r.Read(0, outVal);
				});
		}


		/***************************************************************/
		// Reader

		inline Reader::Reader(Connection& conn, sqlite3_stmt* const& stmt) : conn(conn), stmt(stmt) {}

		inline int Reader::GetColumnCount() {
			return sqlite3_column_count(stmt);
		}

		inline DataTypes Reader::GetColumnDataType(int const& colIdx) {
			if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
			return (DataTypes)sqlite3_column_type(stmt, colIdx);
		}

		inline char const* Reader::GetColumnName(int const& colIdx) {
			if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
			return sqlite3_column_name(stmt, colIdx);
		}

		inline int Reader::GetColumnIndex(char const* const& colName) {
			int count = sqlite3_column_count(stmt);
			for (int i = 0; i < count; ++i) {
				if (!strcmp(colName, sqlite3_column_name(stmt, i))) return i;
			}
			return -1;
		}

		inline int Reader::GetColumnIndex(std::string const& colName) {
			return GetColumnIndex(colName.c_str());
		}


		template<bool safeCheck>
		inline bool Reader::IsDBNull(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
			}
			return GetColumnDataType(colIdx) == DataTypes::Null;
		}

		template<bool safeCheck>
		inline int Reader::ReadInt32(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) conn.ThrowError(-3, "can't convert null to int");
			}
			return sqlite3_column_int(stmt, colIdx);
		}

		template<bool safeCheck>
		inline int64_t Reader::ReadInt64(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) conn.ThrowError(-3, "can't convert null to int64_t");
			}
			return sqlite3_column_int64(stmt, colIdx);
		}

		template<bool safeCheck>
		inline double Reader::ReadDouble(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) conn.ThrowError(-3, "can't convert null to double");
			}
			return sqlite3_column_double(stmt, colIdx);
		}

		template<bool safeCheck>
		inline char const* Reader::ReadCString(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) return nullptr;
			}
			return (char const*)sqlite3_column_text(stmt, colIdx);
		}

		template<bool safeCheck>
		inline std::pair<char const*, int> Reader::ReadText(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) return std::make_pair(nullptr, 0);
			}
			auto ptr = (char const*)sqlite3_column_text(stmt, colIdx);
			auto len = sqlite3_column_bytes(stmt, colIdx);
			return std::make_pair(ptr, len);
		}

		template<bool safeCheck>
		inline std::pair<char const*, int> Reader::ReadBlob(int const& colIdx) {
			if constexpr (safeCheck) {
				if (colIdx < 0 || colIdx >= numCols) conn.ThrowError(-2, "colIdx out of range");
				if (IsDBNull<false>(colIdx)) return std::make_pair(nullptr, 0);
			}
			auto ptr = (char const*)sqlite3_column_blob(stmt, colIdx);
			auto len = sqlite3_column_bytes(stmt, colIdx);
			return std::make_pair(ptr, len);
		}

		template<bool safeCheck>
		inline std::string Reader::ReadString(int const& colIdx) {
			auto r = ReadText<safeCheck>(colIdx);
			if constexpr (safeCheck) {
				if (!r.first) conn.ThrowError(-3, "can't convert null to std::string");
			}
			return std::string(r.first, r.second);
		}

		template<bool safeCheck>
		inline std::string_view Reader::ReadStringView(int const& colIdx) {
			auto r = ReadText<safeCheck>(colIdx);
			if constexpr (safeCheck) {
				if (!r.first) conn.ThrowError(-3, "can't convert null to std::string_view");
			}
			return std::string_view(r.first, r.second);
		}

		template<bool safeCheck>
		inline xx::Data Reader::ReadData(int const& colIdx) {
			auto r = ReadBlob<safeCheck>(colIdx);
			if constexpr (safeCheck) {
				if (!r.first) conn.ThrowError(-3, "can't convert null to xx::Data");
			}
			return xx::Data(r.first, r.second);
		}


		template<bool safeCheck, typename T, class ENABLED>
		inline void Reader::Read(int const& colIdx, T& outVal) {
			if constexpr (std::is_same_v<int, T>) {
				outVal = ReadInt32<safeCheck>(colIdx);
			}
			else if constexpr (std::is_same_v<int64_t, T>) {
				outVal = ReadInt64<safeCheck>(colIdx);
			}
			else if constexpr (std::is_same_v<double, T>) {
				outVal = ReadDouble<safeCheck>(colIdx);
			}
			else if constexpr (std::is_same_v<std::string, T>) {
				outVal = std::move(ReadString<safeCheck>(colIdx));
			}
			else if constexpr (std::is_same_v<xx::Data, T>) {
				outVal = std::move(ReadData<safeCheck>(colIdx));
			}
			else if constexpr (std::is_enum_v<T>) {
				if constexpr (sizeof(T) <= 4) {
					outVal = (T)ReadInt32<safeCheck>(colIdx);
				}
				else {
					outVal = (T)ReadInt64<safeCheck>(colIdx);
				}
			}
			else if constexpr (xx::IsOptional_v<T>) {
				if (IsDBNull(colIdx)) {
					outVal.reset();
				}
				else {
					outVal = xx::ChildType_t<T>();
					Read<false>(colIdx, outVal.value());
				}
			}
			else {
				// not support
				assert(false);
			}
		}

		template<typename T>
		inline void Reader::Read(char const* const& colName, T& outVal) {
			Read(GetColumnIndex(colName), outVal);
		}
		template<typename T>
		inline void Reader::Read(std::string const& colName, T& outVal) {
			Read(colName.c_str(), outVal);
		}



		// 一次填充多个
		template<typename...Args>
		inline void Reader::Reads(Args& ...args) {
			int colIdx = 0;
			ReadsCore(colIdx, args...);
		}
		template<typename Arg, typename...Args>
		inline void Reader::ReadsCore(int& colIdx, Arg& arg, Args& ...args) {
			Read(colIdx, arg);
			ReadsCore(++colIdx, args...);
		}
		inline void Reader::ReadsCore(int& colIdx) {
			(void)colIdx;
		}
	}
}
