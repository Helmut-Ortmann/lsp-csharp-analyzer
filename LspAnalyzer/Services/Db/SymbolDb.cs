using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModels.Symbols;
using LinqToDB;
using LinqToDB.DataProvider;
using LspDb.Linq2sql.LinqUtils;
using OmniSharp.Extensions.LanguageServer.Client;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using File = DataModels.Symbols.File;

namespace LspAnalyzer.Services.Db
{
    public class SymbolDb
    {
        private readonly LanguageClient _client;
        readonly IDataProvider _dbProvider;
        private readonly string _dbPath;
        private readonly string _connectionString;
        // Performance optimization
        private Dictionary<int,string> _dictKind = new Dictionary<int,string>();
        private readonly Dictionary<string, int> _dictFile = new Dictionary<string,int>();

        public SymbolDb(string dbPath, LanguageClient client)
        {
            _dbPath = dbPath;
            _client = client;
            _connectionString = LinqUtil.GetConnectionString(dbPath, out _dbProvider);
           

        }
        /// <summary>
        /// Create Database
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            // Delete Symbol database
            DeleteOldDatabase();
 

            //// accessing with LINQ to SQL
            using (var db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))

            { 
                var sp = db.DataProvider.GetSchemaProvider();
                var dbSchema = sp.GetSchema(db);
                try
                {
                    db.BeginTransaction();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"SQLite locked?\r\n\r\n{e}", "Can't begin transaction");
                    return false;
                }
               
                if (dbSchema.Tables.All(t => t.TableName != "code_item_kinds"))
                {
                    db.CreateTable<CodeItemKinds>();
                }

                if (dbSchema.Tables.All(t => t.TableName != "files"))
                {
                    db.CreateTable<File>();
                }

                if (dbSchema.Tables.All(t => t.TableName != "code_items"))
                {
                    db.CreateTable<CodeItems>();
                }
                if (dbSchema.Tables.All(t => t.TableName != "code_item_usages"))
                {
                    db.CreateTable<CodeItemUsages>();
                }

                db.GetTable<CodeItemKinds>()
                    .Delete();
                db.Insert(new CodeItemKinds { Id = 1, Name = "File" });
                db.Insert(new CodeItemKinds { Id = 2, Name = "Module" });
                db.Insert(new CodeItemKinds { Id = 3, Name = "Namespace" });
                db.Insert(new CodeItemKinds { Id = 4, Name = "Package" });
                db.Insert(new CodeItemKinds { Id = 5, Name = "Class" });
                db.Insert(new CodeItemKinds { Id = 6, Name = "Method" });
                db.Insert(new CodeItemKinds { Id = 7, Name = "Property" });
                db.Insert(new CodeItemKinds { Id = 8, Name = "Field" });
                db.Insert(new CodeItemKinds { Id = 9, Name = "Constructor" });
                db.Insert(new CodeItemKinds { Id = 10, Name = "Enum" });
                db.Insert(new CodeItemKinds { Id = 11, Name = "Method" });
                db.Insert(new CodeItemKinds { Id = 12, Name = "Function" });
                db.Insert(new CodeItemKinds { Id = 13, Name = "Variable" });
                db.Insert(new CodeItemKinds { Id = 14, Name = "Constant" });
                db.Insert(new CodeItemKinds { Id = 15, Name = "String" });
                db.Insert(new CodeItemKinds { Id = 16, Name = "Number" });
                db.Insert(new CodeItemKinds { Id = 17, Name = "Boolean" });
                db.Insert(new CodeItemKinds { Id = 18, Name = "Array" });
                db.Insert(new CodeItemKinds { Id = 19, Name = "Object" });
                db.Insert(new CodeItemKinds { Id = 20, Name = "Key" });
                db.Insert(new CodeItemKinds { Id = 21, Name = "Null" });
                db.Insert(new CodeItemKinds { Id = 22, Name = "EnumMember" });
                db.Insert(new CodeItemKinds { Id = 23, Name = @"Struct" });
                db.Insert(new CodeItemKinds { Id = 24, Name = "Event" });
                db.Insert(new CodeItemKinds { Id = 25, Name = "Operator" });
                db.Insert(new CodeItemKinds { Id = 26, Name = "TypeParameter" });


                db.Insert(new CodeItemKinds { Id = 27, Name = "Type27" });
                db.Insert(new CodeItemKinds { Id = 28, Name = "Type28" });
                db.Insert(new CodeItemKinds { Id = 29, Name = "Type29" });

                db.Insert(new CodeItemKinds { Id = 30, Name = "Type30" });
                db.Insert(new CodeItemKinds { Id = 31, Name = "Type31" });
                db.Insert(new CodeItemKinds { Id = 32, Name = "Type32" });
                db.Insert(new CodeItemKinds { Id = 33, Name = "Type33" });
                db.Insert(new CodeItemKinds { Id = 34, Name = "Type34" });
                db.Insert(new CodeItemKinds { Id = 35, Name = "Type35" });
                db.Insert(new CodeItemKinds { Id = 36, Name = "Type36" });
                db.Insert(new CodeItemKinds { Id = 37, Name = "Type37" });
                db.Insert(new CodeItemKinds { Id = 38, Name = "Type38" });
                db.Insert(new CodeItemKinds { Id = 39, Name = "Type39" });

                db.Insert(new CodeItemKinds { Id = 40, Name = "Type40" });
                db.Insert(new CodeItemKinds { Id = 41, Name = "Type41" });
                db.Insert(new CodeItemKinds { Id = 42, Name = "Type42" });
                db.Insert(new CodeItemKinds { Id = 43, Name = "Type43" });
                db.Insert(new CodeItemKinds { Id = 44, Name = "Type44" });
                db.Insert(new CodeItemKinds { Id = 45, Name = "Type45" });
                db.Insert(new CodeItemKinds { Id = 46, Name = "Type46" });
                db.Insert(new CodeItemKinds { Id = 47, Name = "Type47" });
                db.Insert(new CodeItemKinds { Id = 48, Name = "Type48" });
                db.Insert(new CodeItemKinds { Id = 49, Name = "Type49" });

                db.Insert(new CodeItemKinds { Id = 50, Name = "Type50" });
                db.Insert(new CodeItemKinds { Id = 51, Name = "Type51" });
                db.Insert(new CodeItemKinds { Id = 52, Name = "Type52" });
                db.Insert(new CodeItemKinds { Id = 53, Name = "Type53" });
                db.Insert(new CodeItemKinds { Id = 54, Name = "Type54" });
                db.Insert(new CodeItemKinds { Id = 55, Name = "Type55" });
                db.Insert(new CodeItemKinds { Id = 56, Name = "Type56" });
                db.Insert(new CodeItemKinds { Id = 57, Name = "Type57" });
                db.Insert(new CodeItemKinds { Id = 58, Name = "Type58" });
                db.Insert(new CodeItemKinds { Id = 59, Name = "Type59" });





                db.Insert(new CodeItemKinds { Id = 200, Name = "Type200" });
                db.Insert(new CodeItemKinds { Id = 201, Name = "Type201" });
                db.Insert(new CodeItemKinds { Id = 202, Name = "Type202" });
                db.Insert(new CodeItemKinds { Id = 203, Name = "Type203" });
                db.Insert(new CodeItemKinds { Id = 204, Name = "Type204" });
                db.Insert(new CodeItemKinds { Id = 205, Name = "Type205" });
                db.Insert(new CodeItemKinds { Id = 206, Name = "Type206" });
                db.Insert(new CodeItemKinds { Id = 207, Name = "Type207" });
                db.Insert(new CodeItemKinds { Id = 208, Name = "Type208" });
                db.Insert(new CodeItemKinds { Id = 209, Name = "Type209" });

                db.Insert(new CodeItemKinds { Id = 210, Name = "Type210" });
                db.Insert(new CodeItemKinds { Id = 211, Name = "Type211" });
                db.Insert(new CodeItemKinds { Id = 212, Name = "Type212" });
                db.Insert(new CodeItemKinds { Id = 213, Name = "Type213" });
                db.Insert(new CodeItemKinds { Id = 214, Name = "Type214" });
                db.Insert(new CodeItemKinds { Id = 215, Name = "Type215" });
                db.Insert(new CodeItemKinds { Id = 216, Name = "Type216" });
                db.Insert(new CodeItemKinds { Id = 217, Name = "Type217" });
                db.Insert(new CodeItemKinds { Id = 218, Name = "Type218" });
                db.Insert(new CodeItemKinds { Id = 219, Name = "Type219" });
                
                
                
                db.Insert(new CodeItemKinds { Id = 220, Name = "Type220" });
                db.Insert(new CodeItemKinds { Id = 221, Name = "Type221" });
                db.Insert(new CodeItemKinds { Id = 222, Name = "Type222" });
                db.Insert(new CodeItemKinds { Id = 223, Name = "Type223" });
                db.Insert(new CodeItemKinds { Id = 224, Name = "Type224" });
                db.Insert(new CodeItemKinds { Id = 225, Name = "Type225" });
                db.Insert(new CodeItemKinds { Id = 226, Name = "Type226" });
                db.Insert(new CodeItemKinds { Id = 227, Name = "Type227" });
                db.Insert(new CodeItemKinds { Id = 228, Name = "Type228" });
                db.Insert(new CodeItemKinds { Id = 229, Name = "Type229" });

                db.Insert(new CodeItemKinds { Id = 230, Name = "Type230" });
                db.Insert(new CodeItemKinds { Id = 231, Name = "Type231" });
                db.Insert(new CodeItemKinds { Id = 232, Name = "Type232" });
                db.Insert(new CodeItemKinds { Id = 233, Name = "Type233" });
                db.Insert(new CodeItemKinds { Id = 234, Name = "Type234" });
                db.Insert(new CodeItemKinds { Id = 235, Name = "Type235" });
                db.Insert(new CodeItemKinds { Id = 236, Name = "Type236" });
                db.Insert(new CodeItemKinds { Id = 237, Name = "Type237" });
                db.Insert(new CodeItemKinds { Id = 238, Name = "Type238" });
                db.Insert(new CodeItemKinds { Id = 239, Name = "Type239" });

                db.Insert(new CodeItemKinds { Id = 240, Name = "Type240" });
                db.Insert(new CodeItemKinds { Id = 241, Name = "Type241" });
                db.Insert(new CodeItemKinds { Id = 242, Name = "Type242" });
                db.Insert(new CodeItemKinds { Id = 243, Name = "Type243" });
                db.Insert(new CodeItemKinds { Id = 244, Name = "Type244" });
                db.Insert(new CodeItemKinds { Id = 245, Name = "Type245" });
                db.Insert(new CodeItemKinds { Id = 246, Name = "Type246" });
                db.Insert(new CodeItemKinds { Id = 247, Name = "Type247" });
                db.Insert(new CodeItemKinds { Id = 248, Name = "Type248" });
                db.Insert(new CodeItemKinds { Id = 249, Name = "Type249" });

                db.Insert(new CodeItemKinds { Id = 250, Name = "Type250" });
                db.Insert(new CodeItemKinds { Id = 251, Name = "Type251" });
                db.Insert(new CodeItemKinds { Id = 252, Name = "Type252" });
                db.Insert(new CodeItemKinds { Id = 253, Name = "TypeAlias" });
                db.Insert(new CodeItemKinds { Id = 254, Name = "StaticMethod" });
                db.Insert(new CodeItemKinds { Id = 255, Name = "Macro" });

                MakeKindDictionary(db);

                db.GetTable<CodeItems>()
                    .Delete();
                db.GetTable<File>()
                    .Delete();
                db.GetTable<CodeItemUsages>()
                    .Delete();
                db.CommitTransaction();

            }

           return true;
        }

        /// <summary>
        /// Make kind dictionary
        /// </summary>
        /// <param name="db"></param>
        private void MakeKindDictionary(SYMBOLDB db)
        {
            var lKind = from f in db.CodeItemKinds
                select new {
                    Id=  f.Id,
                    Kind = f.Name
                };
            foreach (var k in lKind)
            {
                _dictKind.Add(k.Id,
                    k.Kind);
            }
        }
        /// <summary>
        /// Load files of workspace
        /// </summary>
        /// <param name="workspace"></param>
        public void LoadFiles(string workspace)
        {
            using (SYMBOLDB db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))
            {
                // delete all
                db
                    .GetTable<File>()
                    .Delete();

                var files = from f in Directory.GetFiles(workspace, "*.*", SearchOption.AllDirectories)
                    where Path.GetExtension(f).ToLower().StartsWith(".c") ||
                          Path.GetExtension(f).ToLower().StartsWith(".h")
                    select f.ToLower().Replace(@"\" , "/");

                db.BeginTransaction();

                foreach (var file in files)
                {
                    long fid = (long)db
                        .InsertWithIdentity(
                            new File
                            {
                                Name = file,
                                LeafName = Path.GetFileName(file)
                            });
                    _dictFile.Add(file,(int)fid );

                }
                db.CommitTransaction();
            }
        }

        /// <summary>
        /// Load items from data table
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dt"></param>
        public int LoadItems(string workspace, DataTable dt)
        {
            int count = 0;
            // create all files
            using (var db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))

            {

                // delete all items that are part of the input symbol table
                var itemsToDelete = (from DataRow g in dt.Rows
                    //join k in db.CodeItemKinds on g.Field<string>("Kind") equals k.Name
                    join i in db.CodeItems on g.Field<string>("Name").Trim() equals i.Name.Trim()
                    where g.Field<string>("Kind") != "File"
                    select new
                    {
                        Name = i.Name,
                        Id = i.Id
                    }).ToDataTable();

               // all symbols
                var items = from DataRow i in dt.Rows
                    join f in db.Files on Path.Combine(workspace.ToLower().Replace(@"\", "/"), i.Field<string>("File"))
                        .ToLower().Replace(@"\", "/") equals f.Name
                    join k in db.CodeItemKinds on i.Field<string>("Kind") equals k.Name
                    where i.Field<string>("Kind") != "File"
                    let position = LspAnalyzerHelper.GetSymbolStartPosition(
                        i.Field<string>("Intern"),
                        i.Field<string>("Name"),
                        new Position(i.Field<long>("StartLine"), i.Field<long>("StartChar")
                        ))

                    select new
                    {
                        Signature = i.Field<string>("Intern"),
                        Name = i.Field<string>("Name"),
                        Kind = k.Id,
                        StartLine = i.Field<long>("StartLine"),
                        EndLine = i.Field<long>("EndLine"),
                        StartChar = i.Field<long>("StartChar"),
                        EndChar = i.Field<long>("EndChar"),
                        NameStartLine = position.Line,
                        NameStartChar = position.Character,
                        FileId = f.Id
                    };
                db.BeginTransaction();

                foreach (var i in items)
                {
                    db.Insert<CodeItems>(new CodeItems
                    {
                        Name = i.Name,
                        Signature = i.Signature,
                        Kind = i.Kind,
                        StartLine = (int)i.StartLine,
                        StartColumn = (int)i.StartChar,
                        EndLine = (int)i.EndLine,
                        EndColumn = (int)i.EndChar,
                        NameStartLine = (int)i.NameStartLine,
                        NameStartColumn = (int)i.NameStartChar,
                        FileId = i.FileId

                    });
                    count += 1;
                }
                db.CommitTransaction();
            }

            return count;

        }

        /// <summary>
        /// Load function usage
        /// </summary>
        public async Task<int> LoadFunctionUsage()
        {

            // create all files
            int countUsages = 0;
            using (var db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))

            {
                var items = (from item in db.CodeItems
                    join file in db.Files on item.FileId equals file.Id
                    join kind in db.CodeItemKinds on item.Kind equals kind.Id
                    where kind.Name == "Function" || 
                          kind.Name == "Macro" || kind.Name == "Enum" || kind.Name == "Field" || kind.Name == "Field" ||kind.Name == "Constant" || kind.Name == "Property"
                    select new 
                    {
					    Id = item.Id,
                        ItemKind = kind.Name,
                        ItemName = item.Name,
                        FileName = file.Name,
                        NameStartLine = item.NameStartLine,
                        NameStartColumn = item.NameStartColumn
						
                    }).ToArray();
                // write to Database
                db.BeginTransaction();

                foreach (var f in items)
                {
                    string method = f.ItemKind == "Function" ? @"$cquery/callers" : @"$cquery/vars";
                    var locations = await _client.TextDocument.Cquery(method, f.FileName, f.NameStartLine, f.NameStartColumn);
                    foreach (var l in locations)
                    {


                        string path = l.Uri.LocalPath.Substring(1).ToLower();
                        // only consider files with path in workspace
                        if (f.FileName.Contains(path.ToLower()))
                        {
                            // ReSharper disable once UnusedVariable
                            // Check to use it in inser
                            if (_dictFile.TryGetValue(path, out int fileId) == false)
                            {
                                MessageBox.Show(
                                    $"path={path}\r\nmethod={method}\r\nkind={f.ItemKind}\r\nitem={f.ItemName}",
                                    "Cant find file id");
                                continue;
                            }

                            db.Insert<CodeItemUsages>(new CodeItemUsages
                            {
                                CodeItemId = f.Id,
                                FileId = _dictFile[path],
                                Signature = " ",
                                StartColumn = (int) l.Range.Start.Character,
                                StartLine = (int) l.Range.Start.Line,
                                EndColumn = (int) l.Range.End.Character,
                                EndLine = (int) l.Range.End.Line,


                            });
                            countUsages += 1;
                        }

                    }
                }
                db.CommitTransaction();

            }

            return countUsages;

        }
        /// <summary>
        /// Output metrics of Code
        /// </summary>
        public void Metrics() {

            using (var db = new DataModels.Symbols.SYMBOLDB(_dbProvider, _connectionString))
            {
                var itemMetrics = from func in db.CodeItems
                    join kind in db.CodeItemKinds on func.Kind equals kind.Id
                    orderby func.Name
                    group kind by kind.Name
                    into grp
                    select
                        $"{grp.Count(),7:N0}\t{grp.Max(x => x.Id)}\t{grp.Key}";
                        //Kind = grp.Key,
                        //Id = grp.Max(x => x.Id),
                        //Count = grp.Count()

                string part1 = String.Join("\r\n", itemMetrics);

                var itemUsageMetrics = from func in db.CodeItems
                    join k1 in db.CodeItemKinds on func.Kind equals k1.Id
                    join u in db.CodeItemUsages on func.Id equals u.CodeItemId
                    orderby func.Name
                    group k1 by k1.Name
                    into grp
                    select $"{grp.Count(),7:N0}\t{grp.Max(x => x.Id)}\t{grp.Key}";
                    //{
                    //    Kind = grp.Key,
                    //    Id = $"{grp.Max(x => x.Id)}",
                    //    Count = $"{grp.Count(),1:N0}"
                    //};
                string part2 = String.Join("\r\n", itemUsageMetrics);

                string text = $"  Kind\tId\tCount\r\n{part1}\r\n\r\n\t\tUsage Count\r\n{part2}";

                MessageBox.Show(text, "Code Metrics, see also Clipboard");
                Clipboard.SetText(text);

            }
        }
    

        private bool DeleteOldDatabase()
        {
            if (System.IO.File.Exists(_dbPath))
            {
                try
                {
                    System.IO.File.Delete(_dbPath);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
