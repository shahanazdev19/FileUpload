using FileStorageSystem.Data;
using FileStorageSystem.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace FileStorageSystem.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly AppDbContext _context;
        public FileUploadController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadProject(IFormFile zipFile)
        {
            if (ModelState.IsValid && zipFile != null)
            {
                // Save project metadata to the database
                var project = new Project
                {
                    Name = "Uploaded Project",
                    Description = "A project uploaded as a ZIP archive",                   
                };
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                // Save project ZIP file to local storage (change path as needed)
                var projectDirectory = Path.Combine("wwwroot", "Projects", project.Id.ToString());
                Directory.CreateDirectory(projectDirectory);
                var zipFilePath = Path.Combine(projectDirectory, "upload.zip");

                using (var fileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    await zipFile.CopyToAsync(fileStream);
                }

                // Extract the contents of the ZIP file
                var extractPath = Path.Combine(projectDirectory, "extracted");
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                return RedirectToAction("Index", "Home");
            }

            // If model is not valid or no ZIP file is provided, return to the upload view with errors
            return View("UploadView");
        }

        public async Task<IActionResult> UploadProject2(IFormFile zipFile)
        {
            if (ModelState.IsValid && zipFile != null)
            {
                // Save project metadata to the database
                var project = new Project
                {
                    Name = "Uploaded Project",
                    Description = "A project uploaded as a ZIP archive",
                };
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                // Save project ZIP file to local storage (change path as needed)
                var projectDirectory = Path.Combine("wwwroot", "Projects", project.Id.ToString());
                Directory.CreateDirectory(projectDirectory);
                var zipFilePath = Path.Combine(projectDirectory, "upload.zip");

                using (var fileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    await zipFile.CopyToAsync(fileStream);
                }

                // Extract the contents of the ZIP file
                var extractPath = Path.Combine(projectDirectory, "extracted");
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                var SavedprojectDirectory = Path.Combine("wwwroot", "Projects", project.Id.ToString());

                foreach (string filePath in Directory.GetFiles(SavedprojectDirectory, "*", SearchOption.AllDirectories))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);
                    string relativeFilePath = filePath.Remove(0, SavedprojectDirectory.Length);
                    var file = new UploadedFile
                    {
                        FileName = fileName,
                        FileData = fileBytes,
                        FilePath = relativeFilePath
                    };
                    _context.UploadedFiles.Add(file);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }

            // If model is not valid or no ZIP file is provided, return to the upload view with errors
            return View("UploadView");
        }
        public IActionResult ViewProject(int projectId)
        {
            // Retrieve project metadata from the database
            var project = _context.Projects.Find(projectId);

            // Assuming project files are stored in a directory named after the project ID
            var projectDirectory = Path.Combine("wwwroot", "Projects", projectId.ToString());

            // Get a list of file and folder names in the project directory
            var projectFiles = Directory.GetFileSystemEntries(projectDirectory, "*", SearchOption.AllDirectories)
                                       .Select(filePath => filePath.Replace(projectDirectory, "").Trim('\\', '/'))
                                       .ToList();

            return View(projectFiles);
        }
    }
}
