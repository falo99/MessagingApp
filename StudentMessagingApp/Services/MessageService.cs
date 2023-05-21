using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentMessagingApp.Models;

namespace StudentMessagingApp.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> _messagesCollection;

        public MessageService(
            IOptions<DbSettings> collectionsDbSettings)
        {
            var mongoClient = new MongoClient(
                collectionsDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                collectionsDbSettings.Value.Databasename);

            _messagesCollection = mongoDatabase.GetCollection<Message>(
                collectionsDbSettings.Value.MessagesCollectionName);
        }

        public async Task<List<Message>> GetAsync() =>
      await _messagesCollection.Find(_ => true).ToListAsync();

        public async Task<Message?> GetAsync(Guid id) =>
            await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Message newMessage)
        {
            newMessage.Id = Guid.NewGuid();
            await _messagesCollection.InsertOneAsync(newMessage);
        }
           

        public async Task UpdateAsync(Guid id, Message updatedMessage) =>
            await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

        public async Task RemoveAsync(Guid id) =>
            await _messagesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
