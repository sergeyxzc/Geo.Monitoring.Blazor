//namespace Geo.Monitoring.Blazor;

///// <summary>
///// Схема в рамках проекта. То к чему можно прикрепить чертеж и разметить датчики с логерами.
///// </summary>
//public class ProjectSchema
//{
//    public int Id { get; set; }
//    public Project Project { get; set; }
//    public int ProjectId { get; set; }
//    public string Name { get; set; }
//}

//public class Project
//{
//    public int Id { get; set; }
//    public Company Company { get; set; }
//    public int CompanyId { get; set; }
//    public string Name { get; set; }
//}

///// <summary>
///// Связь между сотрудникам и проектом. Сотрудника надо добавлять в проект.
///// </summary>
////public class ProjectEmployee
////{
////    public int ProjectId { get; set; }
////    public int EmployeeId { get; set; }
////}

///// <summary>
///// Комапнию заводи через админку или через web-ручку на сервере.
///// </summary>
//public class Company
//{
//    public int Id { get; set; }
//    public string Name { get; set; }

//    // Доп поля о ригистрации компании
//}

//public class Employee
//{
//    public int Id { get; set; }
//    public Company Company { get; set; }
//    public int CompanyId { get; set; }
//    public EmployeeRole EmployeeRole { get; set; }
//    public int EmployeeRoleId { get; set; }
//    public string FirstName { get; set; }
//    public string SecondName { get; set; }
//    public string MiddleName { get; set; }
//    public DateOnly? BirthDate { get; set; }

//    /// <summary>
//    /// Явдяется ли сотрудник владельуем фирмы в каком либо виде
//    /// </summary>
//    public bool IsBeneficiary { get; set; }


//    // Доп поля, сслыки на паспартные данные и тп
//}

///// <summary>
///// Роли-должности (не авторизационные) которые существуют в компании.
///// </summary>
//public class EmployeeRole
//{
//    public int Id { get; set; }
//    public Company Company { get; set; }
//    public int CompanyId { get; set; }
//    public string Name { get; set; }
//}


//public class CompanyEmployeePermission
//{
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public Employee Employee { get; set; }

//    public Company Company { get; set; }
//    public int? CompanyId { get; set; }

//    public Project Project { get; set; }
//    public int? ProjectId { get; set; }

//    public ProjectSchema Schema { get; set; }
//    public int? SchemaId { get; set; }

//    /// <summary>
//    /// Право дано/запрещено заперщено
//    /// </summary>
//    public PermissionType PermissionType { get; set; }
    
//    public int PermissionId { get; set; }
//    public Permission Permission { get; set; }
//}

//public enum PermissionType
//{
//    None = 0,
//    Granted = 1,
//    Denied = 2
//}


//public class Permission
//{
//    public int Id { get; set; }

//    /// <summary>
//    /// Вид запрещеющего правила
//    /// </summary>
//    public string Kind { get; set; }
//}