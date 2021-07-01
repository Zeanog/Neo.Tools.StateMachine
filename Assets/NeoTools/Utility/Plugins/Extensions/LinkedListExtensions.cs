using System.Collections.Generic;

//namespace Neo.Extensions {
public static class LinkedListExtensions
{
    public static LinkedListNode<T> Find<T>( this LinkedList<T> self, System.Predicate<T> predicate ) {
        LinkedListNode<T> node = self.First;

        while( node != null ) {
            if(predicate(node.Value)) {
                return node;
            }

            node = node.Next;
        }

        return null;
    }
}
//}