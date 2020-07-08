using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoSql.MongoDB
{
    public class MongoDBContext
    {
        private readonly IMongoClient _client = null;
        private readonly IMongoDatabase _database = null;

        public MongoDBContext(string mongoConnectionString, string databaseName)
        {
            _client = new MongoClient(mongoConnectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public BsonDocument RunCommand(string cmdText)
        {
            return _database.RunCommand<BsonDocument>(cmdText);
        }

        public IList<BsonDocument> GetDatabase()
        {
            return _client.ListDatabases().ToList();
        }

        #region SELECT
        /// <summary>
        /// 判断文档存在状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <returns></returns>
        public bool IsExistDocument<T>(string documentname, FilterDefinition<T> filter)
        {
            return _database.GetCollection<T>(documentname).CountDocuments(filter) > 0;
        }

        /// <summary>
        /// 通过条件得到查询的结果个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long GetCount<T>(string documentname, FilterDefinition<T> filter)
        {
            return _database.GetCollection<T>(documentname).CountDocuments(filter);
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var result = _database.GetCollection<T>(documentname).Find(filter);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// 通过指定字段获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentByCol<T>(string documentname, string columnName, string columnvalue)
        {
            ObjectId value = ObjectId.Parse(columnvalue);
            var filter = Builders<T>.Filter.Eq(columnName, value);
            var result = _database.GetCollection<T>(documentname).Find(filter);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string documentname, FilterDefinition<T> filter)
        {
            var result = _database.GetCollection<T>(documentname).Find(filter);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string documentname, string id, ProjectionDefinition<T> fields)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            return _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }

        /// <summary>
        /// 通过指定的条件获取一个对象，如果有多条，只取第一条，同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetDocumentByUserFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }

        /// <summary>
        /// 获取全部文档
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="documentname"></param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname)
        {
            var filter = Builders<T>.Filter.Empty;
            return _database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 获取全部文档
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="documentname"></param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname, FilterDefinition<T> filter)
        {
            return _database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 获取全部文档同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname, ProjectionDefinition<T> fields)
        {
            var filter = Builders<T>.Filter.Empty;
            return _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过一个条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return _database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 通过条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter)
        {
            return _database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 通过条件获取对象,同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value, ProjectionDefinition<T> fields)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过条件获取对象,同时过滤数据和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter">过滤器</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilterMaxCount<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields, SortDefinition<T> sort, int pageIndex, int pageSize, int maxPage)
        {
            IList<T> result = new List<T>();
            result = _database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Skip(maxPage).Limit(1).ToList();
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = _database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = _database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = _database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }
        #endregion

        #region INSERT

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public void Insert<T>(string documentName, T document)
        {
            try
            {
                _database.GetCollection<T>(documentName).InsertOne(document);
            }
            catch (MongoWriteException me)
            {
                MongoBulkWriteException mbe = me.InnerException as MongoBulkWriteException;
                if (mbe != null && mbe.HResult == -2146233088)
                    throw new Exception("插入重复的键！");
                throw new Exception(mbe.Message);
            }
            catch (Exception ep)
            {
                throw ep;
            }
        }

        /// <summary>
        /// 新增多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public void InsertMany<T>(string documentname, IList<T> documents)
        {
            try
            {
                _database.GetCollection<T>(documentname).InsertMany(documents);
            }
            catch (MongoWriteException me)
            {
                MongoBulkWriteException mbe = me.InnerException as MongoBulkWriteException;
                if (mbe != null && mbe.HResult == -2146233088)
                    throw new Exception("插入重复的键！");
                throw new Exception(mbe.Message);
            }
            catch (Exception ep)
            {
                throw ep;
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <param name="id"></param>
        /// <param name="oldinfo"></param>
        /// <returns></returns>
        public void UpdateReplaceOne<T>(string documentname, string id, T oldinfo)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            _database.GetCollection<T>(documentname).ReplaceOne(filter, oldinfo);
        }

        /// <summary>
        /// 只能替换一条，如果有多条的话
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="oldinfo"></param>
        public void UpdateReplaceOne<T>(string documentname, FilterDefinition<T> filter, T oldinfo)
        {
            _database.GetCollection<T>(documentname).ReplaceOne(filter, oldinfo);
        }

        /// <summary>
        /// 更新指定属性值，按ID就只有一条，替换一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="setvalue"></param>
        /// <returns></returns>
        public void Update<T>(string documentname, string id, string property, string value)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var update = Builders<T>.Update.Set(property, value);
            _database.GetCollection<T>(documentname).UpdateOne(filter, update);
        }

        public void Update<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            _database.GetCollection<T>(documentname).UpdateOne(filter, update);
        }

        public void UpdateMany<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            _database.GetCollection<T>(documentname).UpdateMany(filter, update);
        }

        #endregion

        #region DELETE
        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filterid = Builders<T>.Filter.Eq("_id", oid);
            _database.GetCollection<T>(documentname).DeleteOne(filterid);
        }

        public void Delete<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            _database.GetCollection<T>(documentname).DeleteOne(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteMany<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            _database.GetCollection<T>(documentname).DeleteMany(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public void DeleteMany<T>(string documentname, FilterDefinition<T> filter)
        {
            _database.GetCollection<T>(documentname).DeleteMany(filter);
        }
        #endregion

    }
}
