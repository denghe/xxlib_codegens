#include "PKG_class.h"
int main() {
	xx::BBuffer bb;
	PKG::TestNamespace::B b;
	b._float = 1.23f;
	b._pos = { 1.1f, 2.2f };
	bb.Write(b);
	xx::CoutN(bb);
	std::cin.get();
	return 0;
}
