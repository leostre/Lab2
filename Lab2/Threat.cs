using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Threat
    {
        private string id;
        public string Id { set { id = value; } get { return "УБИ." + id; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Object { get; set; }
        public bool IsConfidential { get; set; }
        public bool IsUndamaged { get; set; }
        public bool IsAccessible { get; set; }

        public Threat(string id, string name, string description, string source, string @object, bool isConfidential, bool isUndamaged, bool isAccessible)
        {
            Id = id;
            Name = name;
            Description = description;
            Source = source;
            Object = @object;
            IsConfidential = isConfidential;
            IsUndamaged = isUndamaged;
            IsAccessible = isAccessible;
        }
        public override string ToString()
        {
            string text = $"\n\nИдентификатор: \n\t{Id}\n\nНазвание: \n\t{Name}\n\nОписание: \n\t{Description}\n\nИсточник: \n\t{Source}\n\nОбъект: \n\t{Object}\n\nНарушение конфиденциальности: \n\t{(IsConfidential ? "Нет" :  "Да" )}\n\nПовреждение информации: \n\t{(IsUndamaged ? "Нет" : "Да")}\n\nНарушение доступа: \n\t{(IsAccessible ? "Нет" : "Да")}";
            return text;
        }

    }
}
