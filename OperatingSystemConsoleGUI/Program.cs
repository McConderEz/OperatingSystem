using OperatingSystem.Model.FileSystemEnteties;
using OperatingSystem.Model.ProcessCommunication;
using System.Text;

FileSystem fs = FileSystem.Instance;
Process process1 = new Process();
process1.InputMethod = (arg) => fs.CreateFile(arg);
process1.Start(process1.InputMethod, @"D:\NTFS\testAsync.txt");
process1.ChangePriority(ThreadPriority.Highest);

var process3 = new Process();
process3.WriteMethod = (arg1,arg2,arg3) => fs.WriteFile(arg1,arg2,arg3);
process3.StartWriteMethod(process3.WriteMethod, @"D:\NTFS\testAsync.txt", new StringBuilder("sadasda"), false);
process3.ChangePriority(ThreadPriority.Highest);

Thread.Sleep(4000);

//Console.WriteLine(process1.ThreadPriority);
//Process process2 = new Process();
//process2.InputMethod = (arg) => fs.Delete(arg);
//process2.Start(process2.InputMethod, @"D:\NTFS\testAsync.txt");

Console.WriteLine("готово");

//process.StartAsync(process.Method, @"D:\NTFS\testAsync.txt");

//fs.CreateFile(@"D:\NTFS\data.bin");
//fs.WriteFile(@"D:\NTFS\data.bin", new StringBuilder("123"), true);
//fs.CopyTo(@"D:\NTFS\data.bin", @"D:\NTFS\dataCopy.bin");


//var str = new StringBuilder();
//char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e' };
//for(var i = 0;i < 512; i++)
//{
//    str.Append(chars[new Random().Next(0, chars.Length - 1)]);
//}

//fs.WriteFile("data.bin", str);
//Console.WriteLine(fs.ReadFile(@"D:\NTFS\data.bin"));


