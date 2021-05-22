using System;

namespace BaseLibrary.Tracing
{
    /// <summary>
    /// Eine Klasse zur Vereinfachung von Tracings.
    /// </summary>
    public static class Trace
    {
        /// <summary>
        /// Der statische <see cref="string"/>, der die Zeichenfolge für das Debug-Tracing enthält.
        /// </summary>
        public static string TraceType_Debug { get => ":::Debug:::    "; }

        /// <summary>
        /// Der statische <see cref="string"/>, der die Zeichenfolge für das Fehler-Tracing enthält.
        /// </summary>
        public static string TraceType_Error { get => ":::Error:::    "; }

        /// <summary>
        /// Der statische <see cref="string"/>, der die Zeichenfolge für das Warn-Tracing enthält.
        /// </summary>
        public static string TraceType_Warning { get => ":::Warning:::  "; }

        /// <summary>
        /// Fügt dem Tracing eine weitere Zeile hinzu.
        /// </summary>
        /// <param name="Type">Der Tracingtyp, der angibt, um welche Art von Tracing es sich handelt.</param>
        /// <param name="Class">Die auslösende Klasse</param>
        /// <param name="Method">Die auslösende Methode</param>
        /// <param name="Message">Die Nachricht / der Fehlercode</param>
        public static void Write(string Type, string Class, string Method, string Message)
        {
            Console.WriteLine($"{Type} -> Class.Method: {Class}.{Method} -> Message: {Message}");
        }
    }
}