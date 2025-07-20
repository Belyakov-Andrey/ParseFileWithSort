namespace ParseFileWithSort;

public class UserDataViewer
{
    private const int DefaultPageSize = 10;
    private const string DefaultSortColumn = "Id";

    private readonly List<User> _users;
    private IReadOnlyList<User> _filteredUsers;
    private int _pageSize = DefaultPageSize;
    private int _currentPage = 1;
    private string _searchTerm = "";
    private string _sortColumn = DefaultSortColumn;
    private bool _sortAscending = true;

    public UserDataViewer(List<User> users)
    {
        _users = users;
        _filteredUsers = new List<User>(users);
    }

    public void Run()
    {
        Console.TreatControlCAsInput = true;

        while (true)
        {
            Console.Clear();
            DisplayUsers();
            DisplayMenu();

            var key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.Q:
                    return;
                case ConsoleKey.N:
                    NextPage();
                    break;
                case ConsoleKey.P:
                    PreviousPage();
                    break;
                case ConsoleKey.S:
                    Search();
                    break;
                case ConsoleKey.O:
                    ChangePageSize();
                    break;
                case ConsoleKey.D1:
                    SortBy("Id");
                    break;
                case ConsoleKey.D2:
                    SortBy("FirstName");
                    break;
                case ConsoleKey.F3:
                    SortBy("LastName");
                    break;
                case ConsoleKey.F4:
                    SortBy("Email");
                    break;
                case ConsoleKey.F5:
                    SortBy("Gender");
                    break;
                case ConsoleKey.F6:
                    SortBy("IpAddress");
                    break;
                case ConsoleKey.F7:
                    ToggleSortOrder();
                    break;
                default:
                    continue;
            }
        }
    }

    private void DisplayUsers()
    {
        Console.WriteLine($"Страница {_currentPage} из {TotalPages}. Всего записей: {_filteredUsers.Count}");
        Console.WriteLine(
            "-------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine(
            $"| {"ID",-5} | {"Имя",-15} | {"Фамилия",-15} | {"Email",-35} | {"Пол",-12} | {"IP адрес",-15} |");
        Console.WriteLine(
            "-------------------------------------------------------------------------------------------------------------------");

        var pagedUsers = _filteredUsers
            .Skip((_currentPage - 1) * _pageSize)
            .Take(_pageSize);

        foreach (var user in pagedUsers)
        {
            Console.WriteLine(
                $"| {user.Id,-5} | {user.FirstName,-15} | {user.LastName,-15} | {user.Email,-35} | {user.Gender,-12} | {user.IpAddress,-15} |");
        }

        Console.WriteLine(
            "-------------------------------------------------------------------------------------------------------------------");
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nУправление:");
        Console.WriteLine("Q - Выход | N - След. страница | P - Пред. страница | S - Поиск | O - Размер страницы");
        Console.WriteLine("1-6 - Сортировка по столбцу | 7 - Изменить порядок сортировки");
        Console.WriteLine($"Текущая сортировка: {_sortColumn} ({(_sortAscending ? "по возр." : "по убыв.")})");
    }

    private void NextPage()
    {
        if (_currentPage < TotalPages)
        {
            _currentPage++;
        }
    }

    private void PreviousPage()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
        }
    }

    private void Search()
    {
        Console.Write("Введите текст для поиска: ");
        var newSearchTerm = Console.ReadLine();

        if (newSearchTerm != _searchTerm)
        {
            _searchTerm = newSearchTerm;
            ApplyFiltersAndSorting();
        }
    }

    private void ChangePageSize()
    {
        Console.Write("Введите новый размер страницы: ");
        if (int.TryParse(Console.ReadLine(), out int newSize) && newSize > 0 && newSize != _pageSize)
        {
            _pageSize = newSize;
            _currentPage = 1;
        }
    }

    public void SortBy(string column)
    {
        if (_sortColumn != column)
        {
            _sortColumn = column;
            ApplyFiltersAndSorting();
        }
    }

    private void ToggleSortOrder()
    {
        _sortAscending = !_sortAscending;
        ApplyFiltersAndSorting();
    }

    private void ApplyFiltersAndSorting()
    {
        var query = _users.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(_searchTerm))
        {
            query = query.Where(u => (u.Id ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     (u.FirstName ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     (u.LastName ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     (u.Email ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     (u.Gender ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     (u.IpAddress ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        query = _sortColumn switch
        {
            "Id" => _sortAscending ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id),
            "FirstName" => _sortAscending ? query.OrderBy(u => u.FirstName) : query.OrderByDescending(u => u.FirstName),
            "LastName" => _sortAscending ? query.OrderBy(u => u.LastName) : query.OrderByDescending(u => u.LastName),
            "Email" => _sortAscending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
            "Gender" => _sortAscending ? query.OrderBy(u => u.Gender) : query.OrderByDescending(u => u.Gender),
            "IpAddress" => _sortAscending ? query.OrderBy(u => u.IpAddress) : query.OrderByDescending(u => u.IpAddress),
            _ => query
        };

        _filteredUsers = query.ToList();
        _currentPage = 1;
    }

    private int TotalPages => (int)Math.Ceiling((double)_filteredUsers.Count / _pageSize);
}