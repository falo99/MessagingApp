using Microsoft.AspNetCore.Mvc;
using StudentMessagingApp.Models;
using StudentMessagingApp.Services;

namespace StudentMessagingApp.Controllers
{

    public class StudentsController : Controller
    {
        private readonly StudentsService _studentsService;
        private readonly MessageService _messageService;

        public StudentsController(StudentsService studentsService, MessageService messageService)
        {
            _studentsService = studentsService;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentsService.GetAsync();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var student = await _studentsService.GetAsync(id);

            if (student is null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsMessage(string id)
        {
            var Student = await _studentsService.GetAsync(id);
            var messageId = Student.Messages;
            List<Message> messages = new List<Message>();
            if(messageId is not null)
            {
                foreach (var m in messageId)
                {
                    var message = await _messageService.GetAsync(m);
                    if (message is not null)
                    {
                        messages.Add(message);
                    }

                }
                if (messages is null)
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
            
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Students newStudent)
        {
          await _studentsService.CreateAsync(newStudent);
            
            return RedirectToAction("Index", "Students");
        }


        public async Task<IActionResult> Delete(string id)
        {
            var student = await _studentsService.GetAsync(id);
            var messageId = student.Messages;
            

            if (student is null)
            {
                return NotFound();
            }
            
   


            await _messageService.RemoveAsyncStudentMessages(id);
            await _studentsService.RemoveAsync(id);

            return RedirectToAction("Index", "Students");
        }


        //}

        //[HttpGet("Student messages by name and surname")]
        //public async Task<ActionResult<List<Message>>> GetStudentMessagesByNameAndSurname(string name, string surname)
        //{
        //    var Student = await _studentsService.GetAsyncByNameAndSurname(name,surname);
        //    var messageId = Student.Messages;
        //    List<Message> messages = new List<Message>();

        //    foreach (var m in messageId)
        //    {
        //        var message = await _messageService.GetAsync(m);
        //        if (message is not null)
        //        {
        //            messages.Add(message);
        //        }

        //    }
        //    if (messages is null)
        //    {
        //        return NotFound();
        //    }
        //    return messages;


        //}

        
        //[HttpPut("{id:length(24)}")]
        //public async Task<IActionResult> Update(string id, Students updatedStudent)
        //{
        //    var student = await _studentsService.GetAsync(id);

        //    if (student is null)
        //    {
        //        return NotFound();
        //    }

        //    updatedStudent.Id = student.Id;

        //    await _studentsService.UpdateAsync(id, updatedStudent);

        //    return NoContent();
        //}




    }
}
