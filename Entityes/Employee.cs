namespace Entities;

public sealed class Employee(string post, string lastName, string firstName, string middleName, 
    int age, bool isMarried, string address, string city, string phoneNumber, string mail, 
    List<string> hobbies, DateTime createdAt) : BaseEntity(createdAt)
{
    public int Id { get; set; }
    public string Post { get; set; } = post;
    public string LastName { get; set; } = lastName;
    public string FirstName { get; set; } = firstName;
    public string MiddleName { get; set; } = middleName;
    public int Age { get; set; } = age;
    public bool IsMarried { get; set; } = isMarried;
    public string Address { get; set; } = address;
    public string City { get; set; } = city;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Mail { get; set; } = mail;
    public List<string> Hobbies { get; set; } = hobbies;
}
