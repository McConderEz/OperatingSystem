
using OperatingSystem.Model;
using System.Text;

FileSystem fs = new FileSystem();


fs.CreateFile("data.bin");

var str = new StringBuilder();
char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e' };
for(var i = 0;i < 512; i++)
{
    str.Append(chars[new Random().Next(0, chars.Length - 1)]);
}

fs.WriteFile("data.bin", str);


