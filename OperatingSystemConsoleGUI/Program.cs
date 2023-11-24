
using OperatingSystem.Model;
using System.Text;

FileSystem fs = new FileSystem();

fs.CreateFile("data.bin");

StringBuilder data = new StringBuilder();

for(int i = 0;i < 12; i++)
{
    data.Append(i);
}

fs.WriteFile("data.bin",data);
fs.WriteFile("data.bin", data);
Console.WriteLine();
