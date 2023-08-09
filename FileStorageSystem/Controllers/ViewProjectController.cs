using FileStorageSystem.Data;
using FileStorageSystem.Model;
using FileStorageSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using NuGet.Packaging;
using System.IO;
using System.IO.Compression;

namespace FileStorageSystem.Controllers
{
    public class ViewProjectController : Controller
    {
        private readonly AppDbContext _context;
        public ViewProjectController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IList<FileModel> finalFiles=new List<FileModel>();
            var projects = _context.Projects.ToList();
            foreach (var project in projects)
            {
                var projectDirectory = Path.Combine("wwwroot", "Projects", projects[0].Id.ToString());
                IList<FileModel> files = FileModel.GetFiles(projectDirectory);
                finalFiles.AddRange(files);
            }
            
            return View(finalFiles);
        }
        public IActionResult ViewProject(int projectId)
        {
            // Retrieve project metadata from the database
            var project = _context.Projects.Find(projectId);

            // Assuming project files are stored in a directory named after the project ID
            var projectDirectory = Path.Combine("wwwroot", "Projects", projectId.ToString());

            IList<FileModel> files = FileModel.GetFiles(projectDirectory);

            // Get a list of file and folder names in the project directory
            var projectFiles = Directory.GetFileSystemEntries(projectDirectory, "*", SearchOption.AllDirectories)
                                       .Select(filePath => filePath.Replace(projectDirectory, "").Trim('\\', '/'))
                                       .ToList();

            return View(projectFiles);
        }
        [HttpPost]
        public IActionResult GetFiles(string path)
        {
            IList<FileModel> files = FileModel.GetFiles(path);
            ViewData["Parent"] = path;
            return PartialView("_Test");
        }
    }
}
