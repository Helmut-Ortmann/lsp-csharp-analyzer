//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/t4models).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace DataModels.Symbols.Test
{
	/// <summary>
	/// Database       : test
	/// Data Source    : test
	/// Server Version : 3.14.2
	/// </summary>
	public partial class TestDB : LinqToDB.Data.DataConnection
	{
		public ITable<Test> Tests { get { return this.GetTable<Test>(); } }

		public TestDB()
		{
			InitDataContext();
		}

		public TestDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();
	}

	[Table("Test")]
	public partial class Test
	{
		[PrimaryKey, Identity] public long ID { get; set; } // integer
	}

	public static partial class TableExtensions
	{
		public static Test Find(this ITable<Test> table, long ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}
	}
}
