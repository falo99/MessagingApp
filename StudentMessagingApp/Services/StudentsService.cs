using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentMessagingApp.Models;

namespace StudentMessagingApp.Services
{
    public class StudentsService
    {
        private readonly IMongoCollection<Students> _studentsCollection;
      

        public StudentsService(
            IOptions<DbSettings> collectionsDbSettings)
        {

            var mongoClient = new MongoClient(
                collectionsDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                collectionsDbSettings.Value.Databasename);

            _studentsCollection = mongoDatabase.GetCollection<Students>(
                collectionsDbSettings.Value.StudentsCollectionName);

            
        }

        
        public async Task<List<Students>> GetAsync() =>
        await _studentsCollection.Find(_ => true).ToListAsync();

        public async Task<Students?> GetAsync(string id) =>
            await _studentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Students?> GetAsyncByNameAndSurname(string name, string surname) =>
            await _studentsCollection.Find(x => x.Name == name && x.Surname == surname).FirstOrDefaultAsync();

        public async Task CreateAsync(Students newStudent)
        {
            newStudent.Messages = new List<Guid>();
            await _studentsCollection.InsertOneAsync(newStudent);
        }
            

        public async Task UpdateAsync(string id, Students updatedStudent) =>
            await _studentsCollection.ReplaceOneAsync(x => x.Id == id, updatedStudent);

        public async Task UpdateAsyncMessages(string id,  Students updatedStudent)
        {
            await _studentsCollection.ReplaceOneAsync(x => x.Id == id, updatedStudent);
        }
          

        public async Task RemoveAsync(string id) =>
            await _studentsCollection.DeleteOneAsync(x => x.Id == id);


    }
}
