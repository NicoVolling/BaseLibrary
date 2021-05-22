using System.Data;

namespace BaseLibrary.Data.Connectivity.SQL
{
    /// <summary>
    /// Enthält ein <see cref="DataSet"/> als Rückgabewert von dem Microsoft SQL-Server und einen <see cref="bool"/> der angibt, ob die Abfrage erfolgreich war.
    /// </summary>
    public struct SQL_Result
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="Success"><see cref="bool"/>, der angibt, ob die Abfrage erfolgreich war.</param>
        /// <param name="Data"><see cref="DataSet"/>, das die zurückgegebenen Tabellen enthält.</param>
        public SQL_Result(bool Success, DataSet Data) : this()
        {
            this.Success = Success;
            this.Data = Data;
        }

        /// <summary>
        /// <see cref="DataSet"/>, das die zurückgegebenen Tabellen enthält.
        /// </summary>
        public DataSet Data { get; set; }

        /// <summary>
        /// <see cref="bool"/>, der angibt, ob die Abfrage erfolgreich war.
        /// </summary>
        public bool Success { get; set; }
    }
}