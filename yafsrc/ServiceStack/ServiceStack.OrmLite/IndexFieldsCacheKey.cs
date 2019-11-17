using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ServiceStack.OrmLite
{
    public class IndexFieldsCacheKey
    {
        int hashCode;

        public ModelDefinition ModelDefinition { get; }

        public IOrmLiteDialectProvider Dialect { get; } 

        public List<string> Fields { get; }

        public IndexFieldsCacheKey(IDataReader reader, ModelDefinition modelDefinition, IOrmLiteDialectProvider dialect)
        {
            ModelDefinition = modelDefinition;
            Dialect = dialect;

            var startPos = 0;
            var endPos = reader.FieldCount;

            Fields = new List<string>(endPos - startPos);

            for (var i = startPos; i < endPos; i++)
                Fields.Add(reader.GetName(i));

            unchecked 
            {
                hashCode = 17;
                hashCode = hashCode * 23 + ModelDefinition.GetHashCode();
                hashCode = hashCode * 23 + Dialect.GetHashCode();
                hashCode = hashCode * 23 + Fields.Count;
                foreach (var t in this.Fields)
                {
                    this.hashCode = this.hashCode * 23 + t.Length;
                }
            }
        }

        public override bool Equals (object obj)
        {
            var that = obj as IndexFieldsCacheKey;
            
            if (obj == null) return false;
            
            return this.ModelDefinition == that.ModelDefinition
                && this.Dialect == that.Dialect
                && this.Fields.Count == that.Fields.Count
                && this.Fields.SequenceEqual(that.Fields);
        }
        
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}