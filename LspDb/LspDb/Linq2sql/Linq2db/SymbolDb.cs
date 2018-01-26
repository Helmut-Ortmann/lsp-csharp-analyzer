using LinqToDB.DataProvider;


namespace DataModels.Symbols
{
    // ReSharper disable once InconsistentNaming
    public partial class SYMBOLDB
    {
        /// <summary>
        /// Create lind2db context wit new Provide, ConnectionString for Visual Studio Code Symbol SQLite Database
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="connectionString"></param>
        public SYMBOLDB(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString)
        {
            InitDataContext();
        }



    }
}
