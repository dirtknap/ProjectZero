﻿using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectZero.Database.Dal.Tables
{
    public interface ISimpleCrudDal<T>
    {
        int Insert(T item, SqlTransaction txn = null);
        T Get(int id, SqlTransaction txn = null);
        List<T> GetAll(SqlTransaction txn = null);
        List<T> GetSelected(List<int> idList, SqlTransaction txn = null);
        List<T> GetNewestN(int number, SqlTransaction txn = null);
        void Update(T item, SqlTransaction txn = null);
        void Delete(int id, SqlTransaction txn = null);
    }
}