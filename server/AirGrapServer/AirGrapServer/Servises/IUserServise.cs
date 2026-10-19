namespace AirGrap.Services;

public interface IUserService
{
    IReadOnlyList<Person> GetAll();
    Person? GetById(int id);
    Person Create(string name, int age);
    bool Update(int id, string name, int age);
    bool Delete(int id);
    //IReadOnlyList<Person> SearchByName(string substring);
}