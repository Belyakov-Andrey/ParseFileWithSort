using ParseFileWithSort;


string filePath = @"D:\file\uset.txt"; // Файл должен быть в рабочей директории

if (!File.Exists(filePath))
{
    Console.WriteLine("Файл не найден!");
    return;
}

var users = User.LoadUsers(filePath);

var viewer = new UserDataViewer(users);
viewer.Run();


