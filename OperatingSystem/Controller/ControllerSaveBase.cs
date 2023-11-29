using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OperatingSystem.Controller
{
    public abstract class ControllerSaveBase
    {
        protected void Save(string fileName, object item)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fs, item);
            }
        }

        protected T Load<T>(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && JsonSerializer.Deserialize<T>(fs) is T items)
                {
                    return items;
                }
                else
                {
                    return default(T);
                }
            }
        }

    }
}
