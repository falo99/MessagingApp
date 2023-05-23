using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentMessagingApp.Models;

namespace StudentMessagingApp.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> _messagesCollection;
        private readonly IMongoCollection<Students> _studentCollection;

        public MessageService(
            IOptions<DbSettings> collectionsDbSettings)
        {
            var mongoClient = new MongoClient(
                collectionsDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                collectionsDbSettings.Value.Databasename);

            _messagesCollection = mongoDatabase.GetCollection<Message>(
                collectionsDbSettings.Value.MessagesCollectionName);

            _studentCollection = mongoDatabase.GetCollection<Students>(
                collectionsDbSettings.Value.Databasename);
        }

        public async Task<List<Message>> GetAsync() =>
      await _messagesCollection.Find(_ => true).ToListAsync();

        public async Task<List<Message>> GetAsyncbyStudentId(string id) =>
            await _messagesCollection.Find(x => x.StudentId == id).ToListAsync();
        

        public async Task<Message?> GetAsync(Guid id) =>
            await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Message>>? GetAsyncByNameAndSurname(string search)
        {

            var splittedString = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Students student;

            if (splittedString.Length >= 2)
            {
                var name = splittedString[0];
                var surname = splittedString[1];

                name = char.ToUpper(name[0]) + name.Substring(1);
                surname = char.ToUpper(surname[0]) + surname.Substring(1);

                student = await _studentCollection.Find(x => x.Name == name && x.Surname.Contains(surname)).FirstOrDefaultAsync();
            }
            else
            {
                search = char.ToUpper(search[0]) + search.Substring(1);
                student =  await _studentCollection.Find(x => x.Name.Contains(search)).FirstOrDefaultAsync();
            }


            if (student == null)
            {
                return null;
            }

            var messages =  await _messagesCollection.Find(x => x.StudentId == student.Id).ToListAsync();

            //if (!messages.Any())
            //{
            //    return new List<Message>();
            //}
          
            return messages;

        }

        public async Task CreateAsync(Message newMessage)
        {
            newMessage.Id = Guid.NewGuid();
            await _messagesCollection.InsertOneAsync(newMessage);
        }
           

        public async Task UpdateAsync(Guid id, Message updatedMessage) =>
            await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

        public async Task RemoveAsync(Guid id, Students updatedStudent)
        {
            updatedStudent.Messages.Remove(id);
            await _messagesCollection.DeleteOneAsync(x => x.Id == id);
            
        }
        public async Task RemoveAsyncStudentMessages(string id)
        {
            await _messagesCollection.DeleteManyAsync(x => x.StudentId == id);
        }


    }
}
