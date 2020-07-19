#pragma warning disable 0169, 0414
using TemplateLibrary;

[TypeId(12)]
class Scene : Node {
}

[TypeId(34)]
class Node {
    Weak<Node> parent;
    List<Shared<Node>> childs;
}
