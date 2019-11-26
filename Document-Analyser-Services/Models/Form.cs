using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class Form
    {
        private Dictionary<string, Field> _fieldMap;

        public List<Field> Fields { get; set; }

        public Form()
        {
            Fields = new List<Field>();
            _fieldMap = new Dictionary<string, Field>();
        }

        public void AddField(Field field)
        {
            Fields.Add(field);
            _fieldMap.Add(field.Key.ToString(), field);
        }

        public Field GetFieldByKey(string key)
        {
            return _fieldMap.GetValueOrDefault(key) ?? new Field(new Block(), new List<Block>());
        }

        public List<Field> SearchFieldsByKey(string key)
        {
            return Fields.FindAll(f => f.Key.ToString().ToLower().Contains(key.ToLower()));
        }

        public override string ToString()
        {
            return string.Join("\n", Fields);
        }
    }
}
