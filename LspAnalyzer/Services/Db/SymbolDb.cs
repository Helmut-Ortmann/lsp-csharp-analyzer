using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using DataModels.Symbols;
using LinqToDB;
using LinqToDB.DataProvider;
using LspDb.Linq2sql.LinqUtils;

namespace LspAnalyzer.Services.Db
{
    public class SymbolDb
    {
        readonly IDataProvider _dbProvider;
        private string _dbPath;
        private readonly string _connectionString;
        public SymbolDb(string dbPath)
        {
            _dbPath = dbPath;
            _connectionString = LinqUtil.GetConnectionString(dbPath, out _dbProvider);
           

        }

        public bool Create()
        {
            // Delete and create database
            SQLiteConnection.CreateFile(_dbPath);
            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            List<string> lSql = new List<string> {
                @"CREATE TABLE code_item_kinds ( id integer NOT NULL CONSTRAINT pk_code_item_kinds_id PRIMARY KEY, name text NOT NULL)",
                @"CREATE TABLE files ( id bigint NOT NULL CONSTRAINT pk_files_id PRIMARY KEY, timestamp bigint NOT NULL, name text NOT NULL, leaf_name text NOT NULL)",
                @"CREATE TABLE code_items ( id bigint NOT NULL CONSTRAINT pk_code_items_id PRIMARY KEY, file_id bigint NOT NULL, parent_id bigint NOT NULL, kind integer NOT NULL,
		name text NOT NULL, type text, start_column integer NOT NULL, start_line integer NOT NULL, end_column integer NOT NULL, end_line integer NOT NULL, 
		name_start_column integer NOT NULL, name_start_line integer NOT NULL, name_end_column integer NOT NULL, name_end_line integer NOT NULL)"
		
            };
            foreach (var sql in lSql)
            {
                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();

            // accessing with LINQ to SQL
            using (var db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))

            {
                db.Insert(new CodeItemKinds {Id = 1, Name = "File"});
                db.Insert(new CodeItemKinds {Id = 2, Name = "Module"});
                db.Insert(new CodeItemKinds {Id = 3, Name = "Namespace"});
                db.Insert(new CodeItemKinds {Id = 4, Name = "Package"});
                db.Insert(new CodeItemKinds {Id = 5, Name = "Class"});
                db.Insert(new CodeItemKinds {Id = 6, Name = "Method"});
                db.Insert(new CodeItemKinds {Id = 7, Name = "Property"});
                db.Insert(new CodeItemKinds {Id = 8, Name = "Field"});
                db.Insert(new CodeItemKinds {Id = 9, Name = "Constructor"});
                db.Insert(new CodeItemKinds {Id = 10, Name = "Enum"});
                db.Insert(new CodeItemKinds {Id = 11, Name = "Method"});
                db.Insert(new CodeItemKinds {Id = 12, Name = "Function"});
                db.Insert(new CodeItemKinds {Id = 13, Name = "Variable"});
                db.Insert(new CodeItemKinds {Id = 14, Name = "Constant"});
                db.Insert(new CodeItemKinds {Id = 15, Name = "String"});
                db.Insert(new CodeItemKinds {Id = 16, Name = "Number"});
                db.Insert(new CodeItemKinds {Id = 17, Name = "Boolean"});
                db.Insert(new CodeItemKinds {Id = 18, Name = "Array"});
                db.Insert(new CodeItemKinds {Id = 19, Name = "Object"});
                db.Insert(new CodeItemKinds {Id = 20, Name = "Key"});
                db.Insert(new CodeItemKinds {Id = 21, Name = "Null"});
                db.Insert(new CodeItemKinds {Id = 22, Name = "EnumMember"});
                db.Insert(new CodeItemKinds {Id = 23, Name = @"Struct"});
                db.Insert(new CodeItemKinds {Id = 24, Name = "Event"});
                db.Insert(new CodeItemKinds {Id = 25, Name = "Operator"});
                db.Insert(new CodeItemKinds {Id = 26, Name = "TypeParameter"});
                db.Insert(new CodeItemKinds {Id = 26, Name = "TypeParameter"});
            }

            return true;
        }

        public void Load(string dbPath, DataTable dt)
        {
            

            // create all files

        }
    }
}
