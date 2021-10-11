using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Linq.Expressions;
using System.Web.Routing;
namespace Dapper.Contrib.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SqlMapperSmanExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary = new Dictionary<string, ISqlAdapter>() {
																							{"sqlconnection", new SqlServerAdapter()},
																							{"npgsqlconnection", new PostgresAdapter()}
																						};

        private static IEnumerable<PropertyInfo> KeyPropertiesCache(Type type)
        {

            IEnumerable<PropertyInfo> pi;
            if (KeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi;
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count == 0)
            {
                var idProp = allProperties.Where(p => p.Name.ToLower() == "id").FirstOrDefault();
                if (idProp != null)
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }
        private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pis;
            if (TypeProperties.TryGetValue(type.TypeHandle, out pis))
            {
                return pis;
            }

            var properties = type.GetProperties().Where(SqlMapperExtensions.IsWriteable);
            TypeProperties[type.TypeHandle] = properties;
            return properties;
        }


        private static string GetTableName(Type type)
        {
            string name;
            if (!TypeTableName.TryGetValue(type.TypeHandle, out name))
            {
                name = type.Name + "s";
                if (type.IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);

                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableattr = type.GetCustomAttributes(false).Where(attr => attr.GetType().Name == "TableAttribute").SingleOrDefault() as
                    dynamic;
                if (tableattr != null)
                    name = tableattr.Name;
                TypeTableName[type.TypeHandle] = name;
            }
            return name;
        }
        /// <summary>
        /// 批量插入(可优化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entitysToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>返回最后一次更新的identity</returns>
        public static long Insert<T>(this IDbConnection connection, IEnumerable<T> entitysToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            bool outerTran = true;
            if (transaction == null)
            {
                outerTran = false;
                transaction = connection.BeginTransaction();
            }
            long r = 0;
            try
            {
                foreach (var entity in entitysToInsert)
                {
                    r = connection.Insert(entity, transaction, commandTimeout);
                }
                if (!outerTran) transaction.Commit();
            }
            catch (Exception e)
            {
                if (!outerTran) transaction.Rollback();
                throw e;
            }

            return r;
        }
        /// <summary>
        /// 批量插入包括主键(可优化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entitysToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int InsertWithKeyField<T>(this IDbConnection connection, IEnumerable<T> entitysToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            bool outerTran = true;
            if (transaction == null)
            {
                outerTran = false;
                transaction = connection.BeginTransaction();
            }
            int r = 0;
            try
            {
                foreach (var entity in entitysToInsert)
                {
                    if (connection.InsertWithKeyField(entity, transaction, commandTimeout))
                        r++;
                }
                if (!outerTran) transaction.Commit();
            }
            catch (Exception e)
            {
                r = 0;
                if (!outerTran) transaction.Rollback();
                else throw e;
            }
            return r;
        }
        /// <summary>
        /// 批量更新指定字段 (可优化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entitysToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="propertys">要更新的字段</param>
        /// <returns></returns>
        public static int Update<T>(this IDbConnection connection, IEnumerable<T> entitysToInsert, string sqlCondition = null, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] propertys) where T : class
        {
            var istrans = false;

            if (transaction == null)
            {
                istrans = true;
                transaction = connection.BeginTransaction();
            }
            int r = 0;
            try
            {
                foreach (var entity in entitysToInsert)
                {
                    if (connection.Update(entity, sqlCondition, transaction, commandTimeout, propertys))
                        r++;
                }
                if (istrans)
                    transaction.Commit();
            }
            catch (Exception e)
            {
                r = 0;
                if(istrans)
                    transaction.Rollback();
                throw e;
            }
            return r;
        }
        /// <summary>
        /// 批量更新 排除指定字段(可优化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entitysToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="excludePropertys">要排除的字段</param>
        /// <returns></returns>
        public static int UpdateExclude<T>(this IDbConnection connection, IEnumerable<T> entitysToInsert, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] excludePropertys) where T : class
        {
            if (transaction == null)
                transaction = connection.BeginTransaction();
            int r = 0;
            try
            {
                foreach (var entity in entitysToInsert)
                {
                    if (connection.UpdateExclude(entity, null, transaction, commandTimeout, excludePropertys))
                        r++;
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                r = 0;
                transaction.Rollback();
                throw e;
            }
            return r;
        }
        /// <summary>
        /// 和标识一起插入,适用有主键没有自动编号的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool InsertWithKeyField<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return InsertWithKeyField(connection, entityToInsert, null, transaction, commandTimeout);
        }



        /// <summary>
        /// 和标识一起插入,适用有主键没有自动编号的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool InsertWithKeyField<T>(this IDbConnection connection, T entityToInsert, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {

            var type = typeof(T);

            var name = String.IsNullOrWhiteSpace(tableName) ? GetTableName(type) : tableName;

            var sbColumnList = new StringBuilder(null);

            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);
            //var allPropertiesExceptKey = allProperties.Except(keyProperties);

            for (var i = 0; i < allProperties.Count(); i++)
            {
                var property = allProperties.ElementAt(i);
                sbColumnList.Append(property.Name);
                if (i < allProperties.Count() - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allProperties.Count(); i++)
            {
                var property = allProperties.ElementAt(i);
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allProperties.Count() - 1)
                    sbParameterList.Append(", ");
            }
            ISqlAdapter adapter = new SqlSmanServerAdapter();
            int count = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(), sbParameterList.ToString(), keyProperties, entityToInsert);
            return count > 0;
        }



        /// <summary>
        /// 按主键+sql条件更新
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout">d</param>
        /// <param name="propertys">用lambda指定要更新的字段</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static bool Update<T>(this IDbConnection connection, T entityToUpdate, string sqlCondition = null, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] propertys) where T : class
        {
            var proxy = entityToUpdate as Dapper.Contrib.Extensions.SqlMapperExtensions.IProxy;
            if (proxy != null)
            {
                if (!proxy.IsDirty) return false;
            }


            var type = typeof(T);

            var keyProperties = KeyPropertiesCache(type);
            if (!keyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] property");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);


            //var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a));

            IEnumerable<string> updateFields;
            if (propertys.Length < 1)
                updateFields = allProperties.Where(a => !keyProperties.Contains(a)).Select(a => a.Name);
            else
                updateFields = getFields(propertys).Except(keyProperties.Select(a => a.Name));

            for (var i = 0; i < updateFields.Count(); i++)
            {
                var property = updateFields.ElementAt(i);

                sb.AppendFormat("{0} = @{1}", property, property);
                if (i < updateFields.Count() - 1)
                    sb.AppendFormat(", ");

            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat("{0} = @{1}", property.Name, property.Name);
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }
            if (!String.IsNullOrEmpty(sqlCondition))
                sb.Append(" and " + sqlCondition);
            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }
        /// <summary>
        /// 按指定条件更新指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="sqlCondition"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="propertys">要更新的字段</param>
        /// <returns></returns>
        public static int UpdateByCondition<T>(this IDbConnection connection, T entityToUpdate, string sqlCondition, dynamic param, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] propertys) where T : class
        {
            if (String.IsNullOrEmpty(sqlCondition))
                throw new Exception("更新条件不能为空");

            var type = typeof(T);

            var keyProperties = KeyPropertiesCache(type);
            if (!keyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] property");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("declare @__x int;update {0} set @__x=1", name);

            var allProperties = TypePropertiesCache(type).ToList();
            var updateFields = getFields(propertys).ToList();
            var parms = new DynamicParameters(param);
            for (var i = 0; i < allProperties.Count; i++)
            {
                var property = allProperties[i];
                if ((!keyProperties.Contains(property) || updateFields.Contains(property.Name)) && (updateFields.Count == 0 || updateFields.Contains(property.Name)))
                {
                    sb.AppendFormat(",{0} = @{1}", property.Name, property.Name);

                }
                var value = property.GetValue(entityToUpdate, null);
                if (typeof(DateTime).IsAssignableFrom(property.PropertyType) && DateTime.MinValue.Equals(value))
                {
                    //跳过这个值算了
                    value = new DateTime(1900, 1, 1);
                }
                else
                {
                    parms.Add(property.Name, value);
                }


            }

            ////var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a));
            //IEnumerable<string> allFields = allProperties.Where(a => !keyProperties.Contains(a)).Select(a => a.Name);
            //if(propertys.Length<1)
            //    updateFields = allProperties.Where(a => !keyProperties.Contains(a)).Select(a=>a.Name);
            //else
            //    updateFields = getFields(propertys).Except(keyProperties.Select(a => a.Name));

            //for (var i = 0; i < updateFields.Count(); i++)
            //{
            //    var property = updateFields.ElementAt(i);

            //    sb.AppendFormat("{0} = @{1}", property, property);
            //    if (i < updateFields.Count() - 1)
            //        sb.AppendFormat(", ");

            //}
            sb.Append(" where ");
            /*
            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat("{0} = @{1}", property.Name, property.Name);
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }
            if (!String.IsNullOrEmpty(sqlCondition))
                */
            sb.Append(sqlCondition);
            var updated = connection.Execute(sb.ToString(), parms, commandTimeout: commandTimeout, transaction: transaction);
            return updated;
        }
        /// <summary>
        /// 按主键+sql条件更新 并排除相应字段
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="sqlCondition">附加查询条件</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout">d</param>
        /// <param name="excludePropertys">用lambda指定要排除更新的字段</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static bool UpdateExclude<T>(this IDbConnection connection, T entityToUpdate, string sqlCondition = null, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] excludePropertys) where T : class
        {
            var proxy = entityToUpdate as Dapper.Contrib.Extensions.SqlMapperExtensions.IProxy;
            if (proxy != null)
            {
                if (!proxy.IsDirty) return false;
            }


            var type = typeof(T);

            var keyProperties = KeyPropertiesCache(type);
            if (!keyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] property");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);


            var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a)).Select(a => a.Name);

            var updateFields = nonIdProps.Except(getFields(excludePropertys));
            for (var i = 0; i < updateFields.Count(); i++)
            {
                var property = updateFields.ElementAt(i);

                sb.AppendFormat("{0} = @{1}", property, property);
                if (i < updateFields.Count() - 1)
                    sb.AppendFormat(", ");

            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat("{0} = @{1}", property.Name, property.Name);
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }
            if (!String.IsNullOrEmpty(sqlCondition))
                sb.Append(" and " + sqlCondition);
            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }
        /// <summary>
        /// 按指定条件更新,排除excludePropertys里的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="sqlCondition">条件</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="excludePropertys">要排除更新的字段</param>
        /// <returns></returns>
        public static int UpdateExcludeByCondition<T>(this IDbConnection connection, T entityToUpdate, string sqlCondition, dynamic param, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] excludePropertys) where T : class
        {
            if (String.IsNullOrEmpty(sqlCondition))
                throw new Exception("更新条件不能为空");

            var type = typeof(T);

            var keyProperties = KeyPropertiesCache(type);
            if (!keyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] property");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("declare @__x int;update {0} set @__x=1", name);

            var allProperties = TypePropertiesCache(type).ToList();
            var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a)).Select(a => a.Name);

            var updateFields = nonIdProps.Except(getFields(excludePropertys)).ToList();
            var parms = new DynamicParameters(param);
            for (var i = 0; i < allProperties.Count; i++)
            {
                var property = allProperties[i];
                if ((!keyProperties.Contains(property) || updateFields.Contains(property.Name)) && (updateFields.Count == 0 || updateFields.Contains(property.Name)))
                {
                    sb.AppendFormat(",{0} = @{1}", property.Name, property.Name);

                }
                var value = property.GetValue(entityToUpdate, null);
                if (typeof(DateTime).IsAssignableFrom(property.PropertyType) && DateTime.MinValue.Equals(value))
                {
                    //跳过这个值算了
                    value = new DateTime(1900, 1, 1);
                }
                else
                {
                    parms.Add(property.Name, value);
                }
            }

            //for (var i = 0; i < updateFields.Count(); i++)
            //{
            //    var property = updateFields.ElementAt(i);

            //    sb.AppendFormat("{0} = @{1}", property, property);
            //    if (i < updateFields.Count() - 1)
            //        sb.AppendFormat(", ");

            //}
            sb.Append(" where ");
            /*
            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat("{0} = @{1}", property.Name, property.Name);
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }*/
            //if (!String.IsNullOrEmpty(sqlCondition))
            sb.Append(sqlCondition);
            var updated = connection.Execute(sb.ToString(), parms, commandTimeout: commandTimeout, transaction: transaction);
            return updated;
        }
        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id.
        /// </summary>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="excludePropertys">用lambda指定要排除更新的字段</param>
        /// <returns>Identity of inserted entity</returns>
        public static long InsertExclude<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null, params Expression<Func<T, object>>[] excludePropertys) where T : class
        {
            var type = typeof(T);

            var name = GetTableName(type);

            var sbColumnList = new StringBuilder(null);

            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);

            var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a)).Select(a => a.Name);

            var updateFields = nonIdProps.Except(getFields(excludePropertys));

            for (var i = 0; i < updateFields.Count(); i++)
            {
                sbColumnList.Append(updateFields.ElementAt(i));
                if (i < updateFields.Count() - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < updateFields.Count(); i++)
            {
                var property = updateFields.ElementAt(i);
                sbParameterList.AppendFormat("@{0}", property);
                if (i < updateFields.Count() - 1)
                    sbParameterList.Append(", ");
            }
            ISqlAdapter adapter = GetFormatter(connection);
            int id = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(), sbParameterList.ToString(), keyProperties, entityToInsert);
            return id;
        }

        public static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            string name = connection.GetType().Name.ToLower();
            if (!AdapterDictionary.ContainsKey(name))
                return new SqlServerAdapter();
            return AdapterDictionary[name];
        }

        private static IEnumerable<string> getFields<T>(Expression<Func<T, object>>[] propertys)
        {
            foreach (var x in propertys)
            {
                var y = x.Body.RemoveConvert() as System.Linq.Expressions.MemberExpression;
                if (y == null)
                    throw new Exception("不支持的表达式 “" + x.Body + "”");
                yield return y.Member.Name;
            }
        }
        /// <summary>
        /// 按条件删除信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="wherelist"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int DeleteByCondition<T>(this IDbConnection connection, string wherelist, object param, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var type = typeof(T);

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("delete from {0} where {1}", name, wherelist);

            var deleted = connection.Execute(sb.ToString(), param, transaction: transaction, commandTimeout: commandTimeout);
            return deleted;
        }

        /// <summary>
        /// 多对多关系子集组合
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="reader"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="addChildren"></param>
        /// <returns></returns>
        public static IEnumerable<TFirst> Map<TFirst, TSecond, TKey>
            (
            this Dapper.SqlMapper.GridReader reader,
            Func<TFirst, TKey> firstKey,
            Func<TSecond, TKey> secondKey,
            Action<TFirst, IEnumerable<TSecond>> addChildren
            )
        {
            var first = reader.Read<TFirst>().ToList();
            var childMap = reader
                .Read<TSecond>()
                .GroupBy(s => secondKey(s))
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in first)
            {
                IEnumerable<TSecond> children;
                if (childMap.TryGetValue(firstKey(item), out children))
                {
                    addChildren(item, children);
                }
            }

            return first;
        }

        public static IEnumerable<TOne> QueryMany<TOne, TMany>(this IDbConnection cnn, string sql, Func<TOne, IList<TMany>> property, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TMany, TOne>(sql, (one, many) =>
            {
                var c = one.GetHashCode();
                if (!cache.ContainsKey(c))
                    cache.Add(c, one);

                var localOne = cache[c];
                var list = property(localOne);
                list.Add(many);
                return localOne;
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

        public static IEnumerable<TOne> QueryMany<TOne, TMany>(this IDbConnection cnn, string sql, Func<TOne, IList<TMany>> property, Func<TOne, dynamic> primarykey, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TMany, TOne>(sql, (one, many) =>
            {
                var c = primarykey(one).GetHashCode();
                if (!cache.ContainsKey(c))
                    cache.Add(c, one);

                var localOne = cache[c];
                var list = property(localOne);
                if (many != null)
                {
                    list.Add(many);
                }
                return localOne;
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

        /// <summary>
        /// TOne分别于TTwo和TMany关联
        /// </summary>
        /// <typeparam name="TOne"></typeparam>
        /// <typeparam name="TTwo"></typeparam>
        /// <typeparam name="TMany"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="property"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<TOne> QueryMany<TOne, TTwo, TMany>(this IDbConnection cnn, string sql,
                                                                     Func<TOne, IList<TMany>> property,
                                                                     Func<TOne, TTwo, TOne> map, dynamic param = null,
                                                                     IDbTransaction transaction = null,
                                                                     bool buffered = true, string splitOn = "Id",
                                                                     int? commandTimeout = null,
                                                                     CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TTwo, TMany, TOne>(sql, (one, two, many) =>
            {
                var c = one.GetHashCode();
                if (!cache.ContainsKey(c))
                    cache.Add(c, one);

                var localOne = cache[c];
                var list = property(localOne);
                list.Add(many);
                return map(localOne, two);
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }


        /// <summary>
        /// TOne分别于TTwo和TMany关联
        /// </summary>
        /// <typeparam name="TOne"></typeparam>
        /// <typeparam name="TTwo"></typeparam>
        /// <typeparam name="TMany"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="property"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<TOne> QueryMany<TOne, TTwo, TMany>(this IDbConnection cnn, string sql, Func<TOne, IList<TMany>> property, Func<TOne, dynamic> primarykey, Func<TOne, TTwo, TOne> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TTwo, TMany, TOne>(sql, (one, two, many) =>
            {
                var hashcode = primarykey(one).GetHashCode();
                if (!cache.ContainsKey(hashcode))
                    cache.Add(hashcode, one);

                var localOne = cache[hashcode];
                var list = property(localOne);
                list.Add(many);
                return map(localOne, two);
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }


        /// <summary>
        /// TOne分别于TTwo,TThree和TMany关联
        /// </summary>
        /// <typeparam name="TOne"></typeparam>
        /// <typeparam name="TTwo"></typeparam>
        /// <typeparam name="TMany"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="property"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<TOne> QueryMany<TOne, TTwo, TThree, TMany>(this IDbConnection cnn, string sql, Func<TOne, IList<TMany>> property, Func<TOne, dynamic> primarykey, Func<TOne, TTwo, TThree, TOne> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TTwo, TThree, TMany, TOne>(sql, (one, two, three, many) =>
            {
                var hashcode = primarykey(one).GetHashCode();
                if (!cache.ContainsKey(hashcode))
                    cache.Add(hashcode, one);

                var localOne = cache[hashcode];
                var list = property(localOne);
                list.Add(many);
                return map(localOne, two, three);
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

        /// <summary>
        /// TOne和TTwo无直接关联，通过TMany多对多关系
        /// </summary>
        /// <typeparam name="TOne"></typeparam>
        /// <typeparam name="TTwo"></typeparam>
        /// <typeparam name="TMany"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="property"></param>
        /// <param name="hashcode">唯一值</param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<TOne> QueryMany<TOne, TTwo, TMany>(this IDbConnection cnn, string sql, Func<TOne, IList<TMany>> property, Func<TOne, int> hashcode, Func<TTwo, TMany, TMany> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<int, TOne>();
            cnn.Query<TOne, TTwo, TMany, TOne>(sql, (one, two, many) =>
            {
                if (!cache.ContainsKey(hashcode(one)))
                    cache.Add(hashcode(one), one);

                var localOne = cache[hashcode(one)];
                if (many != null)
                {
                    many = map(two, many);
                    var list = property(localOne);
                    list.Add(many);
                }
                return localOne;
            }, param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

    }




    /// <summary>
    /// 插入不返回标识
    /// </summary>
    public class SqlSmanServerAdapter : ISqlAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="tableName"></param>
        /// <param name="columnList"></param>
        /// <param name="parameterList"></param>
        /// <param name="keyProperties"></param>
        /// <param name="entityToInsert"></param>
        /// <returns></returns>
        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            string cmd = String.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

            return connection.Execute(cmd, entityToInsert, transaction: transaction, commandTimeout: commandTimeout);

        }
    }
}
