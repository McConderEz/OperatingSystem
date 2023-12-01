using OperatingSystem.Model.FileSystemEnteties;
using System.Text;

FileSystem fs = new FileSystem();

fs.CreateFile(@"D:\NTFS\data.bin");
fs.WriteFile(@"D:\NTFS\data.bin", new StringBuilder("sadasdsadsadafgbvcx"));
fs.CopyTo(@"D:\NTFS\data.bin", @"D:\NTFS\dataCopy.bin");


//var str = new StringBuilder();
//char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e' };
//for(var i = 0;i < 512; i++)
//{
//    str.Append(chars[new Random().Next(0, chars.Length - 1)]);
//}

//fs.WriteFile("data.bin", str);
//Console.WriteLine(fs.ReadFile(@"D:\NTFS\data.bin"));


