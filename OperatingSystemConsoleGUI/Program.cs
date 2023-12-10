using OperatingSystem.Controller;
using OperatingSystem.Model.FileSystemEntities;
using OperatingSystem.Model.ProcessCommunication;
using OperatingSystem.Model.OperatingSystem;
using System.Text;


OperatingSystem.Model.OperatingSystem.OperatingSystem operatingSystem = new OperatingSystem.Model.OperatingSystem.OperatingSystem();
operatingSystem.Аuthorization("minoddein","0958700191");

//operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), @"D:\NTFS\file1.txt");
//operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), @"D:\NTFS\file2.txt");
//operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), @"D:\NTFS\file3.txt");
//operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), @"D:\NTFS\file4.txt");
//operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), @"D:\NTFS\file5.txt");

for (int i = 1; i <= 5; i++)
{
    operatingSystem.TaskPlanner.StartProcess((arg1, arg2, arg3) => operatingSystem.FileSystem.WriteFile(arg1, arg2, arg3), @$"D:\NTFS\file{i}.txt", new StringBuilder("sdasda"), false);
}
operatingSystem.TaskPlanner.GetTaskPlannerInfo();

Console.WriteLine();

#region
//FileSystem fs = FileSystem.Instance;
//TaskPlanner taskPlanner = TaskPlanner.Instance;
//taskPlanner.StartProcess((arg) => fs.CreateFile(@"D:\NTFS\test.bin"), @"D:\NTFS\test.bin");
//taskPlanner.GenerationRandomProcess();
//taskPlanner.GenerationRandomProcess();
//taskPlanner.GenerationRandomProcess();
#endregion
#region
//process.StartAsync(process.Method, @"D:\NTFS\testAsync.txt");
//fs.CreateFile(@"D:\NTFS\data.bin");
//fs.WriteFile(@"D:\NTFS\data.bin", new StringBuilder("123"), true);
//fs.CopyTo(@"D:\NTFS\data.bin", @"D:\NTFS\dataCopy.bin")
//var str = new StringBuilder();
//char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e' };
//for(var i = 0;i < 512; i++)
//{
//    str.Append(chars[new Random().Next(0, chars.Length - 1)]);
//}
//fs.WriteFile("data.bin", str);
//Console.WriteLine(fs.ReadFile(@"D:\NTFS\data.bin"));
#endregion

