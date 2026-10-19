using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AirGrap.Services;

public record Person(int Id, string Name, int Age);

public class UserService : IUserService
{
    private readonly List<Person> _persons = new List<Person>
    {
        new(1, "Иван", 25),
        new(2, "Мария", 30)
    };
    int NextId = 3;
    public Person? GetById(int id)
    {
        if (id <= 0 || id > _persons.Count) return null;
        return _persons[id - 1];
    }
    public IReadOnlyList<Person> GetAll() => _persons.ToList();
    public  Person Create(string name, int age)
    {
        var p = new Person(NextId++,name,age);
        _persons.Add(p);
        return p;
    } 
    public bool Update(int Id, string name,int age){
        bool _out = _persons.Exists(p => p.Id == Id);
        if (_out){
            _persons[_persons.FindIndex(p =>p.Id == Id)] = new Person(Id,name,age);
        } 
        return _out;
    }
    public bool Delete(int Id){
        var removedCount = _persons.RemoveAll(p => p.Id == Id);
        return removedCount > 0;
    }
}