namespace ParseFileWithSort;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string IpAddress { get; set; }


    public static List<User> LoadUsers(string filePath)
    {
        var users = new List<User>();

        try
        {
            var lines = File.ReadAllLines(filePath);
            
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(new[] { ",", ";", ";" }, StringSplitOptions.None);

                if (parts.Length >= 6)
                {
                    for (int i = 0; i < parts.Length; i++)
                    {
                        parts[i] = new string(parts[i].Where(c => !char.IsControl(c)).ToArray()).Trim();
                    }

                    users.Add(new User
                    {
                        Id = parts[0],
                        FirstName = parts[1],
                        LastName = parts[2],
                        Email = parts[3],
                        Gender = parts[4],
                        IpAddress = parts[5]
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
        }

        return users;
    }
}