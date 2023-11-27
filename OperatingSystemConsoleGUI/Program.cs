
using OperatingSystem.Model;
using System.Text;

FileSystem fs = new FileSystem();


fs.CreateFile("data.bin");

var str = new StringBuilder();
char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e' };
for(var i = 0;i < 2000; i++)
{
    str.Append(chars[new Random().Next(0, chars.Length - 1)]);
}

fs.WriteFile("data.bin", str);
Console.WriteLine(fs.GetEntities(@"D:\NTFS").First());
Console.WriteLine(fs.ReadFile(@"D:\NTFS\data.bin"));

fs.Delete(@"D:\NTFS\data.bin");