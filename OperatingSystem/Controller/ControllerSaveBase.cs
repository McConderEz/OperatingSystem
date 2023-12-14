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
        protected async Task SaveAsync(string fileName, object item)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate,FileAccess.Write,
                FileShare.None))
            {
                JsonSerializer.Serialize(fs, item);
            }
        }

        protected async Task<T> LoadAsync<T>(string fileName)
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
