using System.Collections.Generic;

public class Tree<T>
{
    public T                Data;
    private List<Tree<T>>   children;

    public Tree(T data)
    {
        this.Data = data;
        children = new List<Tree<T>>();
    }

    public void AddChild(T data)
    {
        children.Add(new Tree<T>(data));
    }

    public Tree<T> GetChild(int i)
    {
        return children[i];
    }

    public static void Visit( Tree<T> node, System.Action<T> visitor )
    {
        visitor(node.Data);
        foreach( Tree<T> kid in node.children ) {
            Visit(kid, visitor);
        }
    }

    public static Tree<T>  FindNode( Tree<T> node, System.Func<T, bool> pred )
    {
        if( node == null )
        {
            return null;
        }

        if( pred(node.Data) )
        {
            return node;
        }

        Tree<T> n = null;
        foreach( Tree<T> kid in node.children )
        {
            n = FindNode(kid, pred);
            if( n != null )
            {
                return n;
            }
        }

        return null;
    } 
}