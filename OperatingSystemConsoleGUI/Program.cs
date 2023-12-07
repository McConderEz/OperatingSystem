using OperatingSystem.Controller;
using OperatingSystem.Model.FileSystemEntities;
using OperatingSystem.Model.ProcessCommunication;
using System.Text;



for(var i = 0; i < 10; i++)
{
    var userController = new UserController(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
}

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

