namespace Entities;

public sealed class Employee : BaseEntity
{
    public int Id { get; set; }
    public string Post { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string PhoneNumber { get; set; } = null!;
    public string Mail { get; set; } = null!;
    public string FamilyStatus { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<string>? Hobbies { get; set; } = null!;

    public Employee(string post, string surname, string name, string patronymic,
    DateTime dateOfBirth, string phoneNumber, string mail, string familyStatus, string city,
    string address, List<string>? hobbies, DateTime createdAt) : base(createdAt)
    {
        Post = post;
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Mail = mail;
        FamilyStatus = familyStatus;
        City = city;
        Address = address;
        Hobbies = hobbies;
        CreatedAt = createdAt;
    }

    public Employee() : base(DateTime.Now) { }
}
