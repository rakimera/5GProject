using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class ProjectImageRepository : BaseRepository<ProjectImage>, IProjectImageRepository
{
    public ProjectImageRepository(Project5GDbContext dbContext) : base(dbContext)
    {
        
    }
    
    public void Delete(ProjectImage projectImage)
    {
        DbContext.ProjectImages.Remove(projectImage);
    }

    public async Task<string> SaveImage(string projectId, IFormFile uploadedFile)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure/Persistence/DataFiles", "ProjectImages");
        var fileExtension = Path.GetExtension(uploadedFile.FileName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
            
        var fileName = $"{projectId}_{DateTime.Now.Microsecond}{fileExtension}";
        string filePath = Path.Combine(folderPath, fileName);
            
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await uploadedFile.CopyToAsync(fileStream);
        }

        return filePath;
    }
}