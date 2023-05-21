using Microsoft.AspNetCore.Mvc;
using StudentMessagingApp.Models;
using StudentMessagingApp.Services;


namespace StudentMessagingApi.Controllers
{
   //[ApiController]
   // [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly MessageService _messagesService;
        private readonly StudentsService _studentsService;
        public MessageController(MessageService messagesService, StudentsService studentService)
        {
            _messagesService = messagesService;
            _studentsService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _messagesService.GetAsync();
            return View(messages);
        }
           

        [HttpGet("get messsage by id")]
        public async Task<IActionResult> Details(Guid id)
        {
            var message = await _messagesService.GetAsync(id);

            if (message is null)
            {
                return NotFound();
            }

            return View(message);
        }

        [HttpGet("Student info based on Message Id")]
        public async Task<IActionResult> DetailsStudent(Guid id)
        {
            var message = await _messagesService.GetAsync(id);
            var student = await _studentsService.GetAsync(message.StudentId);
            if (student is null)
            {
                return NotFound();
            }
            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Message newMesssage)
        {
            await _messagesService.CreateAsync(newMesssage);
           
           return RedirectToAction("Index", "Message");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var message = await _messagesService.GetAsync(id);

            if (message is null)
            {
                return NotFound();
            }

            await _messagesService.RemoveAsync(id);

            return RedirectToAction("Index", "Message");
        }
    }
}
