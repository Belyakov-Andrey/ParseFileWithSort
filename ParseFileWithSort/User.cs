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
            int lineNumber = 1;

            foreach (var line in lines.Skip(1))
            {
                lineNumber++;
                try
                {
                    var user = ParseUserLine(line);
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке строки {lineNumber}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
        }

        return users;
    }

    public static User ParseUserLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            throw new ArgumentException("Строка не может быть пустой");
        }

        var parts = line.Split(new[] { ",", ";", ";" }, StringSplitOptions.None);

        if (parts.Length < 6)
        {
            throw new ArgumentException($"Недостаточно данных в строке. Ожидалось 6 полей, получено {parts.Length}");
        }
        
        var cleanedParts = new string[6];
        for (int i = 0; i < 6; i++)
        {
            cleanedParts[i] = CleanField(parts[i]);
        }

        if (string.IsNullOrWhiteSpace(cleanedParts[0]))
        {
            throw new ArgumentException("ID пользователя не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(cleanedParts[3]))
        {
            throw new ArgumentException("Email не может быть пустым");
        }

        return new User
        {
            Id = cleanedParts[0],
            FirstName = cleanedParts[1],
            LastName = cleanedParts[2],
            Email = cleanedParts[3],
            Gender = cleanedParts[4],
            IpAddress = cleanedParts[5]
        };
    }

    public static string CleanField(string field)
    {
        return new string(field.Where(c => !char.IsControl(c)).ToArray()).Trim();
    }
}