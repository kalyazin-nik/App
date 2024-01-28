namespace Entities;

public sealed class Employee(string post, string surname, string name, string patronymic,
    DateTime dateOfBirth, string phoneNumber, string mail, string familyStatus, string address,
    string city, List<string>? hobbies, DateTime createdAt) : BaseEntity(createdAt)
{
    public int Id { get; set; }
    public string Post { get; set; } = post;
    public string Surname { get; set; } = surname;
    public string Name { get; set; } = name;
    public string Patronymic { get; set; } = patronymic;
    public DateTime DateOfBirth { get; set; } = dateOfBirth;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Mail { get; set; } = mail;
    public string FamilyStatus { get; set; } = familyStatus;
    public string Address { get; set; } = address;
    public string City { get; set; } = city;
    public List<string>? Hobbies { get; set; } = hobbies;
}
