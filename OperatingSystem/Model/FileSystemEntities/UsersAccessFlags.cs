using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEntities
{
    /// <summary>
    /// Права доступа для U-user(владельца файла)
    /// Права доступа для g-group(группы владельца файла)
    /// Права доступа для o-other(других пользователей системы)
    /// </summary>
    [DataContract]
    public class UsersAccessFlags
    {
        [DataMember]
        public AttributeFlags U { get; set; }
        [DataMember]
        public AttributeFlags G { get; set; }
        [DataMember]
        public AttributeFlags O { get; set; }

        [JsonConstructor]
        public UsersAccessFlags(AttributeFlags u, AttributeFlags g, AttributeFlags o)
        {
            U = u;
            G = g;
            O = o;
        }
    }
}
