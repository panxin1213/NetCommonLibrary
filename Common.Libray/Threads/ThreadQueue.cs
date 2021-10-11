using Common.Library.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common.Library.Threads
{
    public abstract class ThreadQueue<T> where T : class,new()
    {

        private ListBack<T> List = null;//检索语句队列

        private int ThreadingCount = 0;//当前线程数

        private int MaxThreadCount = 10;//最大线程数

        private bool isRun = false;//是否正在开始运行

        private bool isRandom = true;//随机取元素

        public Dictionary<Thread, DateTime> threadList = new Dictionary<Thread, DateTime>();//当前线程集合

        protected int ThreadTimeOutNumber = 299000;//线程超时时间

        protected int sleepTime = 3000;//线程等待时间

        /// <summary>
        /// 队列剩余待处理信息条数
        /// </summary>
        public int ListCount
        {
            get
            {
                lock (List)
                {
                    if (List == null)
                    {
                        return 0;
                    }

                    return List.Count;
                }
            }
        }

        public ThreadQueue(int maxThreadCount, bool IsRandom)
        {
            Init(maxThreadCount, IsRandom);
        }
        public ThreadQueue(int maxThreadCount)
        {
            Init(maxThreadCount, false);
        }


        private void Init(int maxThreadCount, bool IsRandom)
        {
            if (maxThreadCount > 0)
            {
                MaxThreadCount = maxThreadCount;
            }
            List = new ListBack<T>((l) =>
            {
                if (!isRun && l.Count > 0)
                {
                    isRun = true;
                    new Thread(AnalyticList).Start();
                }
            });
            isRandom = IsRandom;
        }

        /// <summary>
        /// 将集合插入到检索语句队列
        /// </summary>
        /// <param name="keylist"></param>
        public virtual void SetList(List<T> list)
        {
            List.AddRange(list);
        }

        /// <summary>
        /// 检索语句队列改变后触发方法
        /// </summary>
        private void AnalyticList()
        {
            Stopwatch _timer = new Stopwatch();
            _timer.Start();
            do
            {
                //打开指定线程数
                while (ThreadingCount < MaxThreadCount && List != null && List.Count > 0)
                {
                    var thread = new Thread(AnalyticThreadAction);
                    lock (List)
                    {
                        ThreadingCount++;
                    }
                    thread.Start();
                    threadList.Add(thread, DateTime.Now);
                }
                Thread.Sleep(sleepTime);
            }
            while ((List != null && List.Count > 0) || ThreadingCount > 0);
            _timer.Stop();
            Logger.Info(this, string.Format("--------{0} End--------", this.GetType().FullName) + _timer.Elapsed.TotalMilliseconds.ToString());
            isRun = false;
        }

        /// <summary>
        /// 线程操作方法
        /// </summary>
        private void AnalyticThreadAction()
        {
            try
            {
                while (List != null && List.Count > 0)
                {
                    //获取KeyList中的一个key，没有则结束线程
                    T m = null;
                    lock (List)
                    {
                        if (List == null || List.Count == 0)
                        {
                            lock (List)
                            {
                                ThreadingCount--;
                                threadList.Remove(Thread.CurrentThread);
                                Thread.CurrentThread.DisableComObjectEagerCleanup();
                            }
                            return;
                        }
                        m = List[isRandom ? new Random().Next(0, List.Count) : 0];
                        List.Remove(m);
                    }

                    Thread thread = null;
                    try
                    {
                        thread = new Thread((o) =>
                        {
                            try
                            {
                                AnalyticCallBack(m);
                            }
                            catch (Exception e)
                            {
                                Logger.Error(this, e.Message, e);
                            }
                        });

                        thread.Start(Thread.CurrentThread.ManagedThreadId);
                        var mt = 0;
                        while (mt < ThreadTimeOutNumber)
                        {
                            if (thread.ThreadState == System.Threading.ThreadState.Stopped)
                            {
                                thread.DisableComObjectEagerCleanup();
                                break;
                            }
                            mt += 5000;
                            Thread.Sleep(5000);
                        }

                        if (thread != null && thread.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            thread.Abort();
                            thread.DisableComObjectEagerCleanup();
                            Logger.Info("ThreadQueueTimeout", "Timeout:" + m.ToJson(), null);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(this, e.Message, e);
                        if (thread != null)
                        {
                            thread.DisableComObjectEagerCleanup();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
            }
            finally
            {
                lock (List)
                {
                    ThreadingCount--;
                    threadList.Remove(Thread.CurrentThread);
                    Thread.CurrentThread.DisableComObjectEagerCleanup();
                }
            }
        }

        /// <summary>
        /// 线程业务处理方法
        /// </summary>
        /// <param name="m"></param>
        protected abstract void AnalyticCallBack(T m);

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRuning
        {
            get
            {
                return ThreadingCount > 0 || isRun;
            }
        }
    }
}
