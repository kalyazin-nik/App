using System.Reflection;
using Database;

namespace Tests
{
    [TestFixture]
    public class ConfigureDatabaseTests
    {
        private PropertyInfo[] _properties;
        [SetUp]
        public void Setup()
        {
            _properties = typeof(User).GetProperties();
        }

        [Test]
        public void CorrectedOutputDictionaryForGetColumnName()
        {
            var expected = new Dictionary<string, string>()
            {
                { "Post", "post" },
                { "LastName", "last_name" },
                { "FirstName", "first_name" },
                { "MiddleName", "middle_name" }
            };

            var actual = ConfigureDatabase.GetColumnNames(_properties);
            Assert.That(actual, Is.EqualTo(expected));
        }

        private record User(string Post, string LastName, string FirstName, string MiddleName, bool Flag, int Year) { }
    }
}