using System;

namespace PasswordManager.Data.Attributes.CollectionName
{
    public class CollectionName : Attribute
    {
        private string _name { get; set; }
        
        public string Name => _name;
        
        public CollectionName(string name)
        {
            this._name = name;
        }
    }
}