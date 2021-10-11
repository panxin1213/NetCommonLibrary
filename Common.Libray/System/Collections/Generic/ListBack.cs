using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// 返回方法集合,调用addrange方法后出发传入方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListBack<T> : List<T>
    {
        private Action<IList<T>> BackAction = null;

        public ListBack(Action<IList<T>> action)
        {
            BackAction = action;
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);

            BackAction(this);
        }


    }
}
