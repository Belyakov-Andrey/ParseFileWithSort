using ParseFileWithSort;


string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.Combine(executableDirectory, "user.txt");

if (!File.Exists(filePath))
{
    Console.WriteLine("Файл не найден!");
    return;
}

var users = User.LoadUsers(filePath);

var viewer = new UserDataViewer(users);
viewer.Run();


