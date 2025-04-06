namespace SaveManagerCLI.OptionTree;

public interface ILeaf;

public class Leaf<T>(string name, T value) : Node<T>(name, value), ILeaf;