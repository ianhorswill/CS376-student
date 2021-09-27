using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//
// This is where you put your code.  There are two sections, one for members to add to the Serializer class,
// and one for members to add to the Deserializer class.
//
namespace Assets.Serialization
{
    // The partial keyword just means we're adding these three methods to the code in Serializer.cs
    public partial class Serializer
    {
        /// <summary>
        /// Print out the serialization data for the specified object.
        /// </summary>
        /// <param name="o">Object to serialize</param>
        private void WriteObject(object o)
        {
            switch (o)
            {
                case null:
                    throw new NotImplementedException("Fill me in");
                    break;

                case int i:
                    throw new NotImplementedException("Fill me in");
                    break;

                case float f:
                    throw new NotImplementedException("Fill me in");
                    break;

                // BUG: this doesn't handle strings that themselves contain quote marks
                // but that doesn't really matter for an assignment like this, so I'm not
                // going to confuse the reader by complicating the code to escape the strings.
                case string s:
                    throw new NotImplementedException("Fill me in");
                    break;

                case bool b:
                    throw new NotImplementedException("Fill me in");
                    break;

                case IList list:
                    throw new NotImplementedException("Fill me in");
                    break;

                default:
                    if (o.GetType().IsValueType)
                        throw new Exception($"Trying to write an unsupported value type: {o.GetType().Name}");

                    WriteComplexObject(o);
                    break;
            }
        }

        /// <summary>
        /// Serialize a complex object (i.e. a class object)
        /// If this object has already been output, just output #id, where is is it's id from GetID.
        /// If it hasn't then output #id { type: "typename", field: value ... }
        /// </summary>
        /// <param name="o">Object to serialize</param>
        private void WriteComplexObject(object o)
        {
            throw new NotImplementedException("Fill me in");
        }
    }

    // The partial keyword just means we're adding these three methods to the code in Deserializer.cs
    public partial class Deserializer
    {
        /// <summary>
        /// Read whatever data object is next in the stream
        /// </summary>
        /// <param name="enclosingId">The object id of whatever object this is a part of, if any</param>
        /// <returns>The deserialized object</returns>
        public object ReadObject(int enclosingId)
        {
            SkipWhitespace();

            if (End)
                throw new EndOfStreamException();

            switch (PeekChar)
            {
                case '#':
                    return ReadComplexObject(enclosingId);

                case '[':
                    return ReadList(enclosingId);

                case '"':
                    return ReadString(enclosingId);

                case '-':
                case '.':
                case var c when char.IsDigit(c):
                    return ReadNumber(enclosingId);

                case var c when char.IsLetter(c):
                    return ReadSpecialName(enclosingId);

                default:
                    throw new Exception($"Unexpected character {PeekChar} found while reading object id {enclosingId}");
            }
        }

        /// <summary>
        /// Called when the next character is a #.  Read the object id of the object and return the
        /// object.  If that object id has already been read, return the object previously returned.
        /// Otherwise, there will be a { } expression after the object id.  Read it, create the object
        /// it represents, and return it.
        /// </summary>
        /// <param name="enclosingId">Object id of the object this expression appears inside of, if any.</param>
        /// <returns>The object referred to by this #id expression.</returns>
        private object ReadComplexObject(int enclosingId)
        {
            GetChar();  // Swallow the #
            var id = (int)ReadNumber(enclosingId);
            SkipWhitespace();

            // You've got the id # of the object.  Are we done now?
            throw new NotImplementedException("Fill me in");

            // Assuming we aren't done, let's check to make sure there's a { next
            SkipWhitespace();
            if (End)
                throw new EndOfStreamException($"Stream ended after reference to unknown ID {id}");
            var c = GetChar();
            if (c != '{')
                throw new Exception($"Expected '{'{'}' after #{id} but instead got {c}");

            // There's a {.
            // Let's hope there's a type: typename line.
            var (hopefullyType, typeName) = ReadField(id);
            if (hopefullyType != "type")
                throw new Exception(
                    $"Expected type name at the beginning of complex object id {id} but instead got {typeName}");
            var type = typeName as string;
            if (type == null)
                throw new Exception(
                    $"Expected a type name (a string) in 'type: ...' expression for object id {id}, but instead got {typeName}");

            // Great!  Now what?
            throw new NotImplementedException("Fill me in");

            // Read the fields until we run out of them
            while (!End && PeekChar != '}')
            {
                var (field, value) = ReadField(id);
                // We've got a field and a value.  Now what?
                throw new NotImplementedException("Fill me in");
            }

            if (End)
                throw new EndOfStreamException($"Stream ended in the middle of {"{ }"} expression for id #{id}");

            GetChar();  // Swallow close bracket

            // We're done.  Now what?
            throw new NotImplementedException("Fill me in");
        }
    }
}
