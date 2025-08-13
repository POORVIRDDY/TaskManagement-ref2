using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [SessionCheck]
    public class UploadController : Controller
    {
        private readonly TaskManagementContext db;

        public UploadController(TaskManagementContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                
                using (var memoryStream = new MemoryStream())
                {
                    await model.File.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();

                    var gnrlNote = new GnrlNote
                    {
                        NotesId = Guid.NewGuid(),
                        ObjectId = Guid.NewGuid(),
                        FileName = model.File.FileName,
                        Data = fileData,
                        LastModifiedOn = DateTime.Now,
                        
                    };

                    db.GnrlNotes.Add(gnrlNote);
                    await db.SaveChangesAsync();

                    var result = new ResultModel(model.File.FileName, model.File.Length, model.File.ContentType, MalwareStatus.Unknown, null);
                    return View("Result", result);
                }
            }

            ModelState.AddModelError("", "Please upload a valid file.");
            return View();
        }
    }
}
