#pragma once
#include "xx_string.h"
namespace xx {
	struct Object {
		Object() = default;
		virtual ~Object() = default;
		Object(Object const&) = delete;
		Object& operator=(Object const&) = delete;
		Object(Object&&) = delete;
		Object& operator=(Object&&) = delete;

		// 序列化相关
		inline virtual uint16_t GetTypeId() const noexcept { return 0; }
		inline virtual void Serialize(Serializer& bb) const noexcept { (void)bb; }
		inline virtual int Deserialize(Deserializer& bb) noexcept { (void)bb; return 0; }

		// 字串输出相关
		inline virtual void ToString(std::string& s) const noexcept { (void)s; };
		inline virtual void ToStringCore(std::string& s) const noexcept { (void)s; };
		bool toStringFlag = false;
		inline void SetToStringFlag(bool const& b = true) const noexcept {
			const_cast<Object*>(this)->toStringFlag = b;
		}

		// 级联相关( 主用于遍历调用生成物派生类 override 的代码 )
		inline virtual int Cascade(void* const& o = nullptr) noexcept { return CascadeCore(o); };
		inline virtual int CascadeCore(void* const& o = nullptr) noexcept { (void)o; return 0; };
	};

	using Object_s = std::shared_ptr<Object>;

	// 适配 T : Object
	template<typename T>
	struct SFuncs<T, std::enable_if_t<std::is_base_of_v<Object, T>>> {
		static inline void Append(std::string& s, T const& in) noexcept {
			in.ToString(s);
		}
	};

	// 适配 std::shared_ptr<T : Object>
	template<typename T>
	struct SFuncs<std::shared_ptr<T>, std::enable_if_t<std::is_base_of_v<Object, T>>> {
		static inline void Append(std::string& s, std::shared_ptr<T> const& in) noexcept {
			if (in) {
				SFuncs<T>::Append(s, *in);
			}
			else {
				s.append("nil");
			}
		}
	};

	// 适配 std::weak_ptr<T : Object>
	template<typename T>
	struct SFuncs<std::weak_ptr<T>, std::enable_if_t<std::is_base_of_v<Object, T>>> {
		static inline void Append(std::string& s, std::weak_ptr<T> const& in) noexcept {
			if (auto o = in.lock()) {
				SFuncs<T>::Append(s, *o);
			}
			else {
				s.append("nil");
			}
		}
	};

	// BFuncs 适配在 xx_serializer.h

	template<typename T>
	struct CFuncs<std::shared_ptr<T>, std::enable_if_t<std::is_base_of_v<xx::Object, T>>> {
		static inline int Cascade(void* const& o, std::shared_ptr<T> const& in) noexcept {
			return in->Cascade(o);
		}
	};
}
