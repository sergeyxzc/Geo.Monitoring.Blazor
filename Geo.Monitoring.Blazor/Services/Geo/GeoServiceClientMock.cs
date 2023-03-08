namespace Geo.Monitoring.Blazor.Services.Geo;

public class GeoServiceClientMock : IGeoServiceClient
{
    private readonly IUserService _userService;
    private static Random R = new();
    private static volatile int IdCount = 1;
    private static int GetNextId() => Interlocked.Increment(ref IdCount);

    private class CompanyMock
    {
        public static CompanyMock Gen(int i)
        {
            var company = new CompanyMock(GetNextId(), $"Company-{i}");

            foreach (var k in Enumerable.Range(1, 10))
            {
                company.CreateEmployee($"co-{i}-lo-{k}@tmail.com");
            }

            foreach (var k in Enumerable.Range(1, 10))
            {
                var p = company.CreateProject($"co-{i}-project-{k}");
                p.GenLoggers();
            }

            return company;
        }

        public CompanyMock(int id, string name)
        {
            Id = id;
            Name = name;
            Employees.Add(new EmployeeMock(GetNextId(), this, "cadmin"));
            Employees.Add(new EmployeeMock(GetNextId(), this, "yadmin"));
        }

        public int Id { get; }
        public List<ProjectMock> Projects { get; } = new();
        public List<EmployeeMock> Employees { get; } = new();
        public string Name { get; set; }
        public string BrandName { get; set; } = "Viola company";

        public Address Address { get; set; } = new Address()
        {
            Country = "Russia",
            City = "Nadym",
            AddressLine = "Lenin Street 1",
            PostalCode = "223344",
            Region = "Siberia"
        };

        public ProjectMock CreateProject(string name)
        {
            var p = new ProjectMock(GetNextId(), this)
            {
                Name = name
            };
            Projects.Add(p);
            return p;
        }

        public EmployeeMock CreateEmployee(string login)
        {
            var e = new EmployeeMock(GetNextId(), this, login)
            {
                FirstName = login,
                LastName = login,
                MiddleName = login
            };
            Employees.Add(e);
            return e;
        }
    }

    private class ProjectMock
    {
        public ProjectMock(int id, CompanyMock company)
        {
            Id = id;
            Company = company;
        }

        public void GenLoggers()
        {
            Loggers.AddRange(Enumerable.Range(1, 10).Select(i =>
            {
                var logger = new LoggerMock(GetNextId(), $"Logger-{i}");
                logger.GenSensors();
                return logger;
            }));
        }

        public int Id { get; }
        public CompanyMock Company { get; }
        public string Name { get; set; }
        public List<int> Employees { get; } = new();
        public List<LoggerMock> Loggers { get; } = new();

        public void AddEmployee(int employeeId)
        {
            var emp = Company.Employees.Find(e => e.Id == employeeId);
            if (Employees.All(e => e != employeeId))
                throw new ArgumentException("Already added");
            Employees.Add(emp.Id);
        }
    }

    private class EmployeeMock
    {
        public EmployeeMock(int id, CompanyMock company, string login)
        {
            Id = id;
            Company = company;
            Login = login;
        }

        public int Id { get; }
        public CompanyMock Company { get; }
        public string Login { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    private class LoggerMock
    {
        public LoggerMock(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; set; }
        public GeoPoint Position { get; set; } = new GeoPoint() { Latitude = 30, Longitude = 60 };
        public List<SensorMock> Sensors { get; } = new();

        public void GenSensors()
        {
            Sensors
                .AddRange(Enumerable.Range(1, 30)
                .Select(i =>
                {
                    var st = R.Next(2) == 0 ? SensorType.Pressure : SensorType.Gyroscope;
                    var s = new SensorMock(GetNextId(), st);
                    s.GenValues();
                    return s;
                })
            );
        }
    }

    private class SensorMock
    {
        public SensorMock(int id, SensorType type)
        {
            Id = id;
            Type = type;
        }
        public int Id { get; }
        public SensorType Type { get; set; }
        public double MaxLimit { get; set; } = R.Next(50, 100);
        public double MinLimit { get; set; } = R.Next(10, 40);
        public string SensorKey { get; set; } = $"key-{GetNextId()}";
        public List<SensorValue> Values { get; } = new();

        public void GenValues()
        {
            var start = /*request.From ?? */DateTime.UtcNow.AddYears(-1);
            var end = /*request.To ?? */DateTime.UtcNow;

            if (start > end)
                start = end;

            var step = (end - start).TotalHours / 1000;

            var values = Enumerable.Range(1, 500).Select(i =>
            {
                var sp = new SensorValue()
                {
                    Timestamp = start.AddHours(step),
                    Value = R.NextDouble() * 100.0
                };
                start = sp.Timestamp;
                return sp;
            }).ToArray();

            Values.AddRange(values);
        }
    }

    public class SensorValue
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }

    private static readonly List<CompanyMock> _companies = new();

    public GeoServiceClientMock(IUserService userService)
    {
        _userService = userService;

        if (_companies is { Count: 0 })
        {
            _companies.AddRange(Enumerable.Range(1, 3).Select(CompanyMock.Gen));
        }
    }

    private async Task<EmployeeMock> GetLoggedEmployeeAsync()
    {
        var user = await _userService.GetUserContextAsync(CancellationToken.None);
        var employee = _companies
            .SelectMany(x => x.Employees)
            .SingleOrDefault(x => 
                x.Id == int.Parse(user.EmployeeId) && 
                x.Company.Id == int.Parse(user.CompanyId));
        return employee;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var employee = _companies.SelectMany(x => x.Employees).SingleOrDefault(x => x.Login == request.LoginName);

        if (employee == null)
        {
            return new LoginResponse()
            {
                ErrorMessage = "Not found employee",
                Successes = false
            };
        }

        return new LoginResponse()
        {
            CompanyId = employee.Company.Id,
            EmployeeId = employee.Id,
            ErrorMessage = "",
            Successes = true
        };
    }

    public async Task<CompanyDetails> GetCompanyInfoAsync(CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        return new CompanyDetails()
        {
            Id = loggedEmployee.Company.Id,
            Name = loggedEmployee.Company.Name,
            BrandName = loggedEmployee.Company.BrandName,
            Address = loggedEmployee.Company.Address
        };
    }

    public async Task<GetCompanyEmployeesResponse> GetCompanyEmployeesAsync(CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        return new GetCompanyEmployeesResponse()
        {
            Employees = loggedEmployee.Company.Employees.Select(x => new EmployeeDesc()
            {
                Id = x.Id,
                BirthDate = x.BirthDate,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                IsBeneficiary = false
            }).ToArray()
        };
    }

    public async Task<GetCompanyProjectsResponse> GetCompanyProjectsAsync(CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        return new GetCompanyProjectsResponse()
        {
            Projects = loggedEmployee.Company.Projects.Select(x => new CompanyProjectDesc()
            {
                Id = x.Id,
                Name = x.Name,
                EmployeeCount = x.Employees.Count
            }).ToArray()
        };
    }

    public async Task<GetProjectResponse> GetProjectAsync(int id, CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        var p = loggedEmployee.Company.Projects.Single(x => x.Id == id);
        //var employees = _loggedEmployeeMock.Company.Employees.ToDictionary(x => x.Id);

        return new GetProjectResponse()
        {
            Project = new CompanyProjectDesc()
            {
                Id = p.Id,
                Name = p.Name,
                EmployeeCount = p.Employees.Count
            },

            Employees = p.Employees.Select(x => new ProjectEmployee
            {
                EmployeeId = x,
                ProjectId = p.Id
            }).ToArray(),

            Loggers = p.Loggers.Select(x => new LoggerDesc
            {
                Id = x.Id,
                Name = x.Name,
                Position = x.Position,
                Sensors = x.Sensors.Select(s =>
                {
                    var last = s.Values.Last();

                    return new SensorDesc()
                    {
                        Id = s.Id,
                        LastValue = last.Value,
                        MaxLimit = s.MaxLimit,
                        MinLimit = s.MinLimit,
                        SensorKey = s.SensorKey,
                        Type = s.Type,
                        UpdateTimestamp = last.Timestamp
                    };
                }).ToArray()
            }).ToArray()
            //Employees = p.Employees.Select(employeeId =>
            //{
            //    var e = employees[employeeId];
            //    return new EmployeeDesc()
            //    {
            //        Id = e.Id,
            //        BirthDate = e.BirthDate,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        MiddleName = e.MiddleName,
            //        IsBeneficiary = false
            //    };
            //}).ToArray(),

        };
    }

    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        var proj = loggedEmployee.Company.CreateProject(request.Name);
        return new CreateProjectResponse() { Id = proj.Id };
    }

    public async Task<UpdateProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        var p = loggedEmployee.Company.Projects.First(x => x.Id == id);
        p.Name = request.Name;
        return new UpdateProjectResponse() { Id = p.Id };
    }

    public async Task<ProjectEmployee> AddProjectEmployeeAsync(int id, AddProjectEmployeeRequest request, CancellationToken cancellationToken)
    {
        var loggedEmployee = await GetLoggedEmployeeAsync();
        var p = loggedEmployee.Company.Projects.First(x => x.Id == id);
        p.AddEmployee(request.EmployeeId);
        return new ProjectEmployee()
        {
            EmployeeId = request.EmployeeId,
            ProjectId = p.Id
        };
    }

    public Task<LoggerDesc> GetLoggerAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SensorDesc> GetSensorAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}