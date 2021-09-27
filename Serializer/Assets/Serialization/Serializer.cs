using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assets.Serialization
{
    /// <summary>
    /// Serializes a data structure into a specified text stream
    /// Uses a simplified version of JSON.
    /// </summary>
    public partial class Serializer
    {
        /// <summary>
        /// Serialize an object to a text stream
        /// </summary>
        /// <param name="o">Object to write</param>
        /// <param name="w">Stream to write it to</param>
        public static void Serialize(object o, TextWriter w) => new Serializer(w).WriteObject(o);

        /// <summary>
        /// Serialize an object to a string
        /// </summary>
        /// <param name="o">Object to serialize</param>
        /// <returns>The serialized form of the object</returns>
        public static string Serialize(object o)
        {
            var w = new StringWriter();
            Serialize(o, w);
            return w.ToString();
        }

        public static object CloneGraph(object o) => Deserializer.Deserialize(Serialize(o));

        /// <summary>
        /// Makes a fresh serializer object to write to the specified stream
        /// Use the WriteObject method to actual serialize something.
        /// </summary>
        /// <param name="writer">Stream to which to serialize</param>
        public Serializer(TextWriter writer)
        {
            Writer = writer;
        }

        /// <summary>
        /// The stream (TextWriter) to write to.
        /// </summary>
        public readonly TextWriter Writer;

        /// <summary>
        /// How much to indent after a newline.
        /// </summary>
        private int indentLevel;

        /// <summary>
        /// Write a string to output.
        /// </summary>
        /// <param name="s">String to write</param>
        private void Write(string s) => Writer.Write(s);

        /// <summary>
        /// Write an integer to output.
        /// </summary>
        /// <param name="i">Number to write</param>
        private void Write(int i) => Writer.Write(i);

        /// <summary>
        /// Write a float to output.
        /// </summary>
        /// <param name="f"></param>
        private void Write(float f) => Writer.Write(f);

        /// <summary>
        /// Write a character to output.
        /// </summary>
        /// <param name="c"></param>
        private void Write(char c) => Writer.Write(c);

        /// <summary>
        /// Write a Boolean to output.
        /// </summary>
        /// <param name="b"></param>
        private void Write(bool b) => Writer.Write(b);

        /// <summary>
        /// Start a new line in the output and indent to indentLevel
        /// </summary>
        private void NewLine()
        {
            Writer.WriteLine();
            for (var i = 0; i < 4 * indentLevel; i++)
                Write(' ');
        }

        /// <summary>
        /// Write a field of an object in format "fieldname: value"
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="fieldValue">Value of the field</param>
        /// <param name="firstOne">If this is the first field printed inside of the object.  This is just so it knows whether to print a comma before the field</param>
        private void WriteField(string fieldName, object fieldValue, bool firstOne)
        {
            if (!firstOne)
            {
                Write(",");
                NewLine();
            }
            Write(fieldName);
            Write(": ");
            WriteObject(fieldValue);
        }

        /// <summary>
        /// Write a { } or [ ] expression whose contents is produced by the specified generator procedure.  See WriteList for an example.
        /// </summary>
        /// <param name="start">Open bracket to use</param>
        /// <param name="generator">Procedure to print the contents inside the brackets, e.g. () => { stuff to print }</param>
        /// <param name="end">Close bracket to use</param>
        private void WriteBracketedExpression(string start, Action generator, string end)
        {
            Write(start);
            indentLevel++;
            NewLine();
            generator();
            indentLevel--;
            NewLine();
            Write(end);
        }

        /// <summary>
        /// Write a list or array in the format: [ elt, elt, elt, ... ]
        /// </summary>
        /// <param name="list"></param>
        private void WriteList(ICollection list)
        {
            if (list.Count == 0)
                Write("[ ]");
            else
                WriteBracketedExpression(
                    "[ ",
                    () =>
                    {
                        var firstItem = true;
                        foreach (var item in list)
                        {
                            if (firstItem)
                                firstItem = false;
                            else
                                Write(", ");
                            WriteObject(item);
                        }
                    },
                    " ]");
        }
    }
}
