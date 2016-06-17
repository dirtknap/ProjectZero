using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectZero.Database.Extensions.Tests
{
    [TestClass]
    public class DbTableTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void DbTable_Insert()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildInsertQuery(new TestDto {Airspeed = 7, Name = "Sir Robin", Quest = "Grail"}, out parameters);

            Assert.AreEqual("INSERT INTO [TestTable] ([Name],[Quest],[Airspeed]) VALUES (@Name,@Quest,@Airspeed)", result);
            Assert.AreEqual("@Name,@Quest,@Airspeed", string.Join(",", parameters.Keys.Select(v => v.ToString())));
            Assert.AreEqual("Sir Robin,Grail,7", string.Join(",", parameters.Values.Select(v => v.ToString())));
        }

        [TestMethod]
        public void DbTable_SelectOne()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildSelectQuery(new TestDto(), new List<int> {5}, null, out parameters);

            Assert.AreEqual("SELECT [Id],[Name],[Quest],[Airspeed] FROM [TestTable] WHERE [Id] = @Id", result);
            Assert.AreEqual("@Id", string.Join(",", parameters.Keys.Select(v => v.ToString())));
            Assert.AreEqual("5", string.Join(",", parameters.Values.Select(v => v.ToString())));
        }

        [TestMethod]
        public void DbTable_SelectFromList()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildSelectQuery(new TestDto(), new List<int> {1, 5, 7}, null, out parameters);

            Assert.AreEqual("SELECT [Id],[Name],[Quest],[Airspeed] FROM [TestTable] WHERE [Id] IN (@Id_1,@Id_2,@Id_3)", result);
            Assert.AreEqual("@Id_1,@Id_2,@Id_3", string.Join(",", parameters.Keys.Select(v => v.ToString())));
            Assert.AreEqual("1,5,7", string.Join(",", parameters.Values.Select(v => v.ToString())));
        }

        [TestMethod]
        public void DbTable_SelectTopN()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildSelectQuery(new TestDto(), new List<int>(), 10, out parameters);

            Assert.AreEqual("SELECT TOP 10 [Id],[Name],[Quest],[Airspeed] FROM [TestTable] ORDER BY [Id] DESC", result);
            Assert.IsTrue(parameters.Keys.Count == 0);
            Assert.IsTrue(parameters.Values.Count == 0);
        }

        [TestMethod]
        public void DbTable_SelectAll()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildSelectQuery(new TestDto(), new List<int>(), null, out parameters);

            Assert.AreEqual("SELECT [Id],[Name],[Quest],[Airspeed] FROM [TestTable]", result);
            Assert.IsTrue(parameters.Keys.Count == 0);
            Assert.IsTrue(parameters.Values.Count == 0);
        }

        [TestMethod]
        public void DbTable_Update()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildUpdateQuery(new TestDto {Id = 9, Airspeed = 7, Name = "Sir Robin", Quest = "Grail" }, out parameters);

            Assert.AreEqual("UPDATE [TestTable] SET [Name]=@Name,[Quest]=@Quest,[Airspeed]=@Airspeed WHERE [Id]=@Id", result);
            Assert.AreEqual("@Id,@Name,@Quest,@Airspeed", string.Join(",", parameters.Keys.Select(v => v.ToString())));
            Assert.AreEqual("9,Sir Robin,Grail,7", string.Join(",", parameters.Values.Select(v => v.ToString())));
        }

        [TestMethod]
        public void DbTable_Delete()
        {
            var parameters = new Dictionary<string, object>();

            var result = DbTable.DoBuildDeleteQuery(new TestDto {Id = 7}, out parameters);

            Assert.AreEqual("DELETE FROM [TestTable] WHERE [Id]=@Id", result);
            Assert.IsTrue((int)parameters["@Id"] == 7);
        }

    }

    [Table("TestTable")]
    internal class TestDto
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }

        [TableField("Name")]
        public string Name { get; set; }
        
        [TableField("Quest")]
        public string Quest { get; set; }

        [TableField("Airspeed")]
        public int Airspeed { get; set; }
    }
}
