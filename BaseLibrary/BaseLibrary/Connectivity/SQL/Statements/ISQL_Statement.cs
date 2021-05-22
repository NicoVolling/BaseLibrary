using System;

namespace BaseLibrary.Data.Connectivity.SQL.Statements
{
    /// <summary>
    /// Eine Schnittstelle für die Sammlung von SQL_Statements, mithilfe derer <see cref="Data.Objects.DataObject"/> abgefragt und verändert werden können.
    /// </summary>
    public interface ISQL_Statement
    {
        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/> mit Übergabe von <paramref name="Statement"/> und <paramref name="Parameters"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <param name="Statement">Die Zeichenfolge, die die zu überbende SQL-Abfrage enthält.</param>
        /// <param name="Parameters">Die Parameter, die die an <see cref="string.Format(string, object[])"/> übergeben werden, um die Daten in die Zeichefolge einzufügen.</param>
        void RunQuerry(string Statement, params string[] Parameters);

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Löscht das <paramref name="DataObject"/> vom Server.
        /// </remarks>
        /// <param name="DataObject">Das <see cref="Data.Objects.DataObject"/>, das von der Aktion betroffen ist.</param>
        void RunQuerry_DELETE(Object DataObject);

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Löscht das <paramref name="DataObject"/> vom Server.
        /// </remarks>
        /// <param name="ID">Die ID des Betroffenen Datensatzes.</param>
        void RunQuerry_DELETE(string ID);

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Fügt das <paramref name="DataObject"/> zur Datensammlung auf dem Server hinzu.
        /// </remarks>
        /// <param name="DataObject">Das <see cref="Data.Objects.DataObject"/>, das von der Aktion betroffen ist.</param>
        void RunQuerry_INSERT(Object DataObject);

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Gibt das aktuelle <see cref="Data.Objects.DataObject"/> vom Server zurück.
        /// </remarks>
        /// <param name="DataObject">Das <see cref="Data.Objects.DataObject"/>, dessen Daten neu geladen werden sollen.</param>
        void RunQuerry_SELECT(Object DataObject);

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Gibt alle aktuellen <see cref="Data.Objects.DataObject"/> vom Server zurück.
        /// </remarks>
        void RunQuerry_SELECTALL();

        /// <summary>
        /// Asynchroner Aufruf von <see cref="Data.Querries.DataQuerry{T}.RunQuerry(string, string[])"/>. Die ausgelösten Ereignisse werden an dieses Objekt weitergeleitet.
        /// <para/>Siehe: <seealso cref="Data.Querries.DataQuerry{T}.DataObjectsReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.DataSetReceived"/>, <seealso cref="Data.Querries.DataQuerry{T}.ErrorReceived"/>
        /// </summary>
        /// <remarks>
        /// Verändert das <see cref="Data.Objects.DataObject"/> auf dem Server.
        /// </remarks>
        /// <param name="DataObject">Das <see cref="Data.Objects.DataObject"/>, das von der Aktion betroffen ist.</param>
        void RunQuerry_UPDATE(Object DataObject);
    }
}