using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Common.Library.Log;
using Common.Library.Caching;
using Admin.Model;

namespace Core.Common.Service
{
    public class AD_Service : ServiceBase<T_AD>
    {
        private Image_Record_Service _irsv = null;

        public AD_Service(IDbConnection db)
            : base(db)
        {
            _irsv = new Image_Record_Service(db);
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T_AD Get(dynamic id)
        {
            return CacheManager.Get(CacheKey.GetCacheKey<T_AD>(
                ((object)id).ToSafeString()
                ), () =>
                {
                    return base.Get((int)id);
                });
        }



        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override long Insert(T_AD entity)
        {
            var trans = db.BeginTransaction();
            long r = 0;
            try
            {
                r = db.Insert(entity, trans);
                if (r > 0)
                {
                    _irsv.UpdateNumByObject(entity, "{" + ImageRecordType.AD.ToString() + ":" + r.ToString() + "}", true, trans);
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }

                if (r > 0)
                {
                    base.RemoveCacheByThisType();
                }

                return r;
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                trans.Rollback();
                return 0;
            }
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="oldentity"></param>
        /// <returns></returns>
        public bool Update(T_AD entity, T_AD oldentity)
        {
            var trans = db.BeginTransaction();
            var r = false;
            try
            {
                r = db.Update(entity, trans);
                if (r)
                {
                    if (oldentity != null)
                    {
                        _irsv.UpdateNumByObject(oldentity, "{" + ImageRecordType.AD.ToString() + ":" + entity.F_Ad_Id.ToString() + "}", false, trans);
                    }
                    _irsv.UpdateNumByObject(entity, "{" + ImageRecordType.AD.ToString() + ":" + entity.F_Ad_Id.ToString() + "}", true, trans);
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }

                if (r)
                {
                    base.RemoveCacheByThisType();
                }

                return r;

            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                trans.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 改变信息显示状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int ChangeLock(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var r = db.Execute("update T_AD set F_Ad_IsLock=-(F_Ad_IsLock-1) where F_Ad_Id in (@ids)", new { ids });

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Delete(int id, IDbTransaction trans = null)
        {
            var m = db.Get<T_AD>(id, trans);

            if (m == null)
            {
                return true;
            }

            var r = db.Delete(m, trans);

            if (r)
            {
                _irsv.UpdateNumByObject(m, "{" + ImageRecordType.AD.ToString() + ":" + m.F_Ad_Id.ToString() + "}", false, trans);
                base.RemoveCacheByThisType();
            }

            return r;

        }

        /// <summary>
        /// 批量删除广告
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            var count = 0;
            var trans = db.BeginTransaction();
            try
            {
                foreach (var item in ids)
                {
                    if (Delete(item, trans))
                    {
                        count++;
                    }
                    else
                    {
                        trans.Rollback();
                        return 0;
                    }
                }

                trans.Commit();
                return count;
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                trans.Rollback();
                return 0;
            }
        }
    }
}
