using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 域名查询.Code
{
    public delegate void DelegateComm();
    public delegate void DelegateComm<A>(A a);
    public delegate void DelegateComm<A, B>(A a, B b);
    public delegate void DelegateComm<A, B, C>(A a, B b, C c);
}
