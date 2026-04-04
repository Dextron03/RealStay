using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PropertyImage
    {
        public string Id {get;set;} = Guid.NewGuid().ToString();
        public string Path {get;set;}
        public string PropertyId {get;set;}
        public Property Property {get;set;}
    
    }
}