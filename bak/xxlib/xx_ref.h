#pragma once
#include "xx_itempool.h"
namespace xx {
	struct RefBase;
	inline xx::ItemPool<std::unique_ptr<RefBase>> allRefs;

	struct RefBase {
		int indexAtContainer = -1;
		inline void Dispose() {
			if (indexAtContainer != -1) {
				auto indexAtContainer = this->indexAtContainer;	// copy to stack for fix gcc issue
				allRefs.RemoveAt(indexAtContainer);
			}
		}
		virtual ~RefBase() {}
	};

	template<typename T>
	struct Ref {
		Ref() = default;
		Ref(Ref const&) = default;
		Ref& operator=(Ref const&) = default;

		int index = -1;
		int64_t version = 0;

		Ref(T* const& ptr) {
			static_assert(std::is_base_of_v<RefBase, T>);
			Reset(ptr);
		}
		Ref(std::unique_ptr<T> const& ptr) : Ref(ptr.get()) {}
		Ref(Ref&& o)
			: index(o.index)
			, version(o.version) {
			o.items = nullptr;
			o.index = -1;
			o.version = 0;
		}
		Ref& operator=(Ref&& o) {
			std::swap(index, o.index);
			std::swap(version, o.version);
			return *this;
		}
		template<typename U>
		Ref& operator=(std::enable_if_t<std::is_base_of_v<T, U>, Ref<U>> const& o) {
			return operator=(*(Ref<T>*) & o);
		}
		template<typename U>
		Ref& operator=(std::enable_if_t<std::is_base_of_v<T, U>, Ref<U>>&& o) {
			return operator=(std::move(*(Ref<T>*) & o));
		}
		template<typename U>
		Ref<U> As() const {
			if (!dynamic_cast<U*>(Lock())) return Ref<U>();
			return *(Ref<U>*)this;
		}
		operator bool() const {
			return version && allRefs.VersionAt(index) == version;
		}
		T* operator->() const {
			if (!operator bool()) throw - 1;		// ø’÷∏’Î
			return (T*)allRefs.ValueAt(index).get();
		}
		T* Lock() const {
			return operator bool() ? (T*)allRefs.ValueAt(index).get() : nullptr;
		}
		template<typename U = T>
		void Reset(T* const& ptr = nullptr) {
			static_assert(std::is_base_of_v<T, U>);
			if (!ptr) {
				index = -1;
				version = 0;
			}
			else {
				assert(ptr->indexAtContainer != -1);
				index = ptr->indexAtContainer;
				version = allRefs.VersionAt(index);
			}
		}
	};
	template<typename A, typename B>
	inline bool operator==(Ref<A> const& a, Ref<B> const& b) {
		return a.Lock() == b.Lock();
	}
	template<typename A, typename B>
	inline bool operator!=(Ref<A> const& a, Ref<B> const& b) {
		return a.Lock() != b.Lock();
	}

	template<typename T, typename ENABLED = std::enable_if_t<std::is_base_of_v<RefBase, T>>, typename ...Args>
	Ref<T> MakeRef(Args&&... args) {
		auto&& u = std::make_unique<T>(std::forward<Args>(args)...);
		auto&& p = u.get();
		p->indexAtContainer = allRefs.Add(std::move(u));
		return p;
	}
}
