namespace ParseFileWithSort;

class UserDataViewer
{
    private List<User> _users;
    private List<User> _filteredUsers;
    private int _pageSize = 10;
    private int _currentPage = 1;
    private string _searchTerm = "";
    private string _sortColumn = "Id";
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
        _searchTerm = Console.ReadLine();

        ApplyFilters();
    }

    private void ChangePageSize()
    {
        Console.Write("Введите новый размер страницы: ");
        if (int.TryParse(Console.ReadLine(), out int newSize) && newSize > 0)
        {
            _pageSize = newSize;
            _currentPage = 1;
        }
    }

    private void SortBy(string column)
    {
        _sortColumn = column;
        ApplyFilters();
    }

    private void ToggleSortOrder()
    {
        _sortAscending = !_sortAscending;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
        {
            _filteredUsers = new List<User>(_users);
        }
        else
        {
            _filteredUsers = _users
                .Where(u => (u.Id ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.FirstName ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.LastName ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.Email ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.Gender ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.IpAddress ?? "").Contains(_searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    
        ApplySorting();
        _currentPage = 1;
    }

    private void ApplySorting()
    {
        switch (_sortColumn)
        {
            case "Id":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.Id).ToList()
                    : _filteredUsers.OrderByDescending(u => u.Id).ToList();
                break;
            case "FirstName":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.FirstName).ToList()
                    : _filteredUsers.OrderByDescending(u => u.FirstName).ToList();
                break;
            case "LastName":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.LastName).ToList()
                    : _filteredUsers.OrderByDescending(u => u.LastName).ToList();
                break;
            case "Email":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.Email).ToList()
                    : _filteredUsers.OrderByDescending(u => u.Email).ToList();
                break;
            case "Gender":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.Gender).ToList()
                    : _filteredUsers.OrderByDescending(u => u.Gender).ToList();
                break;
            case "IpAddress":
                _filteredUsers = _sortAscending
                    ? _filteredUsers.OrderBy(u => u.IpAddress).ToList()
                    : _filteredUsers.OrderByDescending(u => u.IpAddress).ToList();
                break;
        }
    }

    private int TotalPages => (int)Math.Ceiling((double)_filteredUsers.Count / _pageSize);
}