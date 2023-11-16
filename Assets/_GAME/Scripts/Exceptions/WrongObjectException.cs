using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class WrongObjectException : Exception
{
    public WrongObjectException() :
        base($"Dragged object dropped on Wrong Item.")
    {
        
    }
}
