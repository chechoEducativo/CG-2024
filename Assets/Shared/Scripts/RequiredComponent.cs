using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredComponent<TComp> where TComp:Component
{
    private TComp contained;

    public TComp Get(Component c)
    {
        if (contained == null)
        {
            contained = c.GetComponent<TComp>();
        }

        return contained;
    }
    
    public static implicit operator TComp(RequiredComponent<TComp> c)
    {
        return c.contained;
    }
}
