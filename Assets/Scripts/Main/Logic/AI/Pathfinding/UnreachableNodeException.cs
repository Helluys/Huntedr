using System;
using System.Runtime.Serialization;

[Serializable]
internal class UnreachableNodeException : Exception {
    public UnreachableNodeException () : this("Unreachable node for pathfinding") {
    }

    public UnreachableNodeException (string message) : base(message) {
    }

    public UnreachableNodeException (string message, Exception innerException) : base(message, innerException) {
    }

    protected UnreachableNodeException (SerializationInfo info, StreamingContext context) : base(info, context) {
    }
}